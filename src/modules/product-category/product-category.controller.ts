import {
  Body,
  Controller,
  Get,
  HttpException,
  HttpStatus,
  Param,
  Post,
  Query,
} from '@nestjs/common';
import { ApiTags } from '@nestjs/swagger';
import { createSuccessResponse, createErrorResponse } from '../../common/dto/response.dto';
import { ProductCategoryService } from './services/product-category.service';
import { GetProductCategoryQueryDto } from './dto/get-product-category-query.dto';

@ApiTags('product-category')
@Controller('productcategory')
export class ProductCategoryController {
  constructor(private readonly productCategoryService: ProductCategoryService) {}

  @Get('getAll')
  async getAll(@Query() query_params: GetProductCategoryQueryDto) {
    try {
      const categories = await this.productCategoryService.getProductCategories(
        query_params.product_line ?? '',
        query_params.green_type ?? '',
      );

      return createSuccessResponse(categories);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể lấy danh sách nhóm sản phẩm'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Get(':product_category_id')
  async getById(@Param('product_category_id') product_category_id: string) {
    try {
      const category = await this.productCategoryService.getProductCategoryById(product_category_id);
      return createSuccessResponse(category);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể lấy thông tin nhóm sản phẩm'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Post('create')
  async create(@Body() product_category_payload: Record<string, unknown>) {
    try {
      const category = await this.productCategoryService.createProductCategory(product_category_payload);
      return createSuccessResponse(category);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể tạo nhóm sản phẩm'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }
}


