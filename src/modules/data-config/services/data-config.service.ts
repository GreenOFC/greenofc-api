import { Injectable, Logger } from '@nestjs/common';

@Injectable()
export class DataConfigService {
  private readonly logger = new Logger(DataConfigService.name);

  async getDataConfigs(green_type: string, type: string): Promise<unknown[]> {
    this.logger.debug('getDataConfigs stub', { green_type, type });
    return [];
  }
}

