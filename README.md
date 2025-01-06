# EmployeeManagementSystem

## Overview
EmployeeManagementSystem is a Blazor WebAssembly application designed to manage employee data efficiently. This project leverages .NET 9 and provides a modern, responsive user interface for managing employee records.

## Features
- Add, update, and delete employee records
- View employee details
- Search and filter employees
- Responsive design for mobile and desktop
- Authentication and authorization

## Technologies Used
- Blazor WebAssembly
- .NET 9
- Entity Framework Core
- ASP.NET Core Web API
- SQL Server
- Bootstrap

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1. **Clone the repository:**
git clone https://github.com/ariusxscourger/EmployeeManagementSystem.git
cd EmployeeManagementSystem
2. **Set up the database:**
    - Update the connection string in `appsettings.json` located in the `Server` project.
    - Apply migrations to create the database schema:
    - 
3. **Run the application:**
    - Open the solution in Visual Studio 2022.
    - Set the `Server` project as the startup project.
    - Press `F5` to run the application.

## Project Structure
- `Client`: Contains the Blazor WebAssembly project.
- `Server`: Contains the ASP.NET Core Web API project.
- `Shared`: Contains shared models and services used by both Client and Server.

## Usage
1. **Home Page:**
    - Displays a list of employees with options to add, edit, or delete records.

2. **Add/Edit Employee:**
    - Form to input employee details such as name, position, department, and contact information.

3. **Employee Details:**
    - View detailed information about a specific employee.



