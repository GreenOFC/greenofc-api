import { Module } from '@nestjs/common';
import { ConfigModule, ConfigService } from '@nestjs/config';
import { JwtModule } from '@nestjs/jwt';
import { MongooseModule } from '@nestjs/mongoose';
import { AuthController } from './auth.controller';
import { AuthService } from './services/auth.service';
import { AuthRefreshService } from './services/auth-refresh.service';
import { MobileVersionService } from './services/mobile-version.service';
import { NotificationService } from './services/notification.service';
import { UserLoginService } from './services/user-login.service';
import { AuthRefresh, AuthRefreshSchema } from './schemas/auth-refresh.schema';
import {
  MobileVersion,
  MobileVersionSchema,
} from './schemas/mobile-version.schema';
import {
  Notification,
  NotificationSchema,
} from './schemas/notification.schema';
import { User, UserSchema } from './schemas/user.schema';
import { UserLogin, UserLoginSchema } from './schemas/user-login.schema';

@Module({
  imports: [
    ConfigModule,
    JwtModule.registerAsync({
      imports: [ConfigModule],
      inject: [ConfigService],
      useFactory: (configService: ConfigService) => ({
        secret: configService.get<string>('JWT_SECRET') ?? 'development-secret',
        signOptions: {
          issuer: configService.get<string>('JWT_ISSUER'),
          audience: configService.get<string>('JWT_AUDIENCE'),
        },
      }),
    }),
    MongooseModule.forFeature([
      { name: User.name, schema: UserSchema },
      { name: UserLogin.name, schema: UserLoginSchema },
      { name: AuthRefresh.name, schema: AuthRefreshSchema },
      { name: MobileVersion.name, schema: MobileVersionSchema },
      { name: Notification.name, schema: NotificationSchema },
    ]),
  ],
  controllers: [AuthController],
  providers: [
    AuthService,
    AuthRefreshService,
    UserLoginService,
    MobileVersionService,
    NotificationService,
  ],
  exports: [AuthService, UserLoginService, MobileVersionService],
})
export class AuthModule {}
