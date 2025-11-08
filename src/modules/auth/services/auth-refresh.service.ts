import { Injectable, Logger } from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import { Model } from 'mongoose';
import { AuthRefresh, AuthRefreshDocument } from '../schemas/auth-refresh.schema';

@Injectable()
export class AuthRefreshService {
  private readonly logger = new Logger(AuthRefreshService.name);

  constructor(
    @InjectModel(AuthRefresh.name)
    private readonly authRefreshModel: Model<AuthRefreshDocument>,
  ) {}

  async upsertRefreshToken(user_name: string, refresh_token: string): Promise<void> {
    const userName = user_name;
    const refreshToken = refresh_token;
    try {
      await this.authRefreshModel.updateOne(
        { user_name: userName },
        { $set: { refresh_token: refreshToken } },
        { upsert: true },
      );
    } catch (error) {
      this.logger.error('Failed to upsert refresh token', error instanceof Error ? error.stack : error);
      throw error;
    }
  }

  async removeRefreshToken(user_name: string): Promise<void> {
    const userName = user_name;
    try {
      await this.authRefreshModel.deleteOne({ user_name: userName });
    } catch (error) {
      this.logger.error('Failed to delete refresh token', error instanceof Error ? error.stack : error);
      throw error;
    }
  }

  async getUserNameByRefreshToken(refresh_token: string): Promise<string | null> {
    const refreshToken = refresh_token;
    const record = await this.authRefreshModel.findOne({ refresh_token: refreshToken }).lean();
    return record?.user_name ?? null;
  }
}

