import { Injectable, Logger } from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import { Model } from 'mongoose';
import { UserLogin, UserLoginDocument } from '../schemas/user-login.schema';

@Injectable()
export class UserLoginService {
  private readonly logger = new Logger(UserLoginService.name);

  constructor(
    @InjectModel(UserLogin.name)
    private readonly userLoginModel: Model<UserLoginDocument>,
  ) {}

  async upsertUserLogin(user_login_payload: Partial<UserLogin>): Promise<void> {
    const payload = user_login_payload;
    try {
      if (!payload.user_id) {
        throw new Error('user_id is required to upsert user login');
      }

      await this.userLoginModel.updateOne(
        { user_id: payload.user_id },
        { $set: payload },
        { upsert: true },
      );
    } catch (error) {
      this.logger.error('Failed to upsert user login', error instanceof Error ? error.stack : error);
      throw error;
    }
  }

  async getUserLoginByToken(token: string): Promise<UserLogin | null> {
    try {
      return this.userLoginModel.findOne({ token }).lean();
    } catch (error) {
      this.logger.error('Failed to find user login by token', error instanceof Error ? error.stack : error);
      throw error;
    }
  }

  async clearRegistrationToken(user_id: string): Promise<void> {
    const userId = user_id;
    try {
      await this.userLoginModel.updateOne(
        { user_id: userId },
        {
          $set: {
            registration_token: '',
            token: '',
          },
        },
      );
    } catch (error) {
      this.logger.error('Failed to clear registration token', error instanceof Error ? error.stack : error);
      throw error;
    }
  }
}

