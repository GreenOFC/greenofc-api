import { Injectable } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { PayMeCert } from '../../../common/utils/payme-cert.util';
import { BannerService } from '../../banner/services/banner.service';
import { PaymeSdkConfigResponse } from '../dto/payme-sdk-config.response';

@Injectable()
export class AppConfigService {
  constructor(
    private readonly bannerService: BannerService,
    private readonly configService: ConfigService,
  ) {}

  async getBannerUrls(): Promise<string[]> {
    return this.bannerService.getAllUrls();
  }

  getMcHost(): string {
    return this.configService.get<string>('mc.host') ?? '';
  }

  getPaymeSdkConfig(): PaymeSdkConfigResponse {
    const sdkEnv = (this.configService.get<string>('payme.sdkEnv') ?? 'DEVELOPMENT').toUpperCase();

    return {
      appToken: this.configService.get<string>('payme.appToken') ?? '',
      secretKey: this.configService.get<string>('payme.secretKey') ?? '',
      appId: this.configService.get<string>('payme.appId') ?? '',
      sdkEnv,
      publicKey: PayMeCert.payMeSDKPubKey(sdkEnv),
      privateKey: PayMeCert.payMeSDKPriKey(sdkEnv),
    };
  }
}

