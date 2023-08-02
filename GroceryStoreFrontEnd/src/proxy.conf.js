const PROXY_CONFIG = [
  {
    "/weatherforecast": {
      target: "https://localhost:5001",
      secure: false,
    },
    "/api/*": {
      target: "https://localhost:5001",
      secure: false,
      loglevel: "debug",
    }
  }
]

module.exports = PROXY_CONFIG;
