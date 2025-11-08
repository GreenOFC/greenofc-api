const PAYME_ENV_PRODUCTION = 'PRODUCTION';

export const PayMeCert = {
  payMePubKey(env: string) {
    if ((env ?? '').toUpperCase() === PAYME_ENV_PRODUCTION) {
      return (
        '-----BEGIN PUBLIC KEY-----\n' +
        'MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAI6xDmedwY3XbdyZ0gzY4niywg47jn0L\n' +
        'uanOudQzmhaj3sWmvKOcUb+T6Ripof6qbHnfHMuqGoSzTZxYu2+RUO0CAwEAAQ==\n' +
        '-----END PUBLIC KEY-----\n'
      );
    }

    return (
      '-----BEGIN PUBLIC KEY-----\n' +
      'MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAJIY82o7iehGxaRYCT1BwvQazphU+X+O\n' +
      'HcCt/0aGj7ybtK4bFBP0BiQTc63NTF05irtUNdO5wkpMIJuBgcxvTwMCAwEAAQ==\n' +
      '-----END PUBLIC KEY-----\n'
    );
  },

  payMePriKey(env: string) {
    if ((env ?? '').toUpperCase() === PAYME_ENV_PRODUCTION) {
      return (
        '-----BEGIN RSA PRIVATE KEY-----\n' +
        'MIIBOAIBAAJAaJl12LpuH4CGhNqiprA3yaOHVuYu41GQLRdvsEb8UspHTAguK3Mm\n' +
        'E8mPNgVQ+zy05Fr8qpJRyfYSGfhF0QznZwIDAQABAkBDH+Ahkhojey5YSZpBkCps\n' +
        '6tVqbdM/K9NzLGwPWq6ITdbzuiNctCsQpVZLdgN7DNvZpPlF0NrGWB1SyG9xeseJ\n' +
        'AiEAqAZ1iyUq21zNRiamdpQPA8/yuXwng/MEcq4j3+46o9UCIQCfXZ80tINGrJYj\n' +
        '41qMzL5kVo/GBB420H4Idv0TbR9ISwIgH29d7FNeyj15dgdjG0ol6PutmIAe0HPV\n' +
        'wDLOKWXjeKUCIGn5uw40X/vWdv0kiimJWa6ltXQLdwAigz0jy0Vo0l2jAiB/CCp+\n' +
        '2aXHoMv5RL39LEIyV1dreVHSQDqCJG5XnNlsIQ==\n' +
        '-----END RSA PRIVATE KEY-----\n'
      );
    }

    return (
      '-----BEGIN RSA PRIVATE KEY-----\n' +
      'MIIBOAIBAAJATUqHuhwnaHR5NRECkBxpLjLVj3lXobOV1A7hJr6AzeKLB92kNlX9\n' +
      '0+8QrdgKg9IZzwKZ+uCoHvYjLa7k1KkrowIDAQABAkAO8AqPQ4WyQGB0ixcNtw/0\n' +
      '58oycmcnT0fztfR5wnOuVpXQ58oCl7xaq2zn8tyxQL/REEGULJTA4f+vtTTx7dSR\n' +
      'AiEAkqVQukcYdC2sEmPpXczhYc2AQeatPuSfqeFWCS+38+0CIQCG7Wl8Dn4dY1NK\n' +
      '3Opq0o+FE5OsMNuNf5FHLe5KDrbLzwIgO8sQSYPnoqdku/LlGowJcfl6zGQkS7qo\n' +
      'p3nrvL8qQFkCIC9NBYDPF9k3m9iPz8I5JMUzUr63thBJ22VHrdTaiayzAiAD5uEl\n' +
      'dYJMmJ942OnRu56dWcNzCAk4e1lLOHfEvZu/YQ==\n' +
      '-----END RSA PRIVATE KEY-----'
    );
  },

  payMeSDKPubKey(env: string) {
    if ((env ?? '').toUpperCase() === PAYME_ENV_PRODUCTION) {
      return (
        '-----BEGIN PUBLIC KEY-----\n' +
        'MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAIrdoK9RathHyYvK0TIcWXwrWGWLJW7I\n' +
        'XI7QtAN3RM0SWcpCRYKTo07KR2CBG87SRD42dQ6jlcVWU1MKqMDpP/0CAwEAAQ==\n' +
        '-----END PUBLIC KEY-----\n'
      );
    }

    return (
      '-----BEGIN PUBLIC KEY-----\n' +
      'MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAI5JgVNO8B2vXhb4oriic1VCHHJMx4XO\n' +
      'Ictl82o3+88iQH+r9Y9OLLGC/2aw2z98O6suDcR2NqkoSGCe3TzSEjMCAwEAAQ==\n' +
      '-----END PUBLIC KEY-----\n'
    );
  },

  payMeSDKPriKey(env: string) {
    if ((env ?? '').toUpperCase() === PAYME_ENV_PRODUCTION) {
      return (
        '-----BEGIN RSA PRIVATE KEY-----\n' +
        'MIIBOQIBAAJAfODKBGG3HIkeKWu0rtWM18olGmsPzdIZOn2z94CLD8/vRbbAqHwy\n' +
        'pIrihAI4m/IsAutqRwE/wZvMI/hbRFWnEQIDAQABAkBfBINyNg+oDMUAa019wkt6\n' +
        'XftBULkjuHstwDvRVON60YJFYm3QndFPAMe98DIMN1vxlU1T+KDeITH5z+SZZtAB\n' +
        'AiEAugmuYUr6xtlJebMO1rJR1vSZ42dCA8IWv9X+5UN3qIECIQCr1xwUephmcLyT\n' +
        'Ip5t2gMv5JhblwMbzjmcsQafLgQ2kQIgD+WD4QxnIVav0JZcTjwugg+klqncGjYb\n' +
        'e4jtnumE8AECICt9M0QTisJSQcdS+Zl/lVLnnY+Adm7xEC+RtcVPVg2hAiEAso65\n' +
        '26euI/cS3VVhE7fcwemswC4aOZar2FzEpO/3z8w=\n' +
        '-----END RSA PRIVATE KEY-----\n'
      );
    }

    return (
      '-----BEGIN RSA PRIVATE KEY-----\n' +
      'MIIBOAIBAAJAea1XTDujNuxt5hdRVn/LuG+itFbd5TDH0cIuPvpnjMPO2ruFu9eW\n' +
      'JhtIIjHlHiGV+lu21PLRBUrOM1GUboVQhQIDAQABAkBmY4MOVzDkytc/w2dijm9z\n' +
      'aB1V+7MFZaL/05Lu0+/G9tcWaV22UO0Zp4zh33G3R4oTo6084yzxchMw7hew2DuB\n' +
      'AiEA8EYE7tuaTwlYlKabnR+duws1N+bCWev3OWhXoxbM6nkCIQCBpCQtq35Ob0/T\n' +
      'iyCKxXx6MpT9v6yMUcwHwNnHmzKTbQIgEf18IxJIRICzpDWxUxtp6PZW3r+lb6wu\n' +
      'T5sTbL+pSikCIEJzF/gwxvT9KTWNQoje4QbhlmzaKl9iLeprdzVQ34OhAiA/Lz7W\n' +
      'mqCA+qWLb/QszFXMh2+//XshI1lE/U+0UBO3yg==\n' +
      '-----END RSA PRIVATE KEY-----'
    );
  },
};

