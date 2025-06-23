# UniventV2

[![Back-End Build Status](https://github.com/LukeX19/UniventV2/actions/workflows/dotnet.yml/badge.svg)](https://github.com/LukeX19/UniventV2/actions/workflows/dotnet.yml)
[![Front-End Build Status](https://github.com/LukeX19/UniventV2/actions/workflows/nodejs.yml/badge.svg)](https://github.com/LukeX19/UniventV2/actions/workflows/nodejs.yml)

**Univent** is a web-based event management platform tailored for university students and administrators. It enables students to discover, join, and host academic or social events, while administrators manage user access, universities, and event categories. The application integrates artificial intelligence to enhance user interaction through personalized event suggestions.

## Table of Contents
1. [Features](#features)
2. [Technologies](#technologies)
3. [Getting Started](#getting-started)
4. [Running the Project](#running-the-project)
5. [API Documentation](#api-documentation)

## Features
- User Roles: Students and Administrators with different access levels
- Event System: Create, Browse, Update, Cancel, and Join/Withdraw from events
- Location Integration: Google Maps API with Marker Placement and Autocomplete
- Authentication & Authorization: JWT-based Security with Role Guards
- Media Upload: Store Event and Profile Images via AWS S3
- AI Event Suggestions: Powered by GitHub Marketplace AI Models
- Weather Integration: OpenWeatherMap Used in Event Suggestions
- Responsive UI: Built with Angular, Tailwind CSS, and Angular Material

## Technologies
### Backend (ASP.NET Core 8)
- Layered architecture (Domain, Application, Infrastructure, API)
- Entity Framework Core (SQL Server)
- MediatR (CQRS pattern)
- AWS S3 Integration
- GitHub Marketplace Models & OpenWeatherMap APIs
- JWT Authentication

### Frontend (Angular)
- Feature-first modular architecture
- Role-based route guards
- Responsive SPA using Tailwind CSS & Angular Material
- Google Maps for event geolocation
- HttpInterceptor for JWT token forwarding

## Getting Started

### Prerequisites
1. **.NET 8 SDK**: Required for running the backend. [Download .NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **Node.js**: Required for running the frontend. [Download Node.js 18+ LTS](https://nodejs.org/)
3. **Angular CLI 18**: Required for running the frontend.
```bash
npm install -g @angular/cli@18
```

### Cloning the Repository
Clone the repository to your local machine:
```bash
git clone https://github.com/LukeX19/UniventV2
```

## Running the Project

### Backend (API)
Before running the backend, make sure your `appsettings.json` or `secrets.json` file is properly configured with the following credentials and keys:
- Database connection string
- AWS S3 access credentials
- GitHub Marketplace Models Key & OpenWeatherMap API keys
- JWT signing key

The file should look like this:
```bash
{
  "ConnectionStrings": {
    "MSSQLServer": "YOUR_KEY"
  },
  "JwtSettings": {
    "SigningKey": "YOUR_KEY",
    "Issuer": "UniventAPI",
    "Audiences": [ "Swagger-Client" ]
  },
  "AWSS3StorageSettings": {
    "BucketName": "YOUR_BUCKET_NAME",
    "Region": "YOUR_REGION",
    "AccessKey": "YOUR_ACCESS_KEY",
    "SecretKey": "YOUR_SECRET_KEY",
    "S3BaseUrl": "YOUR_BASE_URL"
  },
  "OpenWeatherMap": {
    "ApiKey": "YOUR_KEY"
  },
  "GitHubAIModels": {
    "Endpoint": "https://models.github.ai/inference",
    "ApiKey": "YOUR_KEY",
    "Model": "YOUR_CHOSEN_AI_MODEL"
  }
}
```

Once configured, don’t forget to apply the latest database migrations.

Steps to run the Backend:

1. Navigate to the API Project:
```bash
cd Univent.Api
```
2. Restore Backend Dependencies:
```bash
dotnet restore
```
3. Build the Backend:
```bash
dotnet build Univent.Api
```
4. Run the Backend:
- Start the API server:
```bash
dotnet run --project Univent.Api
```
- The API should be running at https://localhost:7019 (or another port if configured).
- Experienced users may skip steps 2 and 3, as `dotnet run` handles them automatically.

### Frontend (Client)
Before running the frontend, make sure to create the `src/environments` folder with 2 environment files: `environment.prod.ts` and `environment.ts`.
The files should look like this:
```bash
// environment.prod.ts
export const environment = {
  production: true,
  apiUrl: 'api/',
  googleMapsApiKey: 'YOUR_GOOGLE_MAPS_KEY'
};


// environment.ts
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7019/api',
  googleMapsApiKey: 'YOUR_GOOGLE_MAPS_KEY'
};
```
Steps to run the Frontend:

1. Navigate to the Frontend Project:
```bash
cd client/Univent
```
2. Install Dependencies:
```bash
npm install
```
3. Run the Frontend:
- Start the development server:
```bash
ng serve
```
- The frontend should be running at http://localhost:4200 by default.

## API Documentation

The backend API includes an interactive documentation that allows you to explore and test available endpoints directly from your browser.
- Swagger UI: Visit https://localhost:7019/swagger to access the Swagger interface, where you can view all available API requests, expected inputs and outputs, and possible error responses.
