# Campus Placement Management

Backend-first ASP.NET Core Web API for a campus placement platform. The implementation follows the supplied Technical Design Document: .NET 10, EF Core 10, SQL Server, ASP.NET Core Identity + JWT, layered architecture, role/resource authorization, college/company approvals, student enrollment and Excel import, jobs, eligibility, applications, recruitment drives, interviews and reports.

## Roles
- SuperAdmin
- PlacementOfficer
- CompanyHR
- Student

## Prerequisites
- .NET 10 SDK
- SQL Server / SQL Server Express
- Visual Studio 2026 or VS Code
- Git

## 1. Configure SQL Server
Edit `appsettings.json` and change `DefaultConnection` if your SQL Server instance is not `.\\SQLEXPRESS`.

## 2. Change the JWT secret
Replace `Jwt:Key` with a long random secret. Do not commit production secrets.

## 3. Restore and build
```powershell
dotnet restore
dotnet build
```

## 4. Create the first migration
The source design says migrations are EF Core generated. This starter contains the migration folder but does not fake a generated migration file.

```powershell
dotnet tool install --global dotnet-ef --version 10.0.0
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 5. Run
```powershell
dotnet run
```

Swagger will normally be available at the HTTPS URL printed by ASP.NET Core, followed by `/swagger`.

## Seeded SuperAdmin
Default development credentials from `appsettings.json`:
- Email: `admin@campusplacement.local`
- Password: `Admin@12345`

Change these values before any real deployment.

## Excel bulk enrollment format
The first worksheet is read. Row 1 must contain headers. Data columns are:
1. FullName
2. Email
3. MobileNumber
4. RegisterNumber
5. Department
6. Degree
7. GraduationYear
8. CGPA
9. ActiveBacklogs
10. ResumeUrl

## Important implementation note
The supplied design document lists the complete backend contracts but intentionally excludes Angular, deployment, logging, API versioning, rate limiting and file-upload functionality. ResumeUrl is therefore only a reference string, not a file upload.

## Main API areas
- `/api/auth`
- `/api/users`
- `/api/colleges`
- `/api/placement-officers`
- `/api/companies`
- `/api/company-hrs`
- `/api/students`
- `/api/jobs`
- `/api/eligibility`
- `/api/applications`
- `/api/recruitment-drives`
- `/api/interviews`
- `/api/reports`
