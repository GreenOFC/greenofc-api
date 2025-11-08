import { Injectable, Logger, NestMiddleware } from '@nestjs/common';
import { Request, Response, NextFunction } from 'express';
import { AuthService } from '../../modules/auth/services/auth.service';

@Injectable()
export class SetPermissionMiddleware implements NestMiddleware {
  private readonly logger = new Logger(SetPermissionMiddleware.name);

  constructor(private readonly authService: AuthService) {}

  async use(
    request: Request,
    response: Response,
    next_function: NextFunction,
  ): Promise<void> {
    try {
      const user_id = this.extractUserId(request);
      if (!user_id) {
        return next_function();
      }

      const user = await this.authService.findUserById(user_id);
      if (user) {
        (request as any).user_entity = user;
        (request as any).user_permissions = Array.isArray(user.role_ids)
          ? [...new Set(user.role_ids)]
          : [];
      }
    } catch (error) {
      this.logger.error(
        'Set permission middleware failed',
        error instanceof Error ? error.stack : error,
      );
    }

    return next_function();
  }

  private extractUserId(request: Request): string | null {
    const user_payload = (request as any).user ?? {};
    if (typeof user_payload?.userId === 'string' && user_payload.userId) {
      return user_payload.userId;
    }

    if (typeof user_payload?.user_id === 'string' && user_payload.user_id) {
      return user_payload.user_id;
    }

    const claim_user_id = this.getClaimFromHeaders(request, 'userId');
    if (claim_user_id) {
      return claim_user_id;
    }

    return null;
  }

  private getClaimFromHeaders(
    request: Request,
    claim_key: string,
  ): string | null {
    const raw_claims = request.headers[`x-${claim_key.toLowerCase()}`];
    if (typeof raw_claims === 'string' && raw_claims) {
      return raw_claims;
    }
    if (Array.isArray(raw_claims) && raw_claims.length > 0) {
      return raw_claims[0];
    }
    return null;
  }
}
