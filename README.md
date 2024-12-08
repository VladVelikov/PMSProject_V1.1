# PMSWeb: Planned Maintenance System

PMSWeb is a functional application designed for small factories or industrial units to streamline maintenance operations, including job planning, inventory management, requisitions, and cost monitoring.

---

## Features

- **Job Planning**: Plan and track tasks efficiently.
- **Inventory Management**: Maintain spare parts and consumable records.
- **Requisition Management**: Create and approve requisitions.
- **Cost Monitoring**: Track costs and budget depletion over time.

---

## Technologies Used

### Core Technologies

- **Languages**: C#, HTML, CSS, JavaScript
- **Backend**: C# with .NET 8.0 (Developed using Visual Studio)
- **Frontend**: Razor View Engine with sections and partial views
- **Database**: Microsoft SQL Server with Entity Framework Core (ORM)

### Additional Features

- **Authentication**: ASP.NET Identity with Google External Provider for Registration/Login
- **Security**: SQL Injection, XSS, CSRF protection, and anti-forgery tokens for `POST` methods
- **File Management**: Google Cloud Storage for user file uploads

---

## Setup Instructions

### Prerequisites

1. **SQL Server**: Ensure a running instance of SQL Server (free version is sufficient).  
   *(Future updates will transition to cloud-based databases.)*
2. **Connection String**: Add the provided connection string to your project's User Secrets.
3. **Google Cloud Storage Credentials**:
   - Unzip the `CredentialsGStorage_UnzipHere.zip` file.
   - Place the extracted files in the `wwwroot` directory.

### First Run

1. Run the application; it will automatically create the database and tables.
2. Register the first user as a "Manager" to initialize the system.
3. Optionally, activate the seeder to populate the database with sample data.
4. Start using PMSWeb with all features enabled.

### Role-Based Permissions

| Role        | Permissions                                       |
|-------------|---------------------------------------------------|
| **Manager** | Full access to all features.                     |
| **Engineer**| Restricted from deleting records and seeding data.|
| **Technician**| Cannot delete records, seed data, or approve requisitions.|

---

## Software Details

- **Backend Architecture**: Repository pattern and Dependency Injection for better manageability.
- **Frontend**: Razor View Engine for server-side rendering, minimal JavaScript usage.
- **Database**: Soft delete implemented for relevant records.
- **Security**: Anti-forgery tokens and server-side validation.
- **Error Handling**: Custom 404 and 500 error pages.
- **Scalability**: Separate application layers enable future API and frontend upgrades.

---

## User Guide *(To Be Updated)*

1. Homepage and Logo
2. Registration
3. Login
4. Logout
5. Seed Database
6. Statistics
7. Dashboard
8. Creators Page
9. Jobs Handling
10. Inventory
11. Purchasing

---

## Future Enhancements

- Add Web API for extensibility.
- Transition to a modern frontend framework (e.g., React, Angular).
- Implement advanced seeders with cloud-based JSON data.

---

## Contributions

Contributions are welcome! Please fork the repository, open issues, or submit pull requests.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

Feel free to use PMSWeb to manage your factory maintenance tasks effectively! 🚀
