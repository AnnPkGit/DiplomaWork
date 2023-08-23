const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:5031';

const PROXY_CONFIG = [
  {
    context: [
      "/api/v1/weatherforecast",
      "/api/v1/user",
      "/Images/",
      "/api/v1/auth",
      "/api/v1/account",
      "/api/v1/example"
   ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
