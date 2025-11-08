import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { HydratedDocument } from 'mongoose';

export type NotificationDocument = HydratedDocument<Notification>;

@Schema({ collection: 'Notification', timestamps: true })
export class Notification {
  @Prop({ required: true })
  user_id: string;

  @Prop({ default: false })
  is_read: boolean;

  @Prop({ default: '' })
  green_type: string;
}

export const NotificationSchema = SchemaFactory.createForClass(Notification);
NotificationSchema.index({ user_id: 1, is_read: 1, green_type: 1 });

