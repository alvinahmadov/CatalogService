# Catalog Service

## Overview

**Catalog Service** is a comprehensive Windows Forms (WinForms) desktop application designed for efficient retail and inventory management. It provides a robust interface for managing product catalogs, tracking inventory levels, processing transactions, and analyzing sales data.

The solution follows a modular architecture, separating the User Interface, Business Logic/Data Access, and Network Communication into distinct projects to ensure maintainability and scalability.

## Architecture

The solution is organized into three main projects:

### 1. Catalog.Client (UI Layer)
The main entry point of the application. It is a **Windows Forms** application that delivers a rich and responsive user experience using **Telerik UI for WinForms**.

*   **Key Features**:
    *   **Dashboard & Controls**: Custom controls for Inventory, Shopping Cart, and Transaction History.
    *   **Data Visualization**: Grid views and layouts for managing large datasets.
    *   **Analytics**: Integrated Google Analytics for tracking application usage (`telerikProviderForGoogleAnalytics`).
*   **Technologies**: .NET Framework 4.5, Windows Forms, Telerik UI.

### 2. Catalog.Common (Business & Data Layer)
This library serves as the backbone of the application, housing the domain models, data access logic, and core services.

*   **Data Access**: Uses **Entity Framework 6** (Database-First approach via `ShopModel.edmx`) to interact with the underlying database (SQL Server or SQL Server Compact).
*   **Entities**: Manages core entities such as `Product`, `Category`, `Inventory`, `ShoppingCart`, `User`, and `TransactionHistory`.
*   **Managers**: Includes `DatabaseManager` for data operations and `RestAPIManager` for handling external API communications.
*   **Utilities**: Helper classes for logging, email, and globalization.

### 3. Catalog.Rest (Infrastructure Layer)
A specialized utility library that acts as a wrapper around **RestSharp**. It simplifies HTTP requests and response handling, providing a consistent interface for the application to consume external RESTful APIs.

*   **Components**: Custom `JsonRestClient`, `RestRequest`, and `RestResponse` wrappers to standardize API interactions.

## Technology Stack

*   **Programming Language**: C#
*   **Framework**: .NET Framework 4.5
*   **ORM**: Entity Framework 6.4.4
*   **UI Components**: Telerik UI for WinForms (2021.1.223.40)
*   **Database**: SQL Server / SQL Server Compact 4.0
*   **External Libraries**: RestSharp, Newtonsoft.Json

## Getting Started

### Prerequisites
*   Visual Studio 2019 or later (with .NET Desktop Development workload).
*   .NET Framework 4.5 Developer Pack.
*   SQL Server (LocalDB or Express) for the database.

### Installation
1.  Clone the repository:
    ```bash
    git clone https://github.com/your-repo/CatalogService.git
    ```
2.  Open the solution file `Catalog.sln` in Visual Studio.
3.  Restore NuGet packages:
    *   Right-click on the solution in Solution Explorer -> **Restore NuGet Packages**.
4.  Configure the database connection string in `App.config` (found in `Catalog.Client`).
5.  Build and Run the `Catalog.Client` project.

## Project Structure

```
CatalogService/
├── Catalog.Client/         # Main WinForms Application
│   ├── CustomControls/     # UI Components (Grids, Views)
│   ├── CustomForms/        # Application Windows (MainForm, Splash)
│   ├── Analytics/          # Google Analytics Integration
│   └── Program.cs          # Entry Point
│
├── Catalog.Common/         # Shared Logic & Data Access
│   ├── Models/             # Domain Models
│   ├── Service/            # Entity Framework Context & EDMX
│   ├── Repository/         # Data Access Repositories
│   └── Core/               # Core Managers (Database, REST)
│
└── Catalog.Rest/           # REST Client Wrapper
    ├── Classes/            # Request/Response implementations
    └── Interfaces/         # Client Interfaces
```
