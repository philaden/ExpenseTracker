using ExpenseTracker.Api.Controllers;
using ExpenseTracker.Api.Data.Domains;
using ExpenseTracker.Api.Helpers;
using ExpenseTracker.Api.Services.Interfaces;
using ExpenseTracker.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ExpenseTracker.Test
{
    public class ExpenseControllerTest
    {
        private MockRepository _mockRepository;
        private Mock<IExpenseService> _expenseService;
        private Mock<ILogger<ExpensesController>> _logger;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _expenseService = _mockRepository.Create<IExpenseService>();
            _logger = _mockRepository.Create<ILogger<ExpensesController>>();
        }

        private ExpensesController GetController()
        {
            return new ExpensesController(_expenseService.Object, _logger.Object);
        }


        #region Test Data for expense Controller
        private const int _Id = 2;

        private readonly string _startDate = new DateTime(2020, 02, 07).ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        private readonly string _endDate = new DateTime(2020, 02, 08).ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        private readonly List<ReadExpenseVm> _expensesReadData = new List<ReadExpenseVm>
        {
            new ReadExpenseVm
            {
                Id = 1, Created = "2020-02-07 00:00", ReasonForExpense = "Payment of School Fee",  ValueOfExpense = 450000.00, VatAmount = 31395.35
            },
            new ReadExpenseVm
            {
                Id = 2, Created = "2020-02-07 00:00", ReasonForExpense = "Transportation",  ValueOfExpense = 450000.00, VatAmount = 31395.35
            },
            new ReadExpenseVm
            {
                Id = 3, Created = "2020-02-08 00:00", ReasonForExpense = "Leisure",  ValueOfExpense = 450000.00, VatAmount = 31395.35
            },
            new ReadExpenseVm
            {
                Id = 3, Created = "2020-02-08 00:00", ReasonForExpense = "Charity",  ValueOfExpense = 450000.00, VatAmount = 31395.35
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
            Id = 2,
            Created = "2020-02-07 10:00",
            ReasonForExpense = "Payment of School Fee",
            ValueOfExpense = 450000.00,
            VatAmount = 31395.35
        };

        private readonly Expense _expenseDatum = new Expense
        {
            Id = 2,
            Created = new DateTime(2020, 02, 07),
            ReasonForExpense = "Payment of School Fee",
            ValueOfExpense = 450000.00,
            IsActive = true,
            IsDeleted = false,
            Modified = null
        };

        private readonly Expense _deleteExpenseDatum = new Expense
        {
            Id = 2,
            Created = new DateTime(2020, 02, 07),
            ReasonForExpense = "Payment of School Fee",
            ValueOfExpense = 450000.00,
            IsActive = true,
            IsDeleted = true,
            Modified = null
        };


        private readonly ExpenseVm _postExpenseDatum = new ExpenseVm
        {
            Created = "2020-02-07 10:00",
            ReasonForExpense = "Payment of School Fee",
            ValueOfExpense = 450000.00
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


        [Test]
        public void GetExpenses_GetValidExpensesDataFromJsonResult_ReturnsRecordsOfExpensesData()
        {
            _expenseService.Setup(exp => exp.ReadExpenses()).Returns(_expensesReadData);
            var response = GetController();
            var res = (ResponseData)response.GetExpenses().Value;
            Assert.That(res.Status, Is.EqualTo(true));
            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public void GetExpenses_GetValidExpensesDataByPassingValidDates_ReturnsRecordsOfExpensesWithinDateRange()
        {
            _expenseService.Setup(exp => exp.ReadExpensesByDate(_startDate, _endDate)).Returns(_expensesReadData);
            var response = GetController();
            var res = (ResponseData)response.GetExpensesByDate(_startDate, _endDate).Value;
            Assert.That(res.Data, Is.Not.Null);
            Assert.That(res.Status, Is.True);
        }

        public void GetExpenses_PassingInValidDates_ReturnsFailureMessage()
        {
            _expenseService.Setup(exp => exp.ReadExpensesByDate(null, _endDate)).Returns(new List<ReadExpenseVm>());
            var response = GetController();
            var res = (ResponseData)response.GetExpensesByDate(null, _endDate).Value;
            Assert.That(res.Data, Is.Null);
            Assert.That(res.Status, Is.False);
        }

        [Test]
        public void GetExpense_GetSuccessfulResultByPassingValidId_ReturnsExpenseDatum()
        {
            _expenseService.Setup(exp => exp.ReadExpense(_Id)).Returns(_expenseReadDatum);
            var response = GetController();
            var res = (ResponseData)response.GetExpense(_Id).Value;
            Assert.That(((ReadExpenseVm)res.Data).Id, Is.EqualTo(_expenseReadDatum.Id));
            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public void SetExpense_PassingValidExpenseDatum_ReturnsSuccessfulResponse()
        {
            _expenseService.Setup(exp => exp.SetExpense(_postExpenseDatum)).Returns(true);
            var response = GetController();
            var res = (ResponseData)response.SetExpense(_postExpenseDatum).Value;
            Assert.That(res.Status, Is.True);
        }

    }
}
