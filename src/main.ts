import { ValidationPipe } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { NestFactory } from '@nestjs/core';
import { DocumentBuilder, SwaggerModule } from '@nestjs/swagger';
import { AppModule } from './app.module';

async function bootstrap() {
  const app = await NestFactory.create(AppModule);
  app.setGlobalPrefix('api');
  app.useGlobalPipes(
    new ValidationPipe({
      whitelist: true,
      transform: true,
      forbidNonWhitelisted: true,
    }),
  );

  const configService = app.get(ConfigService);
  const cors_origins_env = configService.get<string>('app.corsOrigins') ?? '';
  const cors_origins = cors_origins_env
    .split(',')
    .map((origin) => origin.trim())
    .filter((origin) => origin.length > 0);

  app.enableCors({
    origin: cors_origins.length > 0 ? cors_origins : true,
    credentials: true,
  });

  const swaggerConfig = new DocumentBuilder()
    .setTitle('GreenOFC API')
    .setDescription('API documentation for the GreenOFC NestJS backend')
    .setVersion('1.0')
    .addBearerAuth({ type: 'http', scheme: 'bearer', bearerFormat: 'JWT' })
    .build();
  const document = SwaggerModule.createDocument(app, swaggerConfig);
  SwaggerModule.setup('docs', app, document, {
    jsonDocumentUrl: 'docs/json',
  });

  const port =
    configService.get<number>('app.port') ??
    configService.get<number>('PORT') ??
    3000;
  await app.listen(port);
}
bootstrap();
