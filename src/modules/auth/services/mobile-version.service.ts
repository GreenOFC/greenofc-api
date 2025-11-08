import { Injectable, Logger } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { InjectModel } from '@nestjs/mongoose';
import { Model } from 'mongoose';
import { OsType } from '../../../common/enums/os-type.enum';
import { MobileVersion, MobileVersionDocument } from '../schemas/mobile-version.schema';

@Injectable()
export class MobileVersionService {
  private readonly logger = new Logger(MobileVersionService.name);

  constructor(
    private readonly configService: ConfigService,
    @InjectModel(MobileVersion.name)
    private readonly mobileVersionModel: Model<MobileVersionDocument>,
  ) {}

  getConfiguredVersion(osType: string): string {
    const androidVersion = this.configService.get<string>('mobile.androidVersion');
    const iosVersion = this.configService.get<string>('mobile.iosVersion');

    if (osType?.toUpperCase() === OsType.Android.toUpperCase()) {
      return androidVersion ?? '1.0';
    }

    if (osType?.toUpperCase() === OsType.IOS.toUpperCase()) {
      return iosVersion ?? '1.0';
    }

    return '1.0';
  }

  async isLatestVersion(osType: string, version?: string): Promise<boolean> {
    if ((osType ?? '').toUpperCase() === OsType.Web.toUpperCase()) {
      return true;
    }

    const versionToCheck = version ?? (await this.getCurrentVersionFromDb(osType)) ?? '0.0';
    const configuredVersion = this.getConfiguredVersion(osType);

    if (!versionToCheck) {
      return false;
    }

    const requestMatch = versionToCheck.match(/^(\d+)\.(\d+)/);
    const configMatch = configuredVersion.match(/^(\d+)\.(\d+)/);

    if (!requestMatch || !configMatch) {
      return false;
    }

    const requestMajor = Number(requestMatch[1]);
    const requestMinor = Number(requestMatch[2]);
    const configMajor = Number(configMatch[1]);
    const configMinor = Number(configMatch[2]);

    if (requestMajor > configMajor) {
      return true;
    }

    if (requestMajor === configMajor && requestMinor >= configMinor) {
      return true;
    }

    return false;
  }

  private async getCurrentVersionFromDb(osType: string): Promise<string | null> {
    try {
      const record = await this.mobileVersionModel
        .findOne({ type: { $regex: new RegExp(`^${osType}$`, 'i') } })
        .sort({ createdAt: -1 })
        .lean();
      return record?.version ?? null;
    } catch (error) {
      this.logger.error('Failed to query mobile version', error instanceof Error ? error.stack : error);
      return null;
    }
  }
}

