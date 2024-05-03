CoreApiWithEntity API

This project is a practice exercise focusing on building a CRUD (Create, Read, Update, Delete) API using ASP.NET Core 6 with Entity Framework, and utilizing SQL Server Management Studio (SSMS) as the database. It follows the Repository pattern for data access and utilizes JSON Web Tokens (JWT) for authentication.

Setup Instructions

Clone Repository:
git clone <repository_url>

Navigate to Project Directory:
cd practice-crud

Restore Dependencies:
dotnet restore


Database Setup:
Ensure SQL Server is installed and running.
Create a database in SSMS named CoreApiWithEntityDB.
Adjust connection string in appsettings.json if needed.


Run Migrations:
dotnet ef database update

Run Application:
dotnet run

Endpoints
POST /api/auth/login
Authenticates user and generates JWT token.
json
Copy code
{
  "username": "example",
  "password": "example_password"
}

GET /api/product/obj
Retrieves Product with pagination. Requires JWT token in the request headers.

GET /api/product/{id}
Retrieves a product by ID. Requires JWT token in the request headers.

POST /api/product
Adds or updates a product. Requires JWT token in the request headers.

DELETE /api/product/{id}
Deletes a product by ID. Requires JWT token in the request headers.

GET /api/product/GetCategories
Retrieves all categories. Requires JWT token in the request headers.

GET /api/product/GetProductCategory
Retrieves product category master data. Requires JWT token in the request headers.

 Features

User Registration: Implement an endpoint for user registration where new users can sign up with a username and password.

User Roles and Permissions: Extend the authentication system to include roles (e.g., Admin, User) and permissions. This allows for more granular control over access to API endpoints.

Swagger Documentation: Integrate Swagger UI to automatically generate interactive API documentation. This makes it easier for developers to explore and test the API endpoints.

Input Validation: Implement input validation to ensure that incoming data meets certain criteria (e.g., required fields, data types) before processing it. This helps prevent invalid data from being persisted to the database.

Logging: Integrate logging to record important events and errors that occur during the execution of the API. This can be useful for troubleshooting issues and monitoring system health.

Error Handling: Enhance error handling to provide more informative and user-friendly error messages to clients. This helps users understand what went wrong and how to resolve the issue.

File Uploads: If applicable, implement endpoints for uploading files (e.g., images, documents). Make sure to handle file validation and storage securely.

Email Notifications: Implement email notifications for certain events (e.g., user registration, password reset) to keep users informed and engaged with your application.


Authentication
Authentication in this API is performed using JSON Web Tokens (JWT). To authenticate, make a POST request to /api/auth/login with a JSON object containing the username and password. Upon successful authentication, the server will respond with a JWT token. Include this token in the Authorization header for subsequent requests to protected endpoints.

Technologies Used
ASP.NET Core 6
Entity Framework Core
SQL Server Management Studio (SSMS)

Dependencies
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools

Contributing
Contributions are welcome. Please open an issue or submit a pull request with your improvements.

License
This project is licensed under the MIT License.

Contact
For any inquiries or feedback, please contact yagyajoshi.synsoft@gmail.com.

Acknowledgments
Thank you to the ASP.NET Core and Entity Framework Core teams for providing excellent documentation and resources.