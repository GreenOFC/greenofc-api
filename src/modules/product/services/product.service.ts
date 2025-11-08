import { Injectable, Logger } from '@nestjs/common';

@Injectable()
export class ProductService {
  private readonly logger = new Logger(ProductService.name);

  async getAllProducts(green_type: string, product_line: string): Promise<unknown[]> {
    this.logger.debug('getAllProducts stub', { green_type, product_line });
    return [];
  }

  async getProductById(product_id: string): Promise<unknown> {
    this.logger.debug('getProductById stub', { product_id });
    return null;
  }

  async createProduct(product_payload: Record<string, unknown>): Promise<Record<string, unknown>> {
    this.logger.debug('createProduct stub', { product_payload });
    return { ...product_payload, id: product_payload?.['id'] ?? 'generated-product-id' };
  }

  async getProductByCategory(product_category_id: string, product_line: string): Promise<unknown[]> {
    this.logger.debug('getProductByCategory stub', { product_category_id, product_line });
    return [];
  }
}


