import {
  Controller,
  Get,
  Param,
  Query,
  Req,
  HttpException,
  HttpStatus,
} from '@nestjs/common';
import { ApiTags } from '@nestjs/swagger';
import type { Request } from 'express';
import { Message, ResponseCode } from '../../common/constants/response.constant';
import { createSuccessResponse, createErrorResponse } from '../../common/dto/response.dto';
import { CustomerService } from './services/customer.service';
import { GetCustomerListQueryDto } from './dto/get-customer-list-query.dto';
import { GetStatusCountQueryDto } from './dto/get-status-count-query.dto';

@ApiTags('customer')
@Controller('customer')
export class CustomerController {
  constructor(private readonly customerService: CustomerService) {}

  @Get('getAll')
  async getCustomerList(
    @Req() request_context: Request,
    @Query() query_params: GetCustomerListQueryDto,
  ) {
    const user_entity = (request_context as any).user_entity ?? null;

    try {
      const { customers, total_page, total_record } =
        await this.customerService.getCustomerList(user_entity, query_params);

      return {
        code: ResponseCode.SUCCESS,
        message: Message.SUCCESS,
        data: customers,
        page_number: query_params.page_number ?? 1,
        total_page,
        total_record,
      };
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể lấy danh sách khách hàng'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Get()
  async getDefaultCustomers() {
    try {
      const customers = await this.customerService.getDefaultCustomerList();
      return createSuccessResponse(customers);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể lấy danh sách khách hàng'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Get('countstatus')
  async countStatus(
    @Req() request_context: Request,
    @Query() query_params: GetStatusCountQueryDto,
  ) {
    const user_entity = (request_context as any).user_entity ?? null;
    try {
      const status_count = await this.customerService.getStatusCount(
        user_entity,
        query_params.green_type ?? '',
        query_params.product_line ?? '',
      );

      return createSuccessResponse(status_count);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể thống kê trạng thái khách hàng'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Get(':id')
  async getCustomer(@Param('id') customer_id: string) {
    try {
      const customer = await this.customerService.getCustomerById(customer_id);
      if (!customer) {
        return createErrorResponse('Không tìm thấy khách hàng');
      }

      return createSuccessResponse(customer);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể lấy thông tin khách hàng'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }
}

