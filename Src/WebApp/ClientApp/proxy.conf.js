const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:7791';

const PROXY_CONFIG = [
  {
    context: [
      "/user/",
      "/Images/",
      "/api/v1/",
      "/sync/notification"
    ],
    target: target,
    secure: false,
    ws: true,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
