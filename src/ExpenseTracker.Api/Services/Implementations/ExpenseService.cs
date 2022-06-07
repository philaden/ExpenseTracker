using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.Data.Domains;
using ExpenseTracker.Api.Repositories;
using ExpenseTracker.Api.Services.Interfaces;
using ExpenseTracker.Api.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Api.Services.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly IBaseRepository<ExpenseTrackerContext> _baseRepository;
        private readonly ILogger<ExpenseService> _logger;

        public ExpenseService(IBaseRepository<ExpenseTrackerContext> baseRepository, ILogger<ExpenseService> logger)
        {
            _baseRepository = baseRepository;
            _logger = logger;
        }

        public List<ReadExpenseVm> ReadExpenses()
        {
            try
            {
                var expenses = _baseRepository.GetAll<Expense>().Where(x => x.IsActive && !x.IsDeleted).ToList();

                var readExpenses = expenses.Select(x =>
                    new ReadExpenseVm
                    {
                        Id = x.Id,
                        VatAmount = EstimateVatAmount(x.ValueOfExpense),
                        ValueOfExpense = x.ValueOfExpense,
                        ReasonForExpense = x.ReasonForExpense,
                        Created = x.Created.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
                    }).ToList();

                return readExpenses;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException + "||" + e.StackTrace);
                return new List<ReadExpenseVm>();
            }
        }

        public List<ReadExpenseVm> ReadExpensesByDate(string startDate, string endDate)
        {
            try
            {
                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate)) return new List<ReadExpenseVm>();

                var convertedStartDate = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                var convertedEndDate = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                
                var expenses = _baseRepository.GetAll<Expense>().
                    Where(x => x.Created >= convertedStartDate && x.Created <= convertedEndDate && !x.IsDeleted && x.IsActive).ToList();

                var readExpenses = expenses.Select(x =>
                    new ReadExpenseVm
                    {
                        Id = x.Id,
                        VatAmount = EstimateVatAmount(x.ValueOfExpense),
                        ValueOfExpense = x.ValueOfExpense,
                        ReasonForExpense = x.ReasonForExpense,
                        Created = x.Created.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
                    }).ToList();

                return readExpenses;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException + "||" + e.StackTrace);
                return new List<ReadExpenseVm>();
            }
        
        }

        public ReadExpenseVm ReadExpense(int Id)
        {
            try
            {
                if (Id < 0 || Id == 0) return new ReadExpenseVm();
                var data = _baseRepository.GetById<Expense>(Id);
                return new ReadExpenseVm
                {
                    Id = data.Id,
                    VatAmount = EstimateVatAmount(data.ValueOfExpense),
                    ValueOfExpense = data.ValueOfExpense,
                    ReasonForExpense = data.ReasonForExpense,
                    Created = data.Created.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException + "||" + e.StackTrace);
                return new ReadExpenseVm();
            }
        }

        public bool SetExpense(ExpenseVm expense)
        {
            try
            {
                if (expense == null) return false;

                var expObj = new Expense
                {
                    ReasonForExpense = expense.ReasonForExpense,
                    ValueOfExpense = expense.ValueOfExpense,
                    IsActive = true
                };

                var status = _baseRepository.Create(expObj);
                return status;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException + "||" + e.StackTrace);
                return false;
            }
        }


        public bool UpdateExpense(UpdateExpenseVm newExpense)
        {
            try
            {
                if (newExpense.Id < 0 || newExpense.Id == 0)
                {
                    return false;
                }
                else if (newExpense == null){
                    return false;
                }
                else
                {
                    var data = _baseRepository.GetById<Expense>(newExpense.Id);
                    data.ValueOfExpense = newExpense.ValueOfExpense;
                    data.ReasonForExpense = newExpense.ReasonForExpense;
                    data.Created = DateTime.ParseExact(newExpense.Created, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    data.Modified = DateTime.Now;
                    return _baseRepository.Update(data);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException + "||" + e.StackTrace);
                return false;
            }
        }
        public bool DeleteExpense(int Id)
        {
            try
            {
                if (Id < 0 || Id == 0) return false;

                var data = _baseRepository.GetById<Expense>(Id);
                data.IsDeleted = true;
                data.Modified = DateTime.Now;

                return _baseRepository.Update<Expense>(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException + "||" + e.StackTrace);
                return false;
            }
        }

        private double EstimateVatAmount(double expenseValue)
        {
            const double VatRate = 0.075d;
            var vatValue = expenseValue * (VatRate / (1 + VatRate));
            return Math.Round(vatValue, 2);
        }
    }
}
