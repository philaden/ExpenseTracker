using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Api.ViewModels
{
    public class ExpenseVm
    {
        [Required]
        public double ValueOfExpense { get; set; }
        [Required]
        public string ReasonForExpense { get; set; }
    }
}
