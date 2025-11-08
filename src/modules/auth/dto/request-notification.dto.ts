import { ApiProperty, ApiPropertyOptional } from '@nestjs/swagger';
import { IsBoolean, IsOptional, IsString } from 'class-validator';

export class GetNotificationRequestDto {
  @IsString()
  @ApiProperty({ description: 'Target user identifier to fetch notifications for.' })
  user_id: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Filter notifications by green type identifier.', example: 'EC' })
  green_type?: string;

  @IsOptional()
  @IsBoolean()
  @ApiPropertyOptional({ description: 'If true, only unread notifications are counted.', example: true })
  is_unread?: boolean;
}
