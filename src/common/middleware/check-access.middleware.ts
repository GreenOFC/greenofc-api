import { Injectable, Logger, NestMiddleware } from '@nestjs/common';
import { Request, Response, NextFunction } from 'express';
import { ResponseStatus, ResponseCode } from '../constants/response.constant';
import { UserLoginService } from '../../modules/auth/services/user-login.service';
import { AuthService } from 'src/modules/auth/services/auth.service';
import { IUser } from '../decorators';

export interface UserRequest extends Request {
  user: IUser;
}

@Injectable()
export class CheckAccessMiddleware implements NestMiddleware {
  private readonly logger = new Logger(CheckAccessMiddleware.name);

  constructor(
    private readonly userLoginService: UserLoginService,
    private readonly authService: AuthService,
  ) {}

  async use(
    request: UserRequest,
    response: Response,
    next_function: NextFunction,
  ): Promise<void> {
    try {
      const auth_header = request.headers.authorization;
      if (!auth_header || !auth_header.startsWith('Bearer ')) {
        return next_function();
      }

      const [, token] = auth_header.split(' ');
      if (!token) {
        return next_function();
      }

      const user_login = await this.userLoginService.getUserLoginByToken(token);
      if (!user_login) {
        response.status(401).json({
          status: ResponseStatus.ERROR,
          code: ResponseCode.IS_LOGGED_IN_OTHER_DEVICE,
          message: 'Bạn đã đăng nhập ở một nơi khác, vui lòng đăng nhập lại!',
        });
        return;
      }

      const currentUser = await this.authService.findUserById(
        user_login.user_id,
      );
      if (!currentUser) {
        response.status(401).json({
          status: ResponseStatus.ERROR,
          code: ResponseCode.UNAUTHORIZED,
          message:
            'Vui lòng đăng nhập lại để thực hiện các biện pháp bảo mật mới và cải tiến.',
        });
        return;
      }

      request.user = currentUser as unknown as IUser;
      return next_function();
    } catch (error) {
      this.logger.error(
        'Check access failed',
        error instanceof Error ? error.stack : error,
      );
      return next_function();
    }
  }
}
