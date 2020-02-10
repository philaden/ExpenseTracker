# ExpenseTracker
This is a simple expense tracker that allows a user to enter expenses and have them saved for later use. This project is written in asp.net core.


Kindly observe the following instructions to run the application using CLI
a. Run dotnet restore to install dependencies
b. Run dotnet build
c. To spin up local database, 

   - ensure the "server" variable in connection string in appsettings references the local instance of your SQL e.g. Server=(localdb)\mssqllocaldb;

   - navigate in the API project "ExpenseTracker" and run update-database.

   - run cd .. to navigate back into the API project "ExpenseTracker"

d. startup the application by running dotnet run

Kindly note the following instructions to test the endpoints:

On startup, the application baseUrl is ....

To access the API docentation kindly browse: http://localhost:5000/swagger/ui

The following endpoints are available for testing:

1. {baseUrl}/api/Expenses/setExpense: To set a new expense record.
2. {baseUrl}/api/Expenses/setExpenses: To post more than one expense data.
3. {baseUrl}/api/Expenses/getExpenses: To all fetch expenses data.
4. {baseUrl}/api/Expenses/getExpensesByDate: To fetch the list of expenses data by passing start and end dates
5. {baseUrl}/api/Expenses/deleteExpense: To remove expense record by its Id.
6. {baseUrl}/api/Expenses/updateExpense To update an expense record by passing the it's Id and the new update model.
