import { Controller, Get, HttpException, HttpStatus, Query } from '@nestjs/common';
import { ApiTags, ApiQuery } from '@nestjs/swagger';
import { createErrorResponse, createSuccessResponse } from '../../common/dto/response.dto';
import { DataConfigService } from './services/data-config.service';

@ApiTags('data-configs')
@Controller('data-configs')
export class DataConfigController {
  constructor(private readonly dataConfigService: DataConfigService) {}

  @Get()
  @ApiQuery({ name: 'green_type', required: false })
  @ApiQuery({ name: 'type', required: false })
  async getDataConfigs(
    @Query('green_type') green_type?: string,
    @Query('type') type?: string,
  ) {
    try {
      const configs = await this.dataConfigService.getDataConfigs(green_type ?? '', type ?? '');
      return createSuccessResponse(configs);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể lấy cấu hình dữ liệu'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }
}

