import {
  Body,
  Controller,
  HttpException,
  HttpStatus,
  Post,
  Query,
  Req,
} from '@nestjs/common';
import { ApiTags } from '@nestjs/swagger';
import type { Request } from 'express';
import { createErrorResponse, createSuccessResponse } from '../../common/dto/response.dto';
import { CustomerDomainService } from './services/customer-domain.service';

@ApiTags('customer-domain')
@Controller('customer/domain')
export class CustomerDomainController {
  constructor(private readonly customerDomainService: CustomerDomainService) {}

  @Post('create')
  async createCustomer(@Req() request_context: Request, @Body() customer_payload: Record<string, unknown>) {
    try {
      const user_payload = (request_context as any).user ?? {};
      const user_id = user_payload?.userId ?? user_payload?.user_id ?? undefined;
      const customer = await this.customerDomainService.createCustomer(customer_payload, user_id);
      return createSuccessResponse(customer);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể tạo khách hàng'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Post('update')
  async updateCustomer(
    @Req() request_context: Request,
    @Body() customer_payload: Record<string, unknown>,
    @Query('part') update_part?: string,
  ) {
    try {
      const user_entity = (request_context as any).user_entity ?? null;
      const updated = await this.customerDomainService.updateCustomerInfo(
        customer_payload,
        update_part,
        user_entity,
      );
      return createSuccessResponse(updated);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể cập nhật thông tin khách hàng'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Post('delete')
  async deleteCustomers(@Body() customer_ids: string[]) {
    try {
      const success = await this.customerDomainService.deleteCustomers(customer_ids);
      return createSuccessResponse(success);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể xóa khách hàng'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Post('updateStatus')
  async updateStatus(@Body() status_payload: Record<string, unknown>) {
    try {
      const success = await this.customerDomainService.updateStatus(status_payload);
      return createSuccessResponse(success);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể cập nhật trạng thái khách hàng'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Post('return')
  async returnStatus(@Body() status_payload: Record<string, unknown>) {
    try {
      const success = await this.customerDomainService.returnStatus(status_payload);
      return createSuccessResponse(success);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể trả trạng thái khách hàng'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }

  @Post('submit')
  async submitStatus(@Body() status_payload: Record<string, unknown>) {
    try {
      const success = await this.customerDomainService.submitStatus(status_payload);
      return createSuccessResponse(success);
    } catch (error) {
      if (error instanceof HttpException) {
        throw error;
      }

      throw new HttpException(
        createErrorResponse('Không thể gửi trạng thái khách hàng'),
        HttpStatus.INTERNAL_SERVER_ERROR,
      );
    }
  }
}

