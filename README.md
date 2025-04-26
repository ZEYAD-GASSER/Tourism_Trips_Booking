# Tourism Trips Booking

A web application for booking tourism trips, built using **ASP.NET Core MVC** and **Entity Framework Core**.

---
## 📌 Project Features
-- User Authentication
-- Admin Dashboard
-- Trip Search & Booking
-- User Dashboard
-- Mock Payment System
-- Reviews and Ratings
-- Customizable Travel Packages

## 🛠️ Technologies & Tools
- ASP.NET Core MVC (**v8.0**)
- Entity Framework Core
- SQL Server
- Visual Studio 2022


## 📦 Required NuGet Packages
Before running the project, make sure to install the following NuGet packages:

```bash
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
```
## 🔧 Database Configuration
Open the file:
Models/Tourism_Trips_Booking_Context.cs
In the OnConfiguring method, change the connection string to match your SQL Server setup
```bash
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer(
        "Data Source=YOUR_SERVER_NAME;Initial Catalog=Tourism_Trips_Booking;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
}
```
Replace YOUR_SERVER_NAME with your actual SQL Server name









