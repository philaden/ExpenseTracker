using ExpenseTracker.Api.Data.Domains;
using ExpenseTracker.Api.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Api.Services.Interfaces
{
    public interface IExpenseService
    {
        List<ReadExpenseVm> ReadExpensesByDate(string startDate, string endDate);
        List<ReadExpenseVm> ReadExpenses();
        ReadExpenseVm ReadExpense(int Id);
        bool SetExpense(ExpenseVm expense);
        bool UpdateExpense(UpdateExpenseVm newExpense);
        bool DeleteExpense(int Id);
    }
}
