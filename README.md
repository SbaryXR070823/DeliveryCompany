# Courier Management System

## Overview

The Courier Management System is an application designed to facilitate the operational and logistical management of a courier company. It includes a user interface (Web), a database for storing information about orders, couriers, and customers, and an application server for processing this data. Users can access the services exclusively through the user interface.

## Functional Requirements

1. **Protected Access:** Access to the application is based on user authentication with a username and password.
   
2. **Order Management:** Automatically place, cancel, and confirm orders based on various criteria such as courier availability, package weight, and size.
   
3. **User Types:**
   - *Administrators:* Rights to add and edit employees, assign and manage vehicles.
   - *Customers:* Rights to place, cancel, and confirm orders, and view order status.
   - *Couriers:* Marking the start and end of the work/delivery day, and updating package status.

4. **Information Viewing:** Users can view the status of orders, ongoing deliveries, and order history at any time.

## Non-functional Requirements

1. **Security:** Ensure the protection of personal data.

## How to Run the Project

1. Update the connection strings in the app settings with your SQL Server.
2. Delete the content of the migration folder.
3. Run the following commands in the Package Manager Console (Visual Studio):
   ```
   add-migration <name> -Context AppIdentityDbAccess
   add-migration <name> -Context DataAppDbContext
   update-database -Context DataAppDbContext
   update-database -Context AppIdentityDbAccess
   ```

## Contributing

Contributions are welcome! Please follow the standard pull request process.

## License

This project is licensed under the [MIT License](LICENSE).
