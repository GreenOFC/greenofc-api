import { Injectable, Logger } from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import { FilterQuery, Model } from 'mongoose';
import { PagingResponse } from '../../../common/dto/pagination.dto';
import { GetBannerRequest } from '../dto/get-banner.request';
import { GetBannerResponse } from '../dto/get-banner.response';
import { Banner, BannerDocument } from '../schemas/banner.schema';

@Injectable()
export class BannerService {
  private readonly logger = new Logger(BannerService.name);

  constructor(
    @InjectModel(Banner.name)
    private readonly bannerModel: Model<BannerDocument>,
  ) {}

  async getPaged(query: GetBannerRequest): Promise<PagingResponse<GetBannerResponse>> {
    try {
      const filter = this.buildFilter(query);
      const [banners, totalRecord] = await Promise.all([
        this.bannerModel
          .find(filter)
          .sort({ createdDate: -1 })
          .skip((query.pageIndex - 1) * query.pageSize)
          .limit(query.pageSize)
          .lean(),
        this.bannerModel.countDocuments(filter),
      ]);

      return {
        totalRecord,
        data: banners.map((banner) => this.mapToResponse(banner)),
      };
    } catch (error) {
      this.logger.error('Failed to fetch banners', error instanceof Error ? error.stack : error);
      throw error;
    }
  }

  async getAllUrls(): Promise<string[]> {
    try {
      const banners = await this.bannerModel
        .find({ isDeleted: false })
        .select({ imageUrl: 1, _id: 0 })
        .lean();

      return banners.map((banner) => banner.imageUrl).filter(Boolean);
    } catch (error) {
      this.logger.error('Failed to fetch banner urls', error instanceof Error ? error.stack : error);
      throw error;
    }
  }

  private buildFilter(query: GetBannerRequest): FilterQuery<BannerDocument> {
    const filter: FilterQuery<BannerDocument> = { isDeleted: false };

    if (query.startDate) {
      const start = new Date(query.startDate);
      const end = new Date(start);
      end.setDate(end.getDate() + 1);
      filter.startDate = { $gte: start, $lt: end };
    }

    if (query.endDate) {
      const start = new Date(query.endDate);
      const end = new Date(start);
      end.setDate(end.getDate() + 1);
      filter.endDate = { $gte: start, $lt: end };
    }

    return filter;
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private mapToResponse(document: any): GetBannerResponse {
    return {
      id: document._id?.toString() ?? '',
      imageUrl: document.imageUrl ?? '',
      redirectUrl: document.redirectUrl ?? '',
      startDate: document.startDate ? new Date(document.startDate) : new Date(),
      endDate: document.endDate ? new Date(document.endDate) : null,
      createdDate: document['createdDate'] ? new Date(document['createdDate']) : new Date(),
    };
  }
}

