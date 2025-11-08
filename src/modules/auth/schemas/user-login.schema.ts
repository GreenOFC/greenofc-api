import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { HydratedDocument } from 'mongoose';

export type UserLoginDocument = HydratedDocument<UserLogin>;

@Schema({ collection: 'UserLogin', timestamps: { createdAt: 'createdDate', updatedAt: 'modifiedDate' } })
export class UserLogin {
  @Prop({ type: String, required: true })
  user_id: string;

  @Prop({ required: true })
  user_name: string;

  @Prop()
  uuid?: string;

  @Prop()
  os_type?: string;

  @Prop()
  token?: string;

  @Prop()
  registration_token?: string;
}

export const UserLoginSchema = SchemaFactory.createForClass(UserLogin);
UserLoginSchema.index({ user_id: 1 }, { unique: true, sparse: true });

