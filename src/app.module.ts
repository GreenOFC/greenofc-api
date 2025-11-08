import {
  MiddlewareConsumer,
  Module,
  NestModule,
  RequestMethod,
} from '@nestjs/common';
import { ConfigModule as NestConfigModule } from '@nestjs/config';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import configuration from './common/config/configuration';
import { validateEnv } from './common/config/env.validation';
import { DatabaseModule } from './common/database/database.module';
import { AppConfigModule } from './modules/app-config/app-config.module';
import { BannerModule } from './modules/banner/banner.module';
import { AuthModule } from './modules/auth/auth.module';
import { CheckVersionMiddleware } from './common/middleware/check-version.middleware';
import { CheckAccessMiddleware } from './common/middleware/check-access.middleware';
import { SetPermissionMiddleware } from './common/middleware/set-permission.middleware';
import { CustomerModule } from './modules/customer/customer.module';
import { DataConfigModule } from './modules/data-config/data-config.module';
import { ProductCategoryModule } from './modules/product-category/product-category.module';
import { ProductModule } from './modules/product/product.module';
import { ProfileModule } from './modules/profile/profile.module';

@Module({
  imports: [
    NestConfigModule.forRoot({
      isGlobal: true,
      envFilePath: ['.env', '.env.local'],
      cache: true,
      load: [configuration],
      validationSchema: validateEnv,
    }),
    DatabaseModule,
    AppConfigModule,
    BannerModule,
    AuthModule,
    CustomerModule,
    DataConfigModule,
    ProductCategoryModule,
    ProductModule,
    ProfileModule,
  ],
  controllers: [AppController],
  providers: [
    AppService,
    CheckVersionMiddleware,
    CheckAccessMiddleware,
    SetPermissionMiddleware,
  ],
})
export class AppModule implements NestModule {
  configure(consumer: MiddlewareConsumer): void {
    consumer.apply(CheckAccessMiddleware).forRoutes('*');
  }
}
