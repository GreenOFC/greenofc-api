import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { HydratedDocument, Schema as MongooseSchema } from 'mongoose';

export type BannerDocument = HydratedDocument<Banner>;

@Schema({
  collection: 'Banner',
  timestamps: { createdAt: 'createdDate', updatedAt: 'modifiedDate' },
})
export class Banner {
  @Prop({ required: true })
  imageUrl: string;

  @Prop({ required: true })
  redirectUrl: string;

  @Prop({ type: Date, required: true })
  startDate: Date;

  @Prop({ type: Date })
  endDate?: Date;

  @Prop({ type: Boolean, default: false })
  isDeleted: boolean;

  @Prop({ type: MongooseSchema.Types.ObjectId, ref: 'Users', default: null })
  creator?: string | null;

  @Prop({ type: MongooseSchema.Types.ObjectId, ref: 'Users', default: null })
  modifier?: string | null;

  @Prop({ type: MongooseSchema.Types.ObjectId, ref: 'Users', default: null })
  deletedBy?: string | null;

  @Prop({ type: Date })
  deletedDate?: Date | null;
}

export const BannerSchema = SchemaFactory.createForClass(Banner);

BannerSchema.index({ startDate: 1 });
BannerSchema.index({ endDate: 1 });

