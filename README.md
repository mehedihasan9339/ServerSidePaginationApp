# Employee List Web Application

This project is a simple web application for displaying a list of employees with server-side pagination using DataTables and ASP.NET Core. It includes features like a dynamic table with data fetched from the server, custom pagination styling, and a loader animation for improved user experience.

## Features

- **Server-Side Pagination**: Efficient handling of large datasets by fetching and displaying data on demand.
- **Dynamic Pagination Helper**: Implemented to allow dynamic pagination for any tables.
- **Custom Styling**: Professionally styled table and pagination with hover effects and responsive design.
- **Loader Animation**: Displays a spinner while data is being fetched to provide feedback to users.
- **AJAX Loading**: Data is loaded asynchronously using AJAX, ensuring smooth user interactions without full-page refreshes.
- **DataTables Integration**: Uses DataTables to enhance the functionality of HTML tables with features like sorting, searching, and pagination.
- **Server-Side Pagination in Web API**: The Web API handles pagination logic to efficiently retrieve data for the front end.

## Technologies Used

- **ASP.NET Core**: Backend framework for building the API and server-side logic.
- **DataTables**: jQuery plugin for enhancing HTML tables with pagination, searching, and sorting.
- **Bootstrap**: CSS framework for responsive design and layout.
- **jQuery**: JavaScript library for AJAX requests and DOM manipulation.
- **SQL Server**: Used as the database for storing employee data.

## Prerequisites

To run this project locally, you'll need the following:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6 or above)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- A modern web browser (Google Chrome, Firefox, Edge)

## Screenshots

Here is a screenshot of the employee list view:

#### Employee List View
![Employee List Screenshot](https://raw.githubusercontent.com/mehedihasan9339/ServerSidePaginationApp/refs/heads/master/ServerSidePaginationApp/server-side-pagination.png)

## Inserting Dummy Data

To test the application with a large dataset, you can insert 10 million dummy records into the `Employees` table by running the following SQL script in your SQL Server database:

```sql
DECLARE @Counter INT = 1;
DECLARE @MaxCount INT = 10000000; -- 10 million
DECLARE @Name NVARCHAR(50);
DECLARE @Position NVARCHAR(50);
DECLARE @Office NVARCHAR(50);
DECLARE @Salary INT;

WHILE @Counter <= @MaxCount
BEGIN
    -- Generate random data for each employee
    SET @Name = 'Employee ' + CAST(@Counter AS NVARCHAR);
    SET @Position = 'Position ' + CAST((ABS(CHECKSUM(NEWID())) % 5 + 1) AS NVARCHAR); -- Random position 1-5
    SET @Office = 'Office ' + CAST((ABS(CHECKSUM(NEWID())) % 10 + 1) AS NVARCHAR); -- Random office 1-10
    SET @Salary = ABS(CHECKSUM(NEWID())) % (120000 - 40000 + 1) + 40000; -- Random salary between 40,000 and 120,000

    INSERT INTO Employees (Name, Position, Office, Salary)
    VALUES (@Name, @Position, @Office, @Salary);

    SET @Counter = @Counter + 1;
END
```

You can also use the following queries to insert dummy data into the Departments table for testing purposes:

```
DECLARE @Counter INT = 1;
DECLARE @MaxCount INT = 5000; -- Set to 5000
DECLARE @Name NVARCHAR(50);
DECLARE @Location NVARCHAR(50);
DECLARE @EmployeeCount INT;

WHILE @Counter <= @MaxCount
BEGIN
    -- Generate random data for each department
    SET @Name = 'Department ' + CAST(@Counter AS NVARCHAR);
    SET @Location = 'Location ' + CAST((ABS(CHECKSUM(NEWID())) % 10 + 1) AS NVARCHAR); -- Random location 1-10
    SET @EmployeeCount = ABS(CHECKSUM(NEWID())) % 100 + 1; -- Random employee count between 1 and 100

    INSERT INTO Departments (Name, Location, EmployeeCount)
    VALUES (@Name, @Location, @EmployeeCount);

    SET @Counter = @Counter + 1;
END
```
