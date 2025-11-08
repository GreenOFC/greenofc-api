import { Controller, Get } from '@nestjs/common';
import { AppService } from './app.service';
import type { ResponseContext } from './common/dto/response.dto';

@Controller('health')
export class AppController {
  constructor(private readonly appService: AppService) {}

  @Get()
  getHealth(): ResponseContext {
    return this.appService.getHealth();
  }
}
