import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { HydratedDocument } from 'mongoose';

export type AuthRefreshDocument = HydratedDocument<AuthRefresh>;

@Schema({ collection: 'AuthRefresh' })
export class AuthRefresh {
  @Prop({ required: true, unique: true })
  user_name: string;

  @Prop({ required: true })
  refresh_token: string;
}

export const AuthRefreshSchema = SchemaFactory.createForClass(AuthRefresh);
AuthRefreshSchema.index({ refresh_token: 1 });

