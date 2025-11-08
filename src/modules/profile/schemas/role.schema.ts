import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { HydratedDocument } from 'mongoose';

export type RoleDocument = HydratedDocument<Role>;

@Schema({ collection: 'Roles' })
export class Role {
  @Prop({ required: true })
  role_name: string;

  @Prop()
  role_description?: string;

  @Prop({ type: [String], default: [] })
  permission_ids: string[];
}

export const RoleSchema = SchemaFactory.createForClass(Role);

