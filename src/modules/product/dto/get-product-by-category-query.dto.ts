import { ApiPropertyOptional } from '@nestjs/swagger';
import { IsOptional, IsString } from 'class-validator';

export class GetProductByCategoryQueryDto {
  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Product category identifier.' })
  product_category_id?: string;

  @IsOptional()
  @IsString()
  @ApiPropertyOptional({ description: 'Product line filter.' })
  product_line?: string;
}


