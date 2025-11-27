# Firmeza - Sales Management System

Complete sales management system built with clean architecture, including a REST API, an administrative web application, and an online store.

## 🏗️ Project Architecture

The project is organized into three main components:

- **`Firmeza.Api`** - REST API with .NET 8.0 and JWT authentication
- **`Firmeza.Web`** - Administrative web application with ASP.NET Core MVC
- **`firmeza-store`** - Online store with Vue.js 3 + Vite

### Layer Structure (Clean Architecture)

```
Firmeza/
├── Domain/              # Entities and business logic
├── Application/         # Use cases and DTOs
├── Infrastructure/      # Persistence and external services
├── Firmeza.Api/        # REST API
├── Firmeza.Web/        # Administrative web application
├── firmeza-store/      # Online store (Vue.js)
└── Firmeza.Test/       # Unit tests
```

## 🚀 Quick Start with Docker

### Prerequisites

- [Docker](https://www.docker.com/get-started) installed
- [Docker Compose](https://docs.docker.com/compose/install/) installed

### Run the Complete Project

1. **Clone the repository**
   ```bash
   git clone https://github.com/Johandry-M6B/Firmeza.git
   cd Firmeza
   ```

2. **Build the images**
   ```bash
   docker compose build
   ```

3. **Start the services**
   ```bash
   docker compose up -d
   ```

4. **Access the applications**
   - **API**: http://localhost:5000 (Swagger UI at root)
   - **Web Admin**: http://localhost:5001
   - **Store**: http://localhost:3000

5. **View logs**
   ```bash
   docker compose logs -f
   ```

6. **Stop the services**
   ```bash
   docker compose down
   ```

## 🛠️ Local Development (Without Docker)

### API (.NET 8.0)

```bash
cd Firmeza.Api
dotnet restore
dotnet run
```

The API will be available at `http://localhost:5000`

### Web Admin (.NET 8.0)

```bash
cd Firmeza.Web
dotnet restore
dotnet run
```

The web application will be available at `http://localhost:5001`

### Store (Vue.js + Vite)

```bash
cd firmeza-store
npm install
npm run dev
```

The store will be available at `http://localhost:5173`

## 📦 Services and Ports

| Service | Port | Description |
|---------|------|-------------|
| API | 5000 | REST API with Swagger UI |
| Web | 5001 | Administrative panel |
| Store | 3000 | Online store |

## 🔑 Key Features

### API
- ✅ JWT Authentication
- ✅ Integrated Swagger UI
- ✅ Configured CORS
- ✅ Global exception handling
- ✅ Health check endpoint (`/health`)
- ✅ Clean Architecture with MediatR

### Web Admin
- ✅ Product management
- ✅ Sales management
- ✅ Reports with Excel/PDF
- ✅ Authentication with Identity
- ✅ PostgreSQL database (Supabase)

### Store
- ✅ Product catalog
- ✅ Shopping cart
- ✅ Vue Router for navigation
- ✅ Pinia for state management
- ✅ Tailwind CSS for styling

## 🗄️ Database

The project uses **PostgreSQL** hosted on **Supabase**. The connection string is configured in:
- `Firmeza.Api/appsettings.json`
- `Firmeza.Web/appsettings.json`

To use a local database, modify the environment variables in `docker-compose.yml`.

## 🧪 Testing

### Run unit tests

```bash
cd Firmeza.Test
dotnet test
```

### Run with coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 📝 Environment Variables

### API
- `ASPNETCORE_ENVIRONMENT` - Runtime environment (Development/Production)
- `ConnectionStrings__DefaultConnection` - Database connection string
- `JwtSettings__SecretKey` - Secret key for JWT
- `JwtSettings__Issuer` - Token issuer
- `JwtSettings__Audience` - Token audience

### Web
- `ASPNETCORE_ENVIRONMENT` - Runtime environment
- `ConnectionStrings__DefaultConnection` - Database connection string

## 🐳 Useful Docker Commands

```bash
# Rebuild a specific service
docker compose build api

# View logs for a specific service
docker compose logs -f api

# Restart a service
docker compose restart api

# Execute commands inside a container
docker compose exec api bash

# Clean everything (containers, volumes, images)
docker compose down -v --rmi all
```

## 📚 Technologies Used

### Backend
- .NET 8.0
- Entity Framework Core
- MediatR
- AutoMapper
- JWT Authentication
- Swagger/OpenAPI

### Frontend
- Vue.js 3
- Vite
- Vue Router
- Pinia
- Tailwind CSS
- Axios

### DevOps
- Docker
- Docker Compose
- Nginx (to serve the store)

## 🤝 Contributing

1. Fork the project
2. Create a branch for your feature (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is private and confidential.

## 👥 Team

Firmeza Team - contact@firmeza.com

---

**Note**: For production, make sure to change the database credentials and JWT keys in the configuration files.
