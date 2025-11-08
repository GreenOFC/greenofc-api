import { ApiProperty, ApiPropertyOptional } from '@nestjs/swagger';
import { IsDefined, IsOptional, IsString } from 'class-validator';

export class RequestLoginInfoDto {
  @IsDefined()
  @ApiProperty({
    description: 'Username, phone, or email provided by the user.',
    example: 'greendev01',
  })
  user_name: string;

  @IsDefined()
  @ApiProperty({
    description: 'Raw password provided by the user.',
    example: 'Abc12345',
  })
  password: string;

  @IsString()
  @IsOptional()
  @ApiPropertyOptional({
    description: 'Device uuid used to track login sessions.',
    example: 'b5fbe45a-4a97-486f-8c22-6bbda235e8b7',
  })
  uuid?: string;

  @IsString()
  @IsOptional()
  @ApiPropertyOptional({
    description: 'Operating system of the client device.',
    example: 'ANDROID',
  })
  os_type?: string;

  @IsString()
  @IsOptional()
  @ApiPropertyOptional({
    description: 'Mobile app version used for compatibility checks.',
    example: '1.6.0',
  })
  mobile_version?: string;

  @IsString()
  @IsOptional()
  @ApiPropertyOptional({
    description: 'Firebase/FCM registration token for push notifications.',
  })
  registration_token?: string;
}
