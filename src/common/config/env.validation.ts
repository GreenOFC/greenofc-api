import * as Joi from 'joi';

export const validateEnv = Joi.object({
  NODE_ENV: Joi.string()
    .valid('development', 'production', 'test', 'staging')
    .default('development'),
  PORT: Joi.number().default(3000),
  MONGODB_URI: Joi.string().uri().default('mongodb://localhost:27017'),
  MONGODB_DB_NAME: Joi.string().default('greenofc'),
  JWT_KEY: Joi.string().allow('', null),
  JWT_ISSUER: Joi.string().default('Test.com'),
  MC_HOST: Joi.string().uri().allow('', null),
  PAYME_APP_TOKEN: Joi.string().allow('', null),
  PAYME_SECRET_KEY: Joi.string().allow('', null),
  PAYME_APP_ID: Joi.string().allow('', null),
  PAYME_SDK_ENV: Joi.string().allow('', null),
  MOBILE_ANDROID_VERSION: Joi.string().allow('', null),
  MOBILE_IOS_VERSION: Joi.string().allow('', null),
  APP_CORS_ORIGINS: Joi.string().allow('', null),
}).unknown(true);

