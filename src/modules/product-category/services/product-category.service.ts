import { Injectable, Logger } from '@nestjs/common';

@Injectable()
export class ProductCategoryService {
  private readonly logger = new Logger(ProductCategoryService.name);

  async getProductCategories(
    product_line: string,
    green_type: string,
  ): Promise<unknown[]> {
    this.logger.debug('getProductCategories stub', { product_line, green_type });
    return [];
  }

  async getProductCategoryById(product_category_id: string): Promise<unknown> {
    this.logger.debug('getProductCategoryById stub', { product_category_id });
    return null;
  }

  async createProductCategory(
    product_category_payload: Record<string, unknown>,
  ): Promise<Record<string, unknown>> {
    this.logger.debug('createProductCategory stub', { product_category_payload });
    return {
      ...product_category_payload,
      id: product_category_payload?.['id'] ?? 'generated-product-category-id',
    };
  }
}


