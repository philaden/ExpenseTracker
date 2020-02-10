using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.Data.Domains;
using ExpenseTracker.Api.Repositories;
using ExpenseTracker.Api.Services.Implementations;
using ExpenseTracker.Api.Services.Interfaces;
using ExpenseTracker.Api.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ExpenseTracker.Test
{
    [TestFixture]
    public class ExpenseServiceTests
    {
        private IExpenseService _expenseService;
        private Mock<IBaseRepository<ExpenseTrackerContext>> _baseRepository;
        private Mock<ILogger<ExpenseService>> _logger;

        #region Test Data for expense service
        private const int _Id = 2;

        private readonly string _startDate = new DateTime(2020, 02, 07).ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        private readonly string _endDate = new DateTime(2020, 02, 08).ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        private readonly List<ReadExpenseVm> _expensesReadData = new List<ReadExpenseVm>
        {
            new ReadExpenseVm
            {
                Id = 1, Created = "2020-02-07 00:00", ReasonForExpense = "Payment of School Fee",  ValueOfExpense = 450000.00
            },
            new ReadExpenseVm
            {
                Id = 2, Created = "2020-02-07 00:00", ReasonForExpense = "Transportation",  ValueOfExpense = 450000.00
            },
            new ReadExpenseVm
            {
                Id = 3, Created = "2020-02-08 00:00", ReasonForExpense = "Leisure",  ValueOfExpense = 450000.00
            },
            new ReadExpenseVm
            {
                Id = 3, Created = "2020-02-08 00:00", ReasonForExpense = "Charity",  ValueOfExpense = 450000.00
            }
        };

        private readonly List<Expense> _expensesData = new List<Expense>
        {
            new Expense
            {
                Id = 1, Created = new DateTime(2020, 02, 07), ReasonForExpense = "Payment of School Fee",  ValueOfExpense = 450000.00,  IsActive = true, IsDeleted = false, Modified = null
            },
            new Expense
            {
                Id = 2, Created = new DateTime(2020, 02, 07), ReasonForExpense = "Transportation",  ValueOfExpense = 450000.00,  IsActive = true, IsDeleted = false, Modified = null
            },
            new Expense
            {
                Id = 3, Created = new DateTime(2020, 02, 08), ReasonForExpense = "Leisure",  ValueOfExpense = 450000.00,  IsActive = true, IsDeleted = false, Modified = null
            },
            new Expense
            {
                Id = 3, Created = new DateTime(2020, 02, 08) , ReasonForExpense = "Charity",  ValueOfExpense = 450000.00,  IsActive = true, IsDeleted = false, Modified = null
            }
        };

        private readonly ReadExpenseVm _expenseReadDatum = new ReadExpenseVm
        {
            Id = 2, Created = "2020-02-07 10:00", ReasonForExpense = "Payment of School Fee", ValueOfExpense = 450000.00
        };

        private readonly Expense _expenseDatum = new Expense
        {
            Id = 2, Created = new DateTime(2020, 02, 07), ReasonForExpense = "Payment of School Fee", ValueOfExpense = 450000.00, IsActive = true, IsDeleted = false, Modified = null
        };

        private readonly Expense _deleteExpenseDatum = new Expense
        {
            Id = 2, Created = new DateTime(2020, 02, 07), ReasonForExpense = "Payment of School Fee", ValueOfExpense = 450000.00, IsActive = true, IsDeleted = true, Modified = null
        };

        
        private readonly ExpenseVm _postExpenseDatum = new ExpenseVm
        {
            Created = "2020-02-07 10:00", ReasonForExpense = "Payment of School Fee", ValueOfExpense = 450000.00
        };

        private readonly UpdateExpenseVm _updateExpenseDatumVm = new UpdateExpenseVm
        {
            Id = 2,
            Created = "2020-02-07 10:00",
            ReasonForExpense = "Payment of School Fee",
            ValueOfExpense = 420000.00
        };

        private readonly UpdateExpenseVm _InvaidUpdateExpenseDatumVm = new UpdateExpenseVm
        {
            Id = 0,
            Created = "2020-02-07 10:00",
            ReasonForExpense = "Payment of School Fee",
            ValueOfExpense = 420000.00
        };

        private readonly ExpenseVm _InvaidUpdateExpenseDatum = new ExpenseVm
        {
            Created = "2020-02-07 10:00",
            ReasonForExpense = null,
            ValueOfExpense = 420000.00
        };

        #endregion

        [SetUp]
        public void SetUp()
        {
            _baseRepository = new Mock<IBaseRepository<ExpenseTrackerContext>>();
            _logger = new Mock<ILogger<ExpenseService>>();
            _expenseService = new ExpenseService(_baseRepository.Object, null);
        }

        [Test]
        public void ReadExpenses_GetValidExpensesData_ReturnsRecordsOfExpensesData()
        {
            _baseRepository.Setup(exp => exp.GetAll<Expense>()).Returns(_expensesData);

            var response = _expenseService.ReadExpenses();
            Assert.That(response.Count, Is.EqualTo(4));
            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public void ReadExpenses_GetValidExpensesDataByPassingValidDates_ReturnsRecordsOfExpensesWithinDateRange()
        {
            _baseRepository.Setup(exp => exp.GetAll<Expense>()).Returns(_expensesData);

            var response = _expenseService.ReadExpensesByDate(_startDate, _endDate);
            Assert.That(response.Count, Is.EqualTo(4));
            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public void ReadExpense_GetValidExpenseDataByPassingValidId_ReturnsExpenseDatum()
        {
            _baseRepository.Setup(exp => exp.GetById<Expense>(_Id)).Returns(_expenseDatum);

            var response = _expenseService.ReadExpense(_Id);
            Assert.That(response.ReasonForExpense, Is.EqualTo(_expenseReadDatum.ReasonForExpense));
            Assert.That(response.ValueOfExpense, Is.EqualTo(_expenseReadDatum.ValueOfExpense));
            Assert.That(response.Id, Is.EqualTo(_expenseReadDatum.Id));
            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public void SetExpense_PostInValidExpenseDatum_ReturnsFalsyValue()
        {
            _baseRepository.Setup(exp => exp.Create(_expenseDatum)).Returns(true);
            var response = _expenseService.SetExpense(null);
            Assert.That(response, Is.EqualTo(false));
        }

        [Test]
        public void UpdateExpense_PostValidIdAndExpenseDatum_ReturnsTruthyValue()
        {
            _baseRepository.Setup(exp => exp.GetById<Expense>(_Id)).Returns(_deleteExpenseDatum);
            _baseRepository.Setup(exp => exp.Update(_deleteExpenseDatum)).Returns(true);
            var response = _expenseService.UpdateExpense(_updateExpenseDatumVm);
            Assert.That(response, Is.EqualTo(true));
        }

        [Test]
        public void UpdateExpense_PostInValidId_ReturnsFalsyValue()
        {
            _baseRepository.Setup(exp => exp.Update(_expenseDatum)).Returns(true);
            var response = _expenseService.UpdateExpense(_InvaidUpdateExpenseDatumVm);
            Assert.That(response, Is.EqualTo(false));
        }

        [Test]
        public void UpdateExpense_PostInValidExpenseData_ReturnsFalsyValue()
        {
            _baseRepository.Setup(exp => exp.Update(_InvaidUpdateExpenseDatum)).Returns(false);
            var response = _expenseService.UpdateExpense(_InvaidUpdateExpenseDatumVm);
            Assert.That(response, Is.EqualTo(false));
        }

        [Test]
        public void DeleteExpense_PostValidId_ReturnsTruthyValue()
        {
            _baseRepository.Setup(exp => exp.GetById<Expense>(_Id)).Returns(_deleteExpenseDatum);
            _baseRepository.Setup(exp => exp.Update<Expense>(_deleteExpenseDatum)).Returns(true);
            var response = _expenseService.DeleteExpense(_Id);
            Assert.That(response, Is.EqualTo(true));
        }

        [Test]
        public void DeleteExpense_PostInValidId_ReturnsFalsyValue()
        {
            _baseRepository.Setup(exp => exp.Delete<Expense>(_Id)).Returns(true);
            var response = _expenseService.DeleteExpense(0);
            Assert.That(response, Is.EqualTo(false));
        }

        [Test]
        public void SetExpense_PostValidExpenseDatum_ReturnsTruthyValue()
        {
            _baseRepository.Setup(exp => exp.Create(_expenseDatum)).Returns(true);
            var response = _expenseService.SetExpense(_postExpenseDatum);
            Assert.That(response, Is.Not.Null);
        }

    }
}
