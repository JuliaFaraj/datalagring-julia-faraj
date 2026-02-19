# Datalagring - CoursesManager

Ett skolprojekt byggt med .NET och React där man kan hantera kurser via ett API.

Projektet använder Clean Architecture/DDD-struktur och Entity Framework Core mot en SQL Server databas.

## Funktioner

### Backend (.NET Minimal API)
- Hämta alla kurser
- Hämta kurs via courseCode
- Skapa kurs
- Uppdatera kurs
- Ta bort kurs
- Optimistic concurrency via RowVersion
- Validering av unika courseCode

### Frontend (React + Vite)
- Visa lista med kurser
- Sök/filter i frontend
- Redigera kurs (PUT)
- Skapa kurs från frontend (POST)
- Enkel felhantering
- Koppling mot API via fetch

## Tekniker

### Backend
- .NET
- Minimal API
- Entity Framework Core (Code First)
- SQL Server
- Dependency Injection

### Frontend
- React
- TypeScript
- Vite

## Projektstruktur (förenklad)
CoursesManager
│
├── CoursesManager.Presentation (API / Minimal API)
├── CoursesManager.Application (Services, DTOs)
├── CoursesManager.Domain (Entities)
├── CoursesManager.Infrastructure (EF Core, repositories)
└── frontend (React)

## Starta projektet

## Starta projektet lokalt

### Backend (.NET API)
1. Öppna solutionen i Visual Studio.
2. Starta projektet:

CoursesManager.Presentation


API och Swagger startar på:

https://localhost:7032/swagger


---

### Frontend (React + Vite)

Gå till frontend-mappen:

```bash
npm install
npm run dev
Frontend körs normalt på:

http://localhost:5173
