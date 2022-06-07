using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Api.ViewModels
{
    public class ReadExpenseVm : ExpenseVm
    {
        [Required]
        public int Id { get; set; }
        public double VatAmount { get; set; }
        public string Created { get; set; }
    }
}
