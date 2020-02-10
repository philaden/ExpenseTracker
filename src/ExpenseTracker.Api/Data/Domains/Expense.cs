using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Api.Data.Domains
{
    public class Expense : BaseEntity
    {
        [Required]
        public double ValueOfExpense { get; set; }

        [Required]
        public string ReasonForExpense { get; set; }
    }
}
