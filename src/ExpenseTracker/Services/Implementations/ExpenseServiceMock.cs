using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Services.Implementations
{
    public class ExpenseServiceMock : IExpenseService
    {
        public bool DeleteExpense(int Id)
        {
            return Id > 0 ? true : false;
        }

        public ReadExpenseVm ReadExpense(int Id)
        {
            return new ReadExpenseVm
            {
                Created = "2020-02-07 10:00",
                ReasonForExpense = "Payment of School Fee",
                ValueOfExpense = 450000.00,
                Id = 2,
                IsActive = true
            };
        }

        public List<ReadExpenseVm> ReadExpenses()
        {
            return new List<ReadExpenseVm>
            {
                new ReadExpenseVm
                {
                    Id = 1, Created = "2020-02-07 00:00", ReasonForExpense = "Payment of School Fee",  ValueOfExpense = 450000.00,  IsActive = true
                },
                new ReadExpenseVm
                {
                    Id = 2, Created = "2020-02-07 00:00", ReasonForExpense = "Transportation",  ValueOfExpense = 450000.00,  IsActive = true
                },
                new ReadExpenseVm
                {
                    Id = 3, Created = "2020-02-08 00:00", ReasonForExpense = "Leisure",  ValueOfExpense = 450000.00,  IsActive = true
                },
                new ReadExpenseVm
                {
                    Id = 3, Created = "2020-02-08 00:00", ReasonForExpense = "Charity",  ValueOfExpense = 450000.00,  IsActive = true
                }
            };
        }

        public List<ReadExpenseVm> ReadExpensesByDate(string startDate, string endDate)
        {
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate)) return new List<ReadExpenseVm>();
            return new List<ReadExpenseVm>
            {
                new ReadExpenseVm
                {
                    Id = 1, Created = "2020-02-07 00:00", ReasonForExpense = "Payment of School Fee",  ValueOfExpense = 450000.00,  IsActive = true
                },
                new ReadExpenseVm
                {
                    Id = 2, Created = "2020-02-07 00:00", ReasonForExpense = "Transportation",  ValueOfExpense = 450000.00,  IsActive = true
                },
                new ReadExpenseVm
                {
                    Id = 3, Created = "2020-02-08 00:00", ReasonForExpense = "Leisure",  ValueOfExpense = 450000.00,  IsActive = true
                },
                new ReadExpenseVm
                {
                    Id = 3, Created = "2020-02-08 00:00", ReasonForExpense = "Charity",  ValueOfExpense = 450000.00,  IsActive = true
                }
            };
        }

        public bool SetExpense(ExpenseVm expense)
        {
            return expense == null ? false : true;
        }

        public void SetExpenses(IEnumerable<ExpenseVm> expenses)
        {
            throw new NotImplementedException();
        }

        public bool UpdateExpense(int Id, ExpenseVm newExpense)
        {
            return (newExpense == null || (Id == 0 || Id < 0)) ? false: true;
        }
    }
}
