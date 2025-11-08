import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { HydratedDocument } from 'mongoose';
import { UserStatus } from '../../../common/enums/user-status.enum';

export type UserDocument = HydratedDocument<User>;

@Schema({ collection: 'Users' })
export class User {
  @Prop({ required: true, unique: true })
  user_name: string;

  @Prop()
  mafc_code?: string;

  @Prop()
  ec_dsa_code?: string;

  @Prop()
  ec_sale_code?: string;

  @Prop({ default: false })
  is_active_ec?: boolean;

  @Prop({ required: true })
  full_name: string;

  @Prop({ required: true, lowercase: true })
  user_email: string;

  @Prop({ required: true })
  phone: string;

  @Prop()
  id_card?: string;

  @Prop({ required: true })
  user_password: string;

  @Prop({ required: true })
  role_name: string;

  @Prop({ default: false })
  is_active: boolean;

  @Prop({ type: String, enum: UserStatus, default: UserStatus.Init })
  status: UserStatus;

  @Prop({ type: [String], default: [] })
  role_ids: string[];

  @Prop({ default: false })
  is_deleted?: boolean;

  @Prop()
  team_lead_user_id?: string;

  @Prop()
  pos_id?: string;

  @Prop()
  referral_code?: string;
}

export const UserSchema = SchemaFactory.createForClass(User);
UserSchema.index({ user_name: 1 }, { unique: true });
UserSchema.index({ phone: 1 });
UserSchema.index({ user_email: 1 });
