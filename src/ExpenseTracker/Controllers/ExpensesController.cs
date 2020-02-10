using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Helpers;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly ILogger<ExpensesController> _logger;

        public ExpensesController(IExpenseService expenseService, ILogger<ExpensesController> logger)
        {
            _expenseService = expenseService;
            _logger = logger;
        }
        
        [HttpGet, Route("getExpenses")]
        public JsonResult GetExpenses()
        {
            try
            {
                var expensesData = _expenseService.ReadExpenses();

                return !expensesData.Any() ?
                   Json(ResponseData.SendFailMsg("There are no expenses data available, Kindly set your expenses for tracking")) :
                   Json(ResponseData.SendSuccessMsg(data: expensesData));
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException);
                return Json(ResponseData.SendExceptionMsg(e));
            }
        }


        [HttpGet, Route("getExpensesByDate")]
        public JsonResult GetExpensesByDate(string startDate, string endDate)
        {
            try
            {
                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate)) return Json(ResponseData.SendFailMsg("Please provide valid date parameters" ));
                var expensesData = _expenseService.ReadExpensesByDate(startDate, endDate);

                return !expensesData.Any() ?
                   Json(ResponseData.SendFailMsg("There are no expenses data available for this period")) :
                   Json(ResponseData.SendSuccessMsg(data: expensesData));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException);
                return Json(ResponseData.SendExceptionMsg(e));
            }
        }

        [HttpGet, Route("getExpense")]
        public JsonResult GetExpense(int Id)
        {
            try
            {
                if (Id < 0 || Id == 0) return Json(ResponseData.SendFailMsg("Kindly provide a valid Id parameter"));
                var expenseDatum = _expenseService.ReadExpense(Id);

                return expenseDatum == null ?
                   Json(ResponseData.SendFailMsg($"Unable to fetch record with Id {Id}")) :
                   Json(ResponseData.SendSuccessMsg(data: expenseDatum));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException);
                return Json(ResponseData.SendExceptionMsg(e));
            }
        }

        [HttpPost, Route("setExpense")]
        public JsonResult SetExpense([FromBody] ExpenseVm expense)
        {
            try
            {
                if (expense == null) return Json(ResponseData.SendFailMsg("Kindly Pass a Valid Expense Object"));
                return _expenseService.SetExpense(expense) ? Json(ResponseData.SendSuccessMsg("Successfully added new expense data")) :
                    Json(ResponseData.SendFailMsg());
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException);
                return Json(ResponseData.SendExceptionMsg(e));
            }             
        }

        [HttpPost, Route("setExpenses")]
        public JsonResult SetExpenses([FromBody] IEnumerable<ExpenseVm> expenses)
        {
            try
            {
                if (expenses == null || !expenses.Any()) return Json(ResponseData.SendFailMsg("Kindly Pass valid expense Objects"));
                _expenseService.SetExpenses(expenses);
                return Json(ResponseData.SendSuccessMsg());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException);
                return Json(ResponseData.SendExceptionMsg(e));
            }
        }

        [HttpPost, Route("updateExpense")]
        public JsonResult UpdateExpense(int Id, [FromBody] ExpenseVm expense)
        {
            try
            {
                if (Id < 0 || Id == 0) return Json(ResponseData.SendFailMsg("Kindly provide a valid Id parameter"));
                if (!ModelState.IsValid || expense == null) return Json(ResponseData.SendFailMsg("Kindly Pass a Valid Expense Object"));
                return _expenseService.UpdateExpense(Id, expense) ? Json(ResponseData.SendFailMsg($"Unable to updated record")) :
                   Json(ResponseData.SendSuccessMsg("Successfully updated record"));                   
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException);
                return Json(ResponseData.SendExceptionMsg(e));
            }
        }

        [HttpGet, Route("deleteExpense")]
        public JsonResult DeleteExpense(int Id)
        {
            try
            {
                if (Id < 0 || Id == 0) return Json(ResponseData.SendFailMsg("Kindly provide a valid Id parameter"));
                return _expenseService.DeleteExpense(Id) ? Json(ResponseData.SendSuccessMsg($"Successfully deleted record")) :
                   Json(ResponseData.SendFailMsg("unable to delete record"));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException);
                return Json(ResponseData.SendExceptionMsg(e));
            }
        }

    }
}