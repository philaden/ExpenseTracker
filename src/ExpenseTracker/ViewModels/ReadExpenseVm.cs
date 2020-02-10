using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModels
{
    public class ReadExpenseVm : ExpenseVm
    {
        [Required]
        public new int Id { get; set; }
        public bool IsActive { get; set; }
    }
}
