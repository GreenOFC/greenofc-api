import { Injectable, Logger } from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import { Model } from 'mongoose';
import { AppConfig, AppConfigDocument } from '../schemas/app-config.schema';

@Injectable()
export class ConfigDatabaseService {
  private readonly logger = new Logger(ConfigDatabaseService.name);

  constructor(
    @InjectModel(AppConfig.name)
    private readonly configModel: Model<AppConfigDocument>,
  ) {}

  async findOneByKey<T>(key: string): Promise<T | null> {
    try {
      const config = await this.configModel.findOne({ key }).lean();
      return (config?.value ?? null) as T | null;
    } catch (error) {
      this.logger.error(`Failed to load config ${key}`, error instanceof Error ? error.stack : error);
      throw error;
    }
  }

  async upsert<T>(key: string, value: T): Promise<void> {
    try {
      await this.configModel.updateOne(
        { key },
        { $set: { value } },
        { upsert: true },
      );
    } catch (error) {
      this.logger.error(`Failed to upsert config ${key}`, error instanceof Error ? error.stack : error);
      throw error;
    }
  }
}

