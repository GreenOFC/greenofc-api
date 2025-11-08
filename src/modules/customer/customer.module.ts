import { Module } from '@nestjs/common';
import { CustomerService } from './services/customer.service';
import { CustomerController } from './customer.controller';
import { CustomerDomainController } from './customer-domain.controller';
import { CustomerDomainService } from './services/customer-domain.service';

@Module({
  controllers: [CustomerController, CustomerDomainController],
  providers: [CustomerService, CustomerDomainService],
})
export class CustomerModule {}

