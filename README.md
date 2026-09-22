# Academix

**Academix** is a web-based academic management portal built with **ASP.NET Core MVC, .NET 10, SQL Server, and Dapper**.

It provides separate workflows for **Administrators, Teachers, and Students** to manage classes, subjects, enrollments, assignments, submissions, grading, and academic information.

---

## Features

### Authentication & Authorization

- ASP.NET Core Identity with Dapper
- Role-based access control
- Admin, Teacher, and Student roles
- Secure login/logout
- Password policy and account lockout
- Email confirmation support
- Session timeout

### Admin

Administrators can manage the overall academic system:

- Manage teachers
- Manage students
- Manage subjects
- Create and manage classes
- Assign teachers to classes
- Manage student enrollments
- View academic information
- Manage assignments and submissions
- Access system-wide management pages

### Teacher

Teachers can manage their assigned classes and assignments:

- View assigned classes
- Create, edit, and delete assignments
- Upload assignment files
- Set assignment deadlines
- Set maximum marks
- Publish or save assignments as drafts
- View student submissions
- View submitted files
- Give marks and feedback
- Edit marks individually
- Edit/save marks for multiple students
- Download submissions
- Export submission information

### Student

Students can manage their academic activities:

- View enrolled classes
- View published assignments
- View assignment details and files
- Submit assignment files
- Update submissions when permitted
- View marks
- View teacher feedback

### Email Notifications

- Sends email notifications for assignment-related activities.
- Uses configurable SMTP settings.
- Uses HTML email templates for notification messages.

### Assignment & Submission Management

- Class-based assignments
- Assignment file upload
- Student submission file upload
- Submission update support
- Deadline-based submission workflow
- Teacher grading
- Decimal/fractional marks
- Teacher feedback
- Submission file viewing/downloading
- Bulk grading
- Export submission lists

### User Interface

- ASP.NET Core Razor Views
- Bootstrap
- jQuery
- AJAX
- DataTables
- Font Awesome
- Responsive management interfaces
- Server-side DataTables processing

---

## Technology Stack

- **.NET 10**
- **ASP.NET Core MVC**
- **C#**
- **Dapper**
- **SQL Server**
- **ASP.NET Core Identity**
- **VeryGood.AspNetCore.Identity.Dapper**
- **jQuery**
- **Bootstrap**
- **DataTables**
- **AJAX**
- **ClosedXML**

---

## Project Structure

```text
Academix/
│
├── BusinessLayer/
│   ├── Models/
│   └── Services/
│
├── DataAccessLayer/
│   ├── DataAccess/
│   ├── DbScripts/
│   ├── SP/
│   └── SqlDb/
│
└── Portal/
    ├── Controllers/
    ├── Models/
    ├── Views/
    ├── Helpers/
    └── wwwroot/
```

The project follows a layered architecture:

```text
Portal (MVC)
     │
     ▼
Business Layer
     │
     ▼
Data Access Layer (Dapper)
     │
     ▼
SQL Server
```

---

## Database

Academix uses:

```text
SQL Server
Database: AcademixDB
```

The database contains the required Identity tables and application tables for:

- Users and roles
- Teachers
- Students
- Subjects
- Classes
- Teacher enrollments
- Student enrollments
- Assignments
- Submissions

Database operations are primarily implemented using **Dapper and SQL Server stored procedures**.

---

## Installation

### Prerequisites

Install:

- .NET 10 SDK
- SQL Server
- SQL Server Management Studio (SSMS)
- Visual Studio 2022 or another compatible IDE

### 1. Clone the Repository

```bash
git clone https://github.com/Adil-Ahnaf/Academix.git
cd Academix
```

### 2. Create the Database

Open:

```text
DataAccessLayer/DbScripts/FullDbScript.sql
```

Run the complete script in **SQL Server Management Studio**.

This script creates the `AcademixDB` database, tables, relationships, and stored procedures required by the application.

> If necessary, update the SQL Server database file paths in `FullDbScript.sql` according to your local SQL Server installation.

### 3. Configure the Connection String

Open:

```text
Portal/appsettings.json
```

Update the database server name:

```json
"ConnectionStrings": {
  "Portal_DbConnection": "Server=localhost;Database=AcademixDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Use the SQL Server instance name appropriate for your environment.

### 4. Restore and Build

From the repository root:

```bash
dotnet restore
dotnet build
```

### 5. Run the Application

```bash
dotnet run --project Portal/Portal.csproj
```

Or open the solution in Visual Studio and run the **Portal** project.

---

## Initial Login Credentials

The seed data provides initial users for testing.

| Role | Password |
|---|---|
| **Admin** | `Admin@1234` |
| **Teacher** | `Teacher@1234` |
| **Student** | `Student@1234` |

The corresponding usernames/email addresses can be found in:

```text
DataAccessLayer/DbScripts/Seeding Initial Data.sql
```

Run the seed script after `FullDbScript.sql` if the sample users are not already present.

> These are development/testing credentials. Change them before using the application in a production environment.

---

## Database Scripts

```text
DataAccessLayer/DbScripts/
├── FullDbScript.sql
└── Seeding Initial Data.sql
```

- **FullDbScript.sql** — Creates the complete database structure and stored procedures.
- **Seeding Initial Data.sql** — Adds initial/sample users and academic data.

---

## License

No open-source license has currently been specified for this repository.
