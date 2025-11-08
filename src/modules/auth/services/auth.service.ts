import {
  HttpException,
  HttpStatus,
  Injectable,
  Logger,
  UnauthorizedException,
} from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { InjectModel } from '@nestjs/mongoose';
import { JwtService } from '@nestjs/jwt';
import { randomBytes } from 'crypto';
import { Model } from 'mongoose';
import { UserStatus } from '../../../common/enums/user-status.enum';
import {
  createErrorResponse,
  createSuccessResponse,
} from '../../../common/dto/response.dto';
import type { ResponseContext } from '../../../common/dto/response.dto';
import { AuthInfoDto } from '../dto/auth-info.dto';
import { GetNotificationRequestDto } from '../dto/request-notification.dto';
import { RequestLoginInfoDto } from '../dto/request-login-info.dto';
import { ResponseLoginInfoDto } from '../dto/response-login-info.dto';
import { LoginDto } from '../dto/login.dto';
import { AuthRefreshService } from './auth-refresh.service';
import { MobileVersionService } from './mobile-version.service';
import { NotificationService } from './notification.service';
import { UserLoginService } from './user-login.service';
import { User, UserDocument } from '../schemas/user.schema';
import { RegisterUserRequestDto } from '../dto/register-user-request.dto';
import { RegisterUserResponseDto } from '../dto/register-user-response.dto';

@Injectable()
export class AuthService {
  private readonly logger = new Logger(AuthService.name);

  constructor(
    @InjectModel(User.name)
    private readonly userModel: Model<UserDocument>,
    private readonly jwtService: JwtService,
    private readonly configService: ConfigService,
    private readonly authRefreshService: AuthRefreshService,
    private readonly userLoginService: UserLoginService,
    private readonly mobileVersionService: MobileVersionService,
    private readonly notificationService: NotificationService,
  ) {}

  async login(login_dto: LoginDto): Promise<AuthInfoDto> {
    const loginDto = login_dto;

    const user = await this.userModel
      .findOne({
        $or: [{ user_name: loginDto.user_name }, { phone: loginDto.user_name }],
      })
      .lean();

    if (!user || user.user_password !== loginDto.user_password) {
      throw new UnauthorizedException(
        'Sai tài khoản hoặc mật khẩu, vui lòng nhập lại!',
      );
    }

    const token = await this.generateManagementJwt(user);
    const refreshToken = this.randomToken();

    await this.authRefreshService.upsertRefreshToken(
      user.user_name,
      refreshToken,
    );

    return {
      user_name: user.user_name,
      user_full_name: user.full_name,
      role_id: user.role_name,
      token,
      refresh_token: refreshToken,
      status: (user.status as UserStatus) ?? UserStatus.Init,
    };
  }

  async logout(user_id: string): Promise<void> {
    const userId = user_id;
    await this.userLoginService.clearRegistrationToken(userId);
  }

