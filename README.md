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

## User Guide

### Main Functionalities

1. **Homepage and Logo**:
   - The homepage provides a simple layout for accessing registration or login functionality.
   - The logo is designed specifically for this project.
   - From the Navbar, non-registered users can access the Dashboard and sub-items of the four main sections. Further actions require registration and login.
   - Logged-in users are redirected to the Dashboard and cannot access the Homepage.

2. **Registration**:
   - New users can register in two ways:
     - **Create a New Account**: Enter a User Name, E-mail, Password, and Position.
     - **Register with Google**: Use an existing Google account and fill out additional required information, including Position.
   - Position selection is crucial as it determines user permissions.
   - After successful registration, users are redirected to the Dashboard.

3. **Login**:
   - Registered users can log in using their User Name and Password or Google authentication.

4. **Logout**:
   - Only logged-in users can log out. This option is not visible for others.

5. **Seed Database**:
   - Available only for users with the **Manager** role via the navigation bar.
   - The Seeder populates the database only if it is empty. Existing records prevent seeding.

6. **Statistics**:
   - Accessed via the "Show Statistics" button on the Dashboard.
   - Displays comparative job and requisition statistics.
   - Managers can also view budget statistics against the predefined budget (currently set at 500,000).

7. **Dashboard**:
   - The Dashboard serves as the central hub for navigating the four main menus:
     - **Creators Page**
     - **Jobs Handling**
     - **Inventory**
     - **Purchasing**

8. **Creators Page**:
   - Allows creation and management of core application entities:
     - **Consumables**: Create, edit, view, and delete consumable items used for various equipment.
     - **Spare Parts**: Create spare parts specific to equipment. Ensure the equipment exists before creating spare parts.
     - **Equipment**: Associate equipment with a maker and link routine maintenances or consumables during creation.
     - **Routine Maintenances**: Non-specific maintenance tasks that can be created first and later associated with equipment.
     - **Specific Maintenances**: Maintenance tasks linked to specific equipment, created after the equipment.
     - **Countries and Cities**: Used to complete supplier addresses. Deletion is restricted if referenced in an address.
     - **Suppliers**: Manage companies supplying spare parts and consumables. Multiple suppliers can be linked to a single item and vice versa.
     - **Manuals**: Upload maintenance manuals. Equipment and Maker fields are required.

9. **Jobs Handling**:
   - Manage job scheduling and history:
     - **Create Job Order**: Define periodic jobs for specific equipment with intervals and due dates.
     - **Jobs for Today**: View due jobs.
     - **All Scheduled Jobs**: Display all jobs irrespective of due dates.
     - **Job History**: View past jobs.

10. **Inventory**:
    - Manage inventory for spare parts and consumables:
      - **Spare Parts Inventory**: Update real stock values to match physical inventory.
      - **Consumables Inventory**: Similar to spare parts but for consumables.

11. **Purchasing**:
    - Handle requisitions for spare parts and consumables:
      - **Create Requisition**: Select items, specify quantities, and choose suppliers. Requisitions are created and await approval.
      - **Approve Requisition**: Managers and Engineers can approve requisitions if the budget allows. Approved requisitions update stock and deduct the cost from the budget.
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
