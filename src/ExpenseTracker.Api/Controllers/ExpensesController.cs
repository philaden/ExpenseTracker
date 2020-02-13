using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Api.Data.Domains;
using ExpenseTracker.Api.Helpers;
using ExpenseTracker.Api.Services.Interfaces;
using ExpenseTracker.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExpenseTracker.Api.Controllers
{
    [Route("api/expenses")]
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
        
        //PLEASE NOTE:  These endpoints is an Implementatiion of Microsoft Specification "JSEND" for JSON Result


        /// <summary>
        /// This endpoint fetches all expenses records set by the user
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getExpenses")]
        public JsonResult GetExpenses()
        {
            try
            {
                var expensesData = _expenseService.ReadExpenses();

                return !expensesData.Any() ?
                   Json(ResponseData.SendFailMsg("There are no expenses and vat data available, Kindly set your expenses for tracking")) :
                   Json(ResponseData.SendSuccessMsg(data: expensesData));
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException);
                return Json(ResponseData.SendExceptionMsg(e));
            }
        }

        /// <summary>
        /// This endpoint fetches expenses and vat data based on date parameters
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet, Route("getExpensesByDate")]
        public JsonResult GetExpensesByDate(string startDate, string endDate)
        {
            try
            {
                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate)) return Json(ResponseData.SendFailMsg("Please provide valid date parameters" ));
                var expensesData = _expenseService.ReadExpensesByDate(startDate, endDate);

                return !expensesData.Any() ?
                   Json(ResponseData.SendFailMsg("There are no expenses and vat data available for this period")) :
                   Json(ResponseData.SendSuccessMsg(data: expensesData));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException + "||" + e.StackTrace);
                return Json(ResponseData.SendExceptionMsg(e));
            }
        }


        /// <summary>
        /// This endpoint fetches an expense datum by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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
                _logger.LogError(e.Message + "=>" + e.InnerException + "||" + e.StackTrace);
                return Json(ResponseData.SendExceptionMsg(e));
            }
        }

        /// <summary>
        /// This endpoint allows the user to set new expense record
        /// </summary>
        /// <param name="expense"></param>
        /// <returns></returns>
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
                _logger.LogError(e.Message + "=>" + e.InnerException + "||" + e.StackTrace);
                return Json(ResponseData.SendExceptionMsg(e));
            }             
        }

        /// <summary>
        /// This endpoint updates an existing expense data 
        /// </summary>
        /// <param name="expense"></param>
        /// <returns></returns>
        [HttpPost, Route("updateExpense")]
        public JsonResult UpdateExpense([FromBody] UpdateExpenseVm expense)
        {
            try
            {
                if (expense.Id < 0 || expense.Id == 0) return Json(ResponseData.SendFailMsg("Kindly provide a valid Id parameter"));
                if (expense == null) return Json(ResponseData.SendFailMsg("Kindly Pass a Valid Expense Object"));
                return _expenseService.UpdateExpense(expense) ? Json(ResponseData.SendSuccessMsg("Successfully updated record")) 
                    : Json(ResponseData.SendFailMsg($"Unable to updated record"));              
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "=>" + e.InnerException + "||" + e.StackTrace);
                return Json(ResponseData.SendExceptionMsg(e));
            }
        }

        /// <summary>
        /// This endpoint deletes an existing expense datum 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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
                _logger.LogError(e.Message + "=>" + e.InnerException + "||" + e.StackTrace);
                return Json(ResponseData.SendExceptionMsg(e));
            }
        }

    }
}