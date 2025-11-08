import { ApiPropertyOptional } from '@nestjs/swagger';
import { IsOptional, IsString } from 'class-validator';

export class GetProductCategoryQueryDto {
  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Product line filter.' })
  product_line?: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Green type filter.' })
  green_type?: string;
}


