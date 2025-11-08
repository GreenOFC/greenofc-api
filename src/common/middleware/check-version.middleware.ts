import { Injectable, Logger, NestMiddleware } from '@nestjs/common';
import { Request, Response, NextFunction } from 'express';
import { MobileVersionService } from '../../modules/auth/services/mobile-version.service';
import { createErrorResponse } from '../dto/response.dto';

@Injectable()
export class CheckVersionMiddleware implements NestMiddleware {
  private readonly logger = new Logger(CheckVersionMiddleware.name);

  constructor(private readonly mobileVersionService: MobileVersionService) {}

  async use(request: Request, response: Response, next_function: NextFunction): Promise<void> {
    try {
      const user_payload = (request as any).user;
      if (!user_payload) {
        return next_function();
      }

      const os_type = user_payload?.ostype ?? user_payload?.os_type ?? '';
      const mobile_version = user_payload?.mobileVersion ?? user_payload?.mobile_version ?? '';

      const is_latest = await this.mobileVersionService.isLatestVersion(os_type, mobile_version);
      if (!is_latest) {
        response.status(426).json(
          createErrorResponse('Phiên bản app đã cũ, vui lòng tải bản app mới!'),
        );
        return;
      }

      return next_function();
    } catch (error) {
      this.logger.error('Check version failed', error instanceof Error ? error.stack : error);
      return next_function();
    }
  }
}

