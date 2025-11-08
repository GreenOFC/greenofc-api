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
import { createErrorResponse, createSuccessResponse } from '../../common/dto/response.dto';
import { ProductService } from './services/product.service';
import { GetProductListQueryDto } from './dto/get-product-list-query.dto';
import { GetProductByCategoryQueryDto } from './dto/get-product-by-category-query.dto';

@ApiTags('product')
@Controller('product')
export class ProductController {
  constructor(private readonly productService: ProductService) {}

  @Get('getAll')
  async getAll(@Query() query_params: GetProductListQueryDto) {
    try {
      const products = await this.productService.getAllProducts(
        query_params.green_type ?? '',
        query_params.product_line ?? '',
      );

      return createSuccessResponse(products);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể lấy danh sách sản phẩm'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Get(':product_id')
  async getById(@Param('product_id') product_id: string) {
    try {
      const product = await this.productService.getProductById(product_id);
      return createSuccessResponse(product);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể lấy thông tin sản phẩm'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Post('create')
  async create(@Body() product_payload: Record<string, unknown>) {
    try {
      const product = await this.productService.createProduct(product_payload);
      return createSuccessResponse(product);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể tạo sản phẩm'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Get('getProductByCategoryId')
  async getProductByCategory(@Query() query_params: GetProductByCategoryQueryDto) {
    try {
      const products = await this.productService.getProductByCategory(
        query_params.product_category_id ?? '',
        query_params.product_line ?? '',
      );

      return createSuccessResponse(products);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể lấy sản phẩm theo nhóm'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }
}


