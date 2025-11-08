import { IsNotEmpty, IsString, MinLength } from 'class-validator';

export class ChangePasswordProfileRequest {
  @IsString()
  @IsNotEmpty()
  old_password: string;

  @IsString()
  @IsNotEmpty()
  @MinLength(6)
  new_password: string;
}

