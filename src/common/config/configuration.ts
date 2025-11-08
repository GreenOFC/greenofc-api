export default () => ({
  app: {
    name: 'greenofc-nest-backend',
    port: parseInt(process.env.PORT ?? '3000', 10),
    environment: process.env.NODE_ENV ?? 'development',
    corsOrigins: process.env.APP_CORS_ORIGINS ?? '',
  },
  mongo: {
    uri: process.env.MONGODB_URI ?? 'mongodb://localhost:27017',
    dbName: process.env.MONGODB_DB_NAME ?? 'greenofc',
  },
  jwt: {
    issuer: process.env.JWT_ISSUER ?? 'Test.com',
    key: process.env.JWT_KEY ?? '',
  },
  mc: {
    host: process.env.MC_HOST ?? '',
  },
  payme: {
    appToken: process.env.PAYME_APP_TOKEN ?? '',
    secretKey: process.env.PAYME_SECRET_KEY ?? '',
    appId: process.env.PAYME_APP_ID ?? '',
    sdkEnv: (process.env.PAYME_SDK_ENV ?? 'DEVELOPMENT').toUpperCase(),
  },
  mobile: {
    androidVersion: process.env.MOBILE_ANDROID_VERSION ?? '1.0',
    iosVersion: process.env.MOBILE_IOS_VERSION ?? '1.0',
  },
});
