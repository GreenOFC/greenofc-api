import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { HydratedDocument } from 'mongoose';

export type MobileVersionDocument = HydratedDocument<MobileVersion>;

@Schema({ collection: 'MobileVersion' })
export class MobileVersion {
  @Prop({ required: true })
  type: string;

  @Prop({ required: true })
  version: string;
}

export const MobileVersionSchema = SchemaFactory.createForClass(MobileVersion);
MobileVersionSchema.index({ type: 1, version: 1 }, { unique: true });

