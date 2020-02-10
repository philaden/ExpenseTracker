using ExpenseTracker.Controllers;
using ExpenseTracker.Data.Domains;
using ExpenseTracker.Repositories;
using ExpenseTracker.Services.Implementations;
using ExpenseTracker.Services.Interfaces;
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
    public class ExpenseControllerTests
    {
        private IExpenseService _expenseService; 
        private Mock<ILogger<ExpensesController>> _logger;
        private ExpensesController _expensesController;

        private const int _Id = 2;

        private readonly string _startDate = new DateTime(2020, 02, 07).ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        private readonly string _endDate = new DateTime(2020, 02, 08).ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        [SetUp]
        public void SetUp()
        {
            _expenseService = new ExpenseServiceMock();
            _logger = new Mock<ILogger<ExpensesController>>();

            _expensesController = new ExpensesController(_expenseService, _logger.Object);
        }

    }
}
