using ExpenseTracker.Data.Domains;
using ExpenseTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IExpenseService
    {
        List<ReadExpenseVm> ReadExpensesByDate(string startDate, string endDate);
        List<ReadExpenseVm> ReadExpenses();
        ReadExpenseVm ReadExpense(int Id);
        bool SetExpense(ExpenseVm expense);
        void SetExpenses(IEnumerable<ExpenseVm> expenses);
        bool UpdateExpense(int Id, ExpenseVm newExpense);
        bool DeleteExpense(int Id);
    }
}
