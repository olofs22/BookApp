# BookQuoteApp

A responsive CRUD web application for books and favourite quotes, built with **Angular 20** (frontend) and a **.NET 9 Web API** (backend). Users register, log in with JWT and manage books and their own quotes.

## Live version

- Frontend: https://icy-stone-056cf8d0f.3.azurestaticapps.net
- Backend (Swagger): https://bookappbackend-eda3cga0fkg5a0hj.swedencentral-01.azurewebsites.net/swagger

> The database is Azure SQL Serverless with auto-pause. If nobody has used the app for a while, the first request can take 20-60 seconds while the database wakes up. Wait a moment and try again.

## Features

- Register an account and log in (JWT)
- A book list visible to all logged-in users, showing who added each book
- Add, edit and delete books (only the owner can edit or delete their own book)
- "My quotes": private favourite quotes, max 5 per user (add, edit, delete)
- Responsive design with Bootstrap, with a hamburger menu on small screens
- Font Awesome icons
- Light/dark theme toggle in the navbar

## Tech stack

| Part | Technology |
|---|---|
| Frontend | Angular 20 (standalone components), Reactive Forms, Bootstrap 5, Font Awesome |
| Backend | .NET 9, ASP.NET Core Web API, Entity Framework Core, ASP.NET Core Identity |
| Authentication | JWT Bearer tokens, stored in `localStorage` and attached by an HTTP interceptor |
| Database | Azure SQL Database |
| Hosting | Azure Web App (backend), Azure Static Web Apps (frontend, deployed via GitHub Actions) |

## How it works

- **Auth:** `POST /api/auth/register` creates a user, `POST /api/auth/login` returns a JWT. Angular stores the token and an interceptor adds `Authorization: Bearer <token>` to every request.
- **Protection:** All book and quote endpoints require a valid token (`[Authorize]`). A route guard redirects users who are not logged in to `/login`.
- **Ownership:** Books can be seen by everyone but only edited or deleted by the user who created them. Quotes are always filtered by the logged-in user, and the limit of 5 quotes is enforced in the backend (the frontend only hides the button).
- **Authors and publishers** are created automatically when a book or quote is added, so there are no duplicates (matching is case-insensitive).

## API overview

| Endpoint | Description |
|---|---|
| `POST /api/auth/register` | Register a user |
| `POST /api/auth/login` | Log in, returns a token |
| `GET/POST /api/books`, `GET/PUT/DELETE /api/books/{id}` | Books |
| `GET/POST /api/quotes`, `GET/PUT/DELETE /api/quotes/{id}` | Own quotes (max 5, `POST` returns `400` when the limit is reached) |
| `GET /api/authors`, `GET /api/publishers` | Lists |

## Running locally

**Requirements:** .NET 9 SDK, Node.js (LTS), Angular CLI 20, access to a SQL Server / Azure SQL database.

### Backend

```bash
cd BookQuoteApp.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your connection string>"
dotnet user-secrets set "Jwt:Key" "<long random key, at least 32 characters>"
dotnet user-secrets set "Jwt:Issuer" "BookQuoteApp"
dotnet user-secrets set "Jwt:Audience" "BookQuoteAppUsers"
dotnet user-secrets set "Jwt:ExpiryMinutes" "60"
dotnet ef database update
dotnet run
```

The API starts on `http://localhost:5199` (Swagger at `/swagger`).

### Frontend

```bash
cd BookQuoteApp.Client
npm install
ng serve
```

Open `http://localhost:4200`. The frontend picks the API address automatically: `localhost:5199` locally, otherwise the hosted backend.

## Project structure

```
BookApp/
├── BookQuoteApp.Api/      # .NET 9 Web API (Controllers, Models, DTOs, Data, Services)
└── BookQuoteApp.Client/   # Angular 20 (core, features, shared)
```

## Security

Secrets (connection string, JWT key) are not committed to Git. Locally they live in .NET User Secrets, and in Azure they are stored in App Service Configuration.
