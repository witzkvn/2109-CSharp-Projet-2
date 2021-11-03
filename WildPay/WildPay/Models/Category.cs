using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildPay.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }

        public Category()
        {

        }
    }
}