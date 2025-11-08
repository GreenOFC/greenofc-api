import { Injectable, Logger } from '@nestjs/common';
import { GetCustomerListQueryDto } from '../dto/get-customer-list-query.dto';

export interface CustomerListResult {
  customers: unknown[];
  total_page: number;
  total_record: number;
}

@Injectable()
export class CustomerService {
  private readonly logger = new Logger(CustomerService.name);

  async getCustomerList(
    user: any,
    query: GetCustomerListQueryDto,
  ): Promise<CustomerListResult> {
    this.logger.debug('getCustomerList stub invoked', { user, query });
    return { customers: [], total_page: 0, total_record: 0 };
  }

  async getDefaultCustomerList(): Promise<unknown[]> {
    this.logger.debug('getDefaultCustomerList stub invoked');
    return [];
  }

  async getStatusCount(
    user: any,
    green_type: string,
    product_line: string,
  ): Promise<Record<string, unknown>> {
    this.logger.debug('getStatusCount stub invoked', {
      user,
      green_type,
      product_line,
    });
    return {};
  }

  async getCustomerById(customer_id: string): Promise<unknown | null> {
    this.logger.debug('getCustomerById stub invoked', { customer_id });
    return null;
  }
}

