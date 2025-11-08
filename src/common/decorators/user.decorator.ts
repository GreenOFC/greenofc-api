import { createParamDecorator, ExecutionContext } from '@nestjs/common';
import { Types } from 'mongoose';

export interface IUser {
  readonly _id: Types.ObjectId;
  readonly email: string;
  readonly user_name: string;
  readonly is_active_ec: boolean;
  readonly full_name: string;
  readonly user_email: string;
  readonly phone: string;
  readonly id_card: string;
  readonly user_password: string;
  readonly role_name: string;
  readonly is_active: boolean;
  readonly status: string;
  readonly role_ids: string[];
  readonly is_deleted: boolean;
  readonly team_lead_user_id: string;
  readonly pos_id: string;
  readonly referral_code: string;
}

export const CurrentUser = createParamDecorator(
  async (_: any, ctx: ExecutionContext): Promise<IUser> => {
    const request = ctx.switchToHttp().getRequest();

    return request.user;
  },
);
