# ExpenseTracker
This is a simple expense tracker that allows a user to enter expenses and have them saved for later use. A user can also see the value of the VAT (7.5%) that applied to each expense.

This project is written in asp.net core because it's open source while Microsoft SQL Server was used as datastore.

Kindly observe the following instructions to run the application using CLI
a. Run dotnet restore to install dependencies
b. Run dotnet build
c. To spin up local database, 

   - ensure the "server" variable in connection string in appsettings references the local instance of your SQL e.g. Server=(localdb)\mssqllocaldb;

   - navigate in the API project "ExpenseTracker.Api" and run update-database.

   - run cd .. to navigate back into the Solution Folder "ExpenseTracker"

d. startup the application by running dotnet run

Kindly note the following instructions to test the endpoints:

On startup, the application baseUrl is http://localhost:59408

To access the API docentation kindly browse: http://localhost:59408/swagger/index.html

The following endpoints are available for testing:

1. {baseUrl}/api/expenses/setExpense: To set a new expense record. 
Example:
POST http://localhost:59408/api/expenses/setExpense
{
	"Created" : "2020-02-07 10:00", 
	"ReasonForExpense" : "Payment of School Fee", 
	"ValueOfExpense" : 450000.00
}

2. {baseUrl}/api/expenses/getExpenses: To all fetch expenses and VAT data.
Example:
GET http://localhost:59408/api/expenses/getExpenses

3. {baseUrl}/api/expenses/getExpensesByDate: To fetch the list of expenses and VAT data by passing start and end dates
Example:
GET http://localhost:59408/api/expenses/getExpensesByDate?startDate=2020-02-07 10:00&endDate=2020-02-07 10:00

4. {baseUrl}/api/expenses/deleteExpense: To remove expense record by its Id.
Example:
GET http://localhost:59408/api/expenses/deleteExpense?Id=1

5. {baseUrl}/api/expenses/updateExpense To update an expense record by passing the its Id and the new update model.
Example:
POST http://localhost:59408/api/expenses/updateExpense
{
	"Id" : 1,
	"Created" : "2020-02-07 10:00", 
	"ReasonForExpense" : "Payment of School Fee", 
	"ValueOfExpense" : 410000.00
}


UNIT TEST:

All units tests are available in the "ExpenseTracker.Test" Project.


THANKS
