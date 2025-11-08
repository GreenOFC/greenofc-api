import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { HydratedDocument, SchemaTypes } from 'mongoose';

export type AppConfigDocument = HydratedDocument<AppConfig>;

@Schema({ collection: 'Config' })
export class AppConfig {
  @Prop({ required: true, unique: true })
  key: string;

  @Prop({ type: SchemaTypes.Mixed })
  value: unknown;
}

export const AppConfigSchema = SchemaFactory.createForClass(AppConfig);
AppConfigSchema.index({ key: 1 }, { unique: true });

