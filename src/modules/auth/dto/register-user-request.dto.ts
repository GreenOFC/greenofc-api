import { ApiProperty, ApiPropertyOptional } from '@nestjs/swagger';
import { IsOptional, IsString } from 'class-validator';

export class RegisterUserRequestDto {
  @IsString()
  @ApiProperty({ description: 'Full name provided by the user.', example: 'Green Dev' })
  full_name: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'National ID card number.', example: '123456789' })
  id_card?: string;

  @IsString()
  @ApiProperty({ description: 'Mobile phone number.', example: '0900000000' })
  phone: string;

  @IsString()
  @ApiProperty({ description: 'Email address.', example: 'user@example.com' })
  user_email: string;

  @IsString()
  @ApiProperty({ description: 'Password chosen by the user.', example: 'Abc12345' })
  user_password: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Team lead user identifier associated with this user.' })
  team_lead_user_id?: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'POS identifier assigned to the user.' })
  pos_id?: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Referral code provided by another user.' })
  referral_code?: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Role name to assign to the user.', example: 'SALE' })
  role_name?: string;
}
