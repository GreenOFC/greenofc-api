import { ApiProperty, ApiPropertyOptional } from '@nestjs/swagger';
import { UserStatus } from '../../../common/enums/user-status.enum';

export class ResponseLoginInfoDto {
  @ApiProperty({ description: 'Identifier of the authenticated user.' })
  user_id: string;

  @ApiProperty({ description: 'Username of the authenticated user.' })
  user_name: string;

  @ApiProperty({ description: 'Full name of the user.' })
  full_name: string;

  @ApiProperty({ description: 'Role name assigned to the user.', example: 'TL' })
  role_name: string;

  @ApiProperty({ description: 'JWT access token for authenticated requests.' })
  token: string;

  @ApiProperty({ description: 'Phone number of the user.', example: '0900000000' })
  phone: string;

  @ApiProperty({ description: 'Email of the user.', example: 'user@example.com' })
  user_email: string;

  @ApiPropertyOptional({ description: 'Push notification registration token.' })
  registration_token?: string;

  @ApiPropertyOptional({ description: 'Number of unread notifications.', example: 5 })
  unread_noti?: number;

  @ApiPropertyOptional({ description: 'MAFC code linked to the user.' })
  mafc_code?: string;

  @ApiPropertyOptional({ description: 'EC DSA code linked to the user.' })
  ec_dsa_code?: string;

  @ApiProperty({ enum: UserStatus, description: 'Current status of the user account.' })
  status: UserStatus;

  @ApiPropertyOptional({ description: 'Set of permission identifiers granted to the user.' })
  permissions?: string[];
}

