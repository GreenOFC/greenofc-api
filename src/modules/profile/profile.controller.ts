import {
  Body,
  Controller,
  ForbiddenException,
  Get,
  HttpException,
  HttpStatus,
  Logger,
  Param,
  Put,
  Req,
} from '@nestjs/common';
import {
  createErrorResponse,
  createSuccessResponse,
  type ResponseContext,
} from '../../common/dto/response.dto';
import { ChangePasswordProfileRequest } from './dto/change-password-profile.request';
import type { UserProfileResponse } from './dto/user-profile.response';
import { ProfileService } from './services/profile.service';
import { CurrentUser } from '../../common/decorators';
import type { IUser } from '../../common/decorators';

@Controller('profile')
export class ProfileController {
  private readonly logger = new Logger(ProfileController.name);

  constructor(private readonly profileService: ProfileService) {}

  @Get()
  async getDetail(
    @CurrentUser() currentUser: IUser,
  ): Promise<ResponseContext<UserProfileResponse>> {
    try {
      const profile = await this.profileService.getProfile(
        currentUser._id.toString(),
      );
      return createSuccessResponse(profile);
    } catch (error) {
      this.logger.error(
        'Failed to load user profile',
        error instanceof Error ? error.stack : error,
      );

      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Lỗi hệ thống, vui lòng liên hệ IT!'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Put(':id/change-password')
  async changePassword(
    @CurrentUser() currentUser: IUser,
    @Param('id') id: string,
    @Body() payload: ChangePasswordProfileRequest,
  ): Promise<ResponseContext<void>> {
    try {
      await this.profileService.changePassword(
        currentUser._id.toString(),
        payload,
      );
      return createSuccessResponse();
    } catch (error) {
      this.logger.error(
        'Failed to change password',
        error instanceof Error ? error.stack : error,
      );

      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Lỗi hệ thống, vui lòng liên hệ IT!'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }
}
