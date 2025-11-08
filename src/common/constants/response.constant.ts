export enum ResponseCode {
  SUCCESS = 1,
  ERROR = 2,
  UNAUTHORIZED = 3,
  IS_LOGGED_IN_OTHER_DEVICE = 4,
}

export const Message = {
  SUCCESS: '',
  ERROR: 'Lỗi hệ thống, vui lòng liên hệ IT!',
};

export const ResponseStatus = {
  SUCCESS: 'SUCCESS',
  ERROR: 'ERROR',
} as const;

