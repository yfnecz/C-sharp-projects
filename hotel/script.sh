#!/bin/sh

# Go to the target directory
cd /usercode/hotel/backend

# Start SQL Server
nohup /opt/mssql/bin/sqlservr --accept-eula start  > /dev/null 2>&1 &

# Create a migration
#dotnet ef migrations add InitialCreate

# Apply the migration to the database
dotnet ef database update

# Check if the database has been created
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Educative@123 -Q  "SELECT name FROM sys.databases;"

# Add rooms in SQL Server
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Educative@123 -Q  "INSERT INTO [hotel].[dbo].[rooms] (Type, Capacity, RatePerNight) VALUES ('Deluxe Suite', 2, 400);"

/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Educative@123 -Q  "INSERT INTO [hotel].[dbo].[rooms] (Type, Capacity, RatePerNight) VALUES ('Executive Suite', 1, 500);"

/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Educative@123 -Q  "INSERT INTO [hotel].[dbo].[rooms] (Type, Capacity, RatePerNight) VALUES ('Queen Room', 2, 300);"

# Check the inserted rooms by typing the following command
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Educative@123 -Q  "SELECT TOP (1000) [Id],[Type],[Capacity],[RatePerNight] FROM [hotel].[dbo].[Rooms]"

# Run the application in a new terminal
dotnet watch run --urls="http://0.0.0.0:3000"