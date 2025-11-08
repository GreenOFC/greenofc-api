import {
  Body,
  Controller,
  Get,
  HttpCode,
  HttpException,
  HttpStatus,
  Post,
  Put,
} from '@nestjs/common';
import {
  ResponseContext,
  createErrorResponse,
} from '../../common/dto/response.dto';
import { AuthInfoDto } from './dto/auth-info.dto';
import { LoginDto } from './dto/login.dto';
import { RequestLoginInfoDto } from './dto/request-login-info.dto';
import { ResponseLoginInfoDto } from './dto/response-login-info.dto';
import { AuthService } from './services/auth.service';
import { RegisterUserRequestDto } from './dto/register-user-request.dto';
import { RegisterUserResponseDto } from './dto/register-user-response.dto';

@Controller('auth')
export class AuthController {
  constructor(private readonly authService: AuthService) {}

  @Post('login')
  async login(@Body() login_dto: LoginDto): Promise<AuthInfoDto> {
    const loginDto = login_dto;
    return this.authService.login(loginDto);
  }

  @Put('logout')
  @HttpCode(HttpStatus.OK)
  async logout(@Body('user_id') user_id?: string): Promise<void> {
    const userId = user_id;
    if (!userId) {
      throw new HttpException(
        createErrorResponse('userId is required'),
        HttpStatus.BAD_REQUEST,
      );
    }

    await this.authService.logout(userId);
  }

  @Post('userlogin')
  async userLogin(
    @Body() request_login_info_dto: any,
  ): Promise<ResponseContext<ResponseLoginInfoDto>> {
    console.log('request_login_info_dto', request_login_info_dto);
    return this.authService.mobileLogin(request_login_info_dto);
  }

  @Post('register')
  async register(
    @Body() register_user_request_dto: RegisterUserRequestDto,
  ): Promise<ResponseContext<RegisterUserResponseDto>> {
    const dto = register_user_request_dto;
    return this.authService.register(dto);
  }

  @Post('verify-otp')
  async verifyOtp(): Promise<void> {
    throw new HttpException('Not implemented yet', HttpStatus.NOT_IMPLEMENTED);
  }

  @Post('send-otp')
  async sendOtp(): Promise<void> {
    throw new HttpException('Not implemented yet', HttpStatus.NOT_IMPLEMENTED);
  }

  @Post('send-reset-password')
  async sendResetPassword(): Promise<void> {
    throw new HttpException('Not implemented yet', HttpStatus.NOT_IMPLEMENTED);
  }

  @Post('send-confirm-otp')
  async sendConfirmOtp(): Promise<void> {
    throw new HttpException('Not implemented yet', HttpStatus.NOT_IMPLEMENTED);
  }

  @Post('set-password')
  async setPassword(): Promise<void> {
    throw new HttpException('Not implemented yet', HttpStatus.NOT_IMPLEMENTED);
  }

  @Get('pos')
  async getPos(): Promise<void> {
    throw new HttpException('Not implemented yet', HttpStatus.NOT_IMPLEMENTED);
  }

  @Get('team-leads')
  async getTeamLeads(): Promise<void> {
    throw new HttpException('Not implemented yet', HttpStatus.NOT_IMPLEMENTED);
  }

  @Get('asm')
  async getAsm(): Promise<void> {
    throw new HttpException('Not implemented yet', HttpStatus.NOT_IMPLEMENTED);
  }

  @Get('referral')
  async getReferral(): Promise<void> {
    throw new HttpException('Not implemented yet', HttpStatus.NOT_IMPLEMENTED);
  }
}
