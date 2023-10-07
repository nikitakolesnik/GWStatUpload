const PROXY_CONFIG = [
  {
    context: [
      "/stats",
    ],
    target: "https://localhost:7000",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
