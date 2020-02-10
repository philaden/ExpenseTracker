using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Api.ViewModels
{
    public class UpdateExpenseVm
    {
        [Required]
        public int Id { get; set; }
        public double ValueOfExpense { get; set; }
        public string ReasonForExpense { get; set; }
        public string Created { get; set; }
    }
}
