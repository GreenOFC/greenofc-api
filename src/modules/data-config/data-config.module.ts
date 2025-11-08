import { Module } from '@nestjs/common';
import { DataConfigController } from './data-config.controller';
import { DataConfigService } from './services/data-config.service';

@Module({
  controllers: [DataConfigController],
  providers: [DataConfigService],
})
export class DataConfigModule {}

