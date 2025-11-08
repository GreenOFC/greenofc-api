import { ApiPropertyOptional } from '@nestjs/swagger';
import { Type } from 'class-transformer';
import { IsInt, IsOptional, IsString, Min } from 'class-validator';

export class GetCustomerListQueryDto {
  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Product line filter.' })
  product_line?: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Green type filter.' })
  green_type?: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Status filter.' })
  status?: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Customer name filter.' })
  customer_name?: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Start date filter (string representation).' })
  from_date?: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'End date filter (string representation).' })
  to_date?: string;

  @IsOptional()
  @Type(() => Number)
  @IsInt()
  @Min(1)
  @ApiPropertyOptional({ description: 'Page number for pagination.', example: 1 })
  page_number?: number;

  @IsOptional()
  @Type(() => Number)
  @IsInt()
  @Min(1)
  @ApiPropertyOptional({ description: 'Page size for pagination.', example: 20 })
  page_size?: number;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Team lead filter.' })
  team_lead?: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'POS manager filter.' })
  pos_manager?: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Sale filter.' })
  sale?: string;
}

