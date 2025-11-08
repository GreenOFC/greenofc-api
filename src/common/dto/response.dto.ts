import { Message, ResponseCode } from '../constants/response.constant';

export interface ResponseContext<T = unknown> {
  code: number;
  message: string;
  data?: T;
}

export interface ResponseMessage {
  status: string;
  message: string;
}

export const createSuccessResponse = <T = unknown>(data?: T): ResponseContext<T> => ({
  code: ResponseCode.SUCCESS,
  message: Message.SUCCESS,
  data,
});

export const createErrorResponse = <T = unknown>(message: string, data?: T): ResponseContext<T> => ({
  code: ResponseCode.ERROR,
  message,
  data,
});

