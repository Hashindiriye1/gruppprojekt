# Car Service Booking App

A **Car Service Booking** application with a .NET API backend, React frontend, and SQL Server database. Users can manage customers, vehicles, services, locations, and bookings from the web interface.

---

## Requirements checklist

### Backend (.NET API, Clean Architecture)

- **Entities (5):** Customer, Vehicle, Service, Location, Booking
- **Relations:** Customer has many Vehicles and Bookings; Vehicle and Service/Location link to Bookings
- **DTOs and mapping:** AutoMapper in the Application layer
- **Services and repositories:** One per entity in Application and Infrastructure
- **Validation:** FluentValidation for create/update DTOs
- **Logging:** ILogger in services
- **Global error handling:** `GlobalExceptionMiddleware`
- **Advanced endpoints:**
  - `GET /api/bookings/filtered?status=&fromDate=&toDate=&sortBy=&sortOrder=` (filter and sort bookings)
  - `GET /api/bookings/upcoming-by-location/{id}?limit=` (upcoming bookings by location)
  - `GET /api/services/with-bookings` (services with customer and vehicle per booking)

### Frontend (React)

- **Pages:** Home, Customers (list + form), Vehicles (list + form), Services (list + form), Locations (list + form), Bookings (list + form)
- **CRUD from UI:** Create and edit Customers, Vehicles, Services, Locations, and Bookings from the app
- **Lists and navigation:** All entities listed with Edit links; "New" actions in the header
- **API integration:** `src/api.ts` and `VITE_API_URL` for backend base URL
- **Loading and error states:** Handled on each page
- **Environment:** `VITE_API_URL` optional; dev proxy forwards `/api` to the backend

### Database (SQL Server)

- **Migrations:** EF Core migrations in the Infrastructure project
- **Relations:** Configured in `CarWashDbContext`
- **Indexes:** e.g. on `Booking.ScheduledDate`, `Booking.Status`, `(LocationId, ScheduledDate)`, `Vehicle.CustomerId`

---

## How to run

### 1. Database

- Use SQL Server (LocalDB is the default).
- Connection string in `src/CarWashBooking.API/appsettings.json`:
  - `Server=(localdb)\\mssqllocaldb;Database=CarWashBooking;...`
- From the repository root, apply migrations:

```bash
dotnet ef database update --project src/CarWashBooking.Infrastructure --startup-project src/CarWashBooking.API
```

### 2. Backend

```bash
cd src/CarWashBooking.API
dotnet run
```

- API base: http://localhost:5218 (or the port in launchSettings.json).
- Swagger (Development): http://localhost:5218/swagger

### 3. Frontend

```bash
cd client
npm install
npm run dev
```

- App: http://localhost:5173
- In dev, Vite proxies `/api` to the backend (see `vite.config.ts`, default target http://localhost:5218).
- To use another backend, set `VITE_API_URL` (e.g. in `.env`) to the full API base URL.

### 4. Seed data (optional)

Create data via the app (Customers, Vehicles, Services, Locations, Bookings) or via Swagger/HTTP:

1. **Customers** – `POST /api/customers`  
   Body: `{ "name": "Anna Test", "email": "anna@test.se", "phone": "070-1234567" }`
2. **Vehicles** – `POST /api/vehicles`  
   Body: `{ "customerId": 1, "licensePlate": "ABC123", "make": "Volvo", "model": "V60" }`
3. **Services** – From the app: Services, then "New service". Or `POST /api/services`  
   Body: `{ "name": "Oil change", "description": "Full synthetic", "price": 499, "durationMinutes": 30 }`
4. **Locations** – From the app: Locations, then "New location". Or `POST /api/locations`  
   Body: `{ "name": "City Center", "address": "Storgatan 1" }`
5. **Bookings** – From the app: Bookings, then "New booking". Or `POST /api/bookings`  
   Body: `{ "customerId": 1, "vehicleId": 1, "serviceId": 1, "locationId": 1, "scheduledDate": "2025-02-10T10:00:00Z", "notes": null }`

You can then use the React app to manage all entities and try the Bookings filter/sort and upcoming-by-location features.

---

## Solution layout

- **src/CarWashBooking.Domain** – Entities (Customer, Vehicle, Service, Location, Booking)
- **src/CarWashBooking.Application** – DTOs, AutoMapper profile, validators, services, repository interfaces
- **src/CarWashBooking.Infrastructure** – EF Core DbContext, repositories, SQL Server
- **src/CarWashBooking.API** – Controllers, global exception middleware, DI
- **client** – React (Vite) SPA: pages, forms, API client, loading and error handling
