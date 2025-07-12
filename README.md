# EzUrl 🔗

Modern URL shortener built with ASP.NET Core Minimal APIs.

## Features

- **🚀 Minimal APIs** - Lightweight ASP.NET Core architecture
- **🔗 URL Shortening** - 5-character short codes
- **⏰ Auto Expiration** - URLs expire after 24 hours
- **🗑️ Auto Cleanup** - Background service removes expired URLs
- **📚 API Docs** - Interactive documentation with Scalar

## Tech Stack

- ASP.NET Core (.NET 9)
- Entity Framework Core
- SQL Server
- Carter Framework
- OpenAPI/Scalar

## Quick Start

1. **Clone and setup (Update connection string in appsettings.json)**
   ```bash
   git clone https://github.com/ilkeozs/EzUrl.git
   cd EzUrl
   dotnet ef database update
   dotnet run
   ```

2. **Visit**
   - App: Check your terminal for the running port
   - Docs: `/scalar/v1` endpoint

## API Endpoints

- `GET /api/urls` - List all URLs
- `GET /api/urls/{id}` - Get URL by ID
- `POST /api/urls` - Create short URL
- `GET /{shortCode}` - Redirect to original URL

## License

MIT
