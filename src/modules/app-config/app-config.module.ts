import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { BannerModule } from '../banner/banner.module';
import { AppConfigController } from './app-config.controller';
import { AppConfig, AppConfigSchema } from './schemas/app-config.schema';
import { AppConfigService } from './services/app-config.service';
import { ConfigDatabaseService } from './services/config-database.service';

@Module({
  imports: [
    MongooseModule.forFeature([{ name: AppConfig.name, schema: AppConfigSchema }]),
    BannerModule,
  ],
  controllers: [AppConfigController],
  providers: [AppConfigService, ConfigDatabaseService],
  exports: [ConfigDatabaseService],
})
export class AppConfigModule {}

