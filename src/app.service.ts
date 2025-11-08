import { Injectable } from '@nestjs/common';
import { createSuccessResponse, ResponseContext } from './common/dto/response.dto';

@Injectable()
export class AppService {
  getHealth(): ResponseContext {
    return createSuccessResponse({ status: 'ok' });
  }
}
