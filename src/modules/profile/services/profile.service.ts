import {
  HttpException,
  HttpStatus,
  Injectable,
  Logger,
} from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import { Model, Types } from 'mongoose';
import { createErrorResponse } from '../../../common/dto/response.dto';
import { User, UserDocument } from '../../auth/schemas/user.schema';
import {
  Permission,
  PermissionDocument,
} from '../schemas/permission.schema';
import { Role, RoleDocument } from '../schemas/role.schema';
import {
  type ChangePasswordProfileRequest,
} from '../dto/change-password-profile.request';
import {
  type TeamMemberDto,
  type UserProfileResponse,
} from '../dto/user-profile.response';

@Injectable()
export class ProfileService {
  private readonly logger = new Logger(ProfileService.name);

  constructor(
    @InjectModel(User.name)
    private readonly userModel: Model<UserDocument>,
    @InjectModel(Role.name)
    private readonly roleModel: Model<RoleDocument>,
    @InjectModel(Permission.name)
    private readonly permissionModel: Model<PermissionDocument>,
  ) {}

  async getProfile(userId: string): Promise<UserProfileResponse> {
    try {
      const user = await this.userModel.findById(userId).lean();

      if (!user) {
        throw new HttpException(
          createErrorResponse('Không tìm thấy user'),
          HttpStatus.BAD_REQUEST,
        );
      }

      const permissions = await this.resolvePermissions(
        (user as any)?.role_ids ?? [],
      );

      const teamLeadInfo = (user as any)?.team_lead_info;
      let teamMembers: TeamMemberDto[] | undefined;
      if (teamLeadInfo?.id) {
        const members = await this.userModel
          .find({ 'team_lead_info.id': teamLeadInfo.id })
          .select(['user_name', 'mafc_code', 'full_name', 'id_card'])
          .lean();

        teamMembers = (members ?? []).reduce<TeamMemberDto[]>(
          (acc, member) => {
            const rawId = (member as any)?._id ?? (member as any)?.id;
            const memberId = typeof rawId?.toString === 'function' ? rawId.toString() : '';
            if (!memberId) {
              return acc;
            }

            acc.push({
              id: memberId,
              user_name: (member as any)?.user_name ?? '',
              mafc_code: (member as any)?.mafc_code,
              full_name: (member as any)?.full_name,
              id_card: (member as any)?.id_card,
            });

            return acc;
          },
          [],
        );
      }

      return this.mapProfile(user, permissions, teamMembers);
    } catch (error) {
      this.logger.error(
        'Failed to get profile',
        error instanceof Error ? error.stack : error,
      );

      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể tải thông tin người dùng'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  async changePassword(
    userId: string,
    payload: ChangePasswordProfileRequest,
  ): Promise<void> {
    try {
      const user = await this.userModel.findById(userId);
      if (!user) {
        throw new HttpException(
          createErrorResponse('Không tìm thấy user'),
          HttpStatus.BAD_REQUEST,
        );
      }

      if (user.user_password !== payload.old_password) {
        throw new HttpException(
          createErrorResponse('Mật khẩu cũ không chính xác!'),
          HttpStatus.BAD_REQUEST,
        );
      }

      if (payload.old_password === payload.new_password) {
        throw new HttpException(
          createErrorResponse('Mật khẩu mới phải khác mật khẩu hiện tại'),
          HttpStatus.BAD_REQUEST,
        );
      }

      user.user_password = payload.new_password;
      user.set('modifier', userId);
      user.set('modified_date', new Date());
      await user.save();
    } catch (error) {
      this.logger.error(
        'Failed to change password',
        error instanceof Error ? error.stack : error,
      );

      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Đổi mật khẩu thất bại, vui lòng thử lại sau'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  private async resolvePermissions(roleIds: unknown): Promise<string[]> {
    const ids = Array.isArray(roleIds)
      ? (roleIds as unknown[])
          .map((id) => {
            if (typeof id === 'string' && Types.ObjectId.isValid(id)) {
              return new Types.ObjectId(id);
            }
            if (
              typeof (id as any)?._id === 'string' &&
              Types.ObjectId.isValid((id as any)._id)
            ) {
              return new Types.ObjectId((id as any)._id);
            }
            return null;
          })
          .filter(
            (id): id is Types.ObjectId =>
              id instanceof Types.ObjectId,
          )
      : [];

    if (ids.length === 0) {
      return [];
    }

    const roles = await this.roleModel
      .find({ _id: { $in: ids } })
      .select(['permission_ids'])
      .lean();

    const permissionIds = [
      ...new Set(
        roles.flatMap((role) => (role as any)?.permission_ids ?? []),
      ),
    ]
      .filter((id) => Types.ObjectId.isValid(id))
      .map((id) => new Types.ObjectId(id));

    if (permissionIds.length === 0) {
      return [];
    }

    const permissions = await this.permissionModel
      .find({ _id: { $in: permissionIds } })
      .select(['value'])
      .lean();

    return [
      ...new Set(
        permissions
          .map((permission) => (permission as any)?.value)
          .filter(
            (value): value is string =>
              typeof value === 'string' && value.length > 0,
          ),
      ),
    ];
  }

  private mapProfile(
    user: any,
    permissions: string[],
    teamMembers?: TeamMemberDto[],
  ): UserProfileResponse {
    const id =
      user?._id?.toString?.() ?? user?.id ?? String(user?._id ?? '');

    const createdDate =
      user?.created_date ??
      user?.createdDate ??
      (user?.created_at ?? user?.createdAt);
    const modifiedDate =
      user?.modified_date ??
      user?.modifiedDate ??
      (user?.modified_at ?? user?.modifiedAt);

    const teamLeadInfo = user?.team_lead_info
      ? {
          id: user?.team_lead_info?.id,
          user_name: user?.team_lead_info?.user_name,
          full_name: user?.team_lead_info?.full_name,
          team_members: teamMembers,
        }
      : null;

    return {
      id,
      user_name: user?.user_name,
      ec_dsa_code: user?.ec_dsa_code,
      ec_sale_code: user?.ec_sale_code,
      mafc_code: user?.mafc_code,
      full_name: user?.full_name,
      user_email: user?.user_email,
      phone: user?.phone,
      id_card: user?.id_card,
      old_id_card: user?.old_id_card,
      role_name: user?.role_name,
      status: user?.status,
      role_ids: Array.isArray(user?.role_ids)
        ? (user.role_ids as unknown[])
            .map((roleId) =>
              typeof roleId === 'string'
                ? roleId
                : (roleId as any)?.toString?.() ?? '',
            )
            .filter((roleId): roleId is string => !!roleId)
        : [],
      team_lead_info: teamLeadInfo,
      is_active: user?.is_active,
      created_date: createdDate,
      modified_date: modifiedDate,
      user_suspension_histories:
        user?.user_suspension_histories ?? [],
      permissions,
    };
  }
}

