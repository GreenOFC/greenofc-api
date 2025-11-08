import { Injectable, Logger } from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import { FilterQuery, Model } from 'mongoose';
import { GetNotificationRequestDto } from '../dto/request-notification.dto';
import { Notification, NotificationDocument } from '../schemas/notification.schema';

@Injectable()
export class NotificationService {
  private readonly logger = new Logger(NotificationService.name);

  constructor(
    @InjectModel(Notification.name)
    private readonly notificationModel: Model<NotificationDocument>,
  ) {}

  async countAsync(request: GetNotificationRequestDto): Promise<number> {
    try {
      const filter: FilterQuery<NotificationDocument> = { userId: request.user_id };

      if (typeof request.is_unread === 'boolean') {
        filter.isRead = !request.is_unread ? { $in: [true, false] } : false;
        if (request.is_unread) {
          filter.isRead = false;
        }
      }

      if (request.green_type) {
        filter.greenType = request.green_type;
      }

      return this.notificationModel.countDocuments(filter);
    } catch (error) {
      this.logger.error('Failed to count notifications', error instanceof Error ? error.stack : error);
      throw error;
    }
  }
}

