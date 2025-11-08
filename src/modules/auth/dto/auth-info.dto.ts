import { ApiProperty } from '@nestjs/swagger';
import { UserStatus } from '../../../common/enums/user-status.enum';

export class AuthInfoDto {
  @ApiProperty({
    example: 'greendev01',
    description: 'Unique username assigned to the user.',
  })
  user_name: string;

  @ApiProperty({ example: 'Green Dev', description: 'Full name of the user.' })
  user_full_name: string;

  @ApiProperty({
    example: 'ROLE_SALE',
    description: 'Primary role identifier of the user.',
  })
  role_id: string;

  @ApiProperty({ description: 'Access token used for authenticated requests.' })
  token: string;

  @ApiProperty({ description: 'Refresh token for renewing access tokens.' })
  refresh_token: string;

  @ApiProperty({
    enum: UserStatus,
    description: 'Current status of the user account.',
  })
  status: UserStatus;
}
