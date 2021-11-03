using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WildPay.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("User")]
        public int FKuserId { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }

    }
}