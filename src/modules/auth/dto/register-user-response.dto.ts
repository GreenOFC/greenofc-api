import { ApiProperty } from '@nestjs/swagger';

export class RegisterUserResponseDto {
  @ApiProperty({ description: 'Identifier of the newly registered user.' })
  id: string;
}

