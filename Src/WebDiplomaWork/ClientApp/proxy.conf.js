const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:7791';

const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/user/",
      "/Images/",
      "/api/v1/auth/login",
      "/api/v1/user/",
      "/api/v1/user",
      "/api/v1/account",
      "/api/v1/toast",
      "/api/toast/by/account",
      "/api/toast/reply",
      "/api/toast/quote",
      "/api/toast/reToast",
      "/api/toast/replies/by/toast",
   ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
