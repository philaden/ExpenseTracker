using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Api.Data.Domains
{
    public abstract class BaseEntity
    {
        [Required]
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        protected BaseEntity()
        {
            IsActive = true;
            IsDeleted = false;
        }
    }
}