  async register(
    register_user_request_dto: RegisterUserRequestDto,
  ): Promise<ResponseContext<RegisterUserResponseDto>> {
    const dto = register_user_request_dto;
    try {
      const normalizedEmail = dto.user_email.toLowerCase();
      const existingUser = await this.userModel.findOne({
        $or: [{ user_email: normalizedEmail }, { phone: dto.phone }],
      });

      if (existingUser && existingUser.status !== UserStatus.Init) {
        if (existingUser.user_email === normalizedEmail) {
          throw new HttpException(
            createErrorResponse(`Email ${existingUser.user_email} đã tồn tại`),
            HttpStatus.BAD_REQUEST,
          );
        }

        if (existingUser.phone === dto.phone) {
          throw new HttpException(
            createErrorResponse(
              `Số điện thoại ${existingUser.phone} đã tồn tại`,
            ),
            HttpStatus.BAD_REQUEST,
          );
        }
      }

      const roleName = dto.role_name ?? existingUser?.role_name ?? 'SALE';
      const payload = this.getRegisterUserPayload(
        dto,
        normalizedEmail,
        roleName,
      );

      if (existingUser) {
        Object.assign(existingUser, payload);
        existingUser.is_active = existingUser.is_active ?? false;
        existingUser.status = existingUser.status ?? UserStatus.Init;
        existingUser.role_ids = existingUser.role_ids ?? [];

        await existingUser.save();

        return createSuccessResponse({ id: existingUser._id.toString() });
      }

      const userName = await this.generateUserName();

      const newUser = new this.userModel({
        user_name: userName,
        ...payload,
        is_active: false,
        status: UserStatus.Init,
        role_ids: [],
      });

      await newUser.save();

      return createSuccessResponse({ id: newUser._id.toString() });
    } catch (error) {
      this.logger.error(
        'Failed to register user',
        error instanceof Error ? error.stack : error,
      );
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Đăng ký thất bại, vui lòng thử lại sau'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  async mobileLogin(
    dto: RequestLoginInfoDto,
  ): Promise<ResponseContext<ResponseLoginInfoDto>> {
    const isLatest = await this.mobileVersionService.isLatestVersion(
      dto.os_type ?? '',
      dto.mobile_version,
    );
    if (!isLatest) {
      throw new HttpException(
        createErrorResponse('Phiên bản app đã cũ, vui lòng tải bản app mới!'),
        426,
      );
    }

    const user = await this.userModel
      .findOne({
        $or: [
          { user_name: dto.user_name },
          { phone: dto.user_name },
          { user_email: (dto.user_name ?? '').toLowerCase() },
        ],
      })
      .lean();

    if (!user) {
      throw new HttpException(
        createErrorResponse(`Không tìm thấy tài khoản ${dto.user_name}`),
        HttpStatus.BAD_REQUEST,
      );
    }

    if (user.user_password !== dto.password) {
      throw new HttpException(
        createErrorResponse('Sai mật khẩu, vui lòng nhập lại!'),
        HttpStatus.BAD_REQUEST,
      );
    }

    if (user.is_active === false) {
      throw new HttpException(
        createErrorResponse('Tài khoản đã bị khóa, vui lòng liên hệ IT!'),
        HttpStatus.FORBIDDEN,
      );
    }

    const token = await this.generateMobileJwt(dto, user);

    await this.userLoginService.upsertUserLogin({
      user_id: user._id?.toString(),
      user_name: dto.user_name,
      uuid: dto.uuid,
      os_type: dto.os_type,
      token,
      registration_token: dto.registration_token,
    });

    const notificationRequest: GetNotificationRequestDto = {
      user_id: user._id?.toString(),
      green_type: '',
      is_unread: true,
    };

    const unreadCount =
      await this.notificationService.countAsync(notificationRequest);

    const userId = ((user as any)?._id ?? user['id'])?.toString?.() ?? '';

    const response: ResponseLoginInfoDto = {
      user_id: userId,
      user_name: user.user_name,
      full_name: user.full_name,
      role_name: user.role_name,
      token,
      phone: user.phone,
      user_email: user.user_email,
      registration_token: dto.registration_token,
      mafc_code: user.mafc_code,
      ec_dsa_code: user.ec_dsa_code,
      status: (user.status as UserStatus) ?? UserStatus.Init,
      permissions: [],
      unread_noti: unreadCount,
    };

    return createSuccessResponse(response);
  }

  private async generateManagementJwt(user: Partial<User>): Promise<string> {
    const payload = {
      unique_name: user.user_name,
      email: user.user_email,
      family_name: user.full_name,
      nameid: user.id_card,
      role: user.role_name,
    };

    return this.jwtService.signAsync(payload, {
      secret: this.configService.get<string>('JWT_SECRET'),
      issuer: this.configService.get<string>('JWT_ISSUER'),
      audience: this.configService.get<string>('JWT_AUDIENCE'),
      expiresIn: '24h',
    });
  }

  private async generateMobileJwt(
    dto: RequestLoginInfoDto,
    user: Partial<User>,
  ): Promise<string> {
    const payload = {
      userName: dto.user_name,
      uuid: dto.uuid,
      ostype: dto.os_type,
      userId: ((user as any)?._id ?? user['id'])?.toString?.(),
      mobileVersion: dto.mobile_version,
      status: user.status,
    };

    return this.jwtService.signAsync(payload, {
      secret: this.configService.get<string>('JWT_SECRET'),
      issuer: this.configService.get<string>('JWT_ISSUER'),
      audience: this.configService.get<string>('JWT_AUDIENCE'),
      expiresIn: '1y',
    });
  }

  private randomToken(): string {
    return randomBytes(32).toString('hex');
  }

  private async generateUserName(): Promise<string> {
    for (let attempt = 0; attempt < 10; attempt += 1) {
      const candidate = `GRS${Math.floor(Math.random() * 1_000_000)
        .toString()
        .padStart(6, '0')}`;

      const existed = await this.userModel.exists({ user_name: candidate });
      if (!existed) {
        return candidate;
      }
    }

    return `GRS${Date.now()}`;
  }

  async findUserById(user_id: string) {
    return this.userModel.findById(user_id).lean();
  }

  private getRegisterUserPayload(
    dto: RegisterUserRequestDto,
    normalizedEmail: string,
    roleName: string,
  ) {
    return {
      full_name: dto.full_name,
      id_card: dto.id_card,
      phone: dto.phone,
      user_email: normalizedEmail,
      user_password: dto.user_password,
      role_name: roleName,
      team_lead_user_id: dto.team_lead_user_id,
      pos_id: dto.pos_id,
      referral_code: dto.referral_code,
    };
  }
}
