import { Controller, Get, InternalServerErrorException, Logger } from '@nestjs/common';
import { ResponseStatus } from '../../common/constants/response.constant';
import { createSuccessResponse } from '../../common/dto/response.dto';
import type { ResponseContext } from '../../common/dto/response.dto';
import type { PaymeSdkConfigResponse } from './dto/payme-sdk-config.response';
import { AppConfigService } from './services/app-config.service';

@Controller('config')
export class AppConfigController {
  private readonly logger = new Logger(AppConfigController.name);

  constructor(private readonly appConfigService: AppConfigService) {}

  @Get('banner')
  async getBanner(): Promise<ResponseContext<string[]>> {
    try {
      const data = await this.appConfigService.getBannerUrls();
      return createSuccessResponse(data);
    } catch (error) {
      this.logger.error('Failed to load banner configuration', error instanceof Error ? error.stack : error);
      throw new InternalServerErrorException({
        status: ResponseStatus.ERROR,
        message: error instanceof Error ? error.message : 'Internal server error',
      });
    }
  }

  @Get('env')
  getEnv(): ResponseContext<string> {
    try {
      const data = this.appConfigService.getMcHost();
      return createSuccessResponse(data);
    } catch (error) {
      this.logger.error('Failed to load environment configuration', error instanceof Error ? error.stack : error);
      throw new InternalServerErrorException({
        status: ResponseStatus.ERROR,
        message: error instanceof Error ? error.message : 'Internal server error',
      });
    }
  }

  @Get('payme-sdk')
  getPaymeSdk(): ResponseContext<PaymeSdkConfigResponse> {
    try {
      const data = this.appConfigService.getPaymeSdkConfig();
      return createSuccessResponse(data);
    } catch (error) {
      this.logger.error('Failed to load Payme SDK configuration', error instanceof Error ? error.stack : error);
      throw new InternalServerErrorException({
        status: ResponseStatus.ERROR,
        message: error instanceof Error ? error.message : 'Internal server error',
      });
    }
  }
}

