using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WildPay.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public double Value { get; set; }

        [ForeignKey("User")]
        public int FKuserId { get; set; }

        [ForeignKey("Category")]
        public int FKcategoryId { get; set; }

        [ForeignKey("Group")]
        public int FKgroupId { get; set; }

        public Expense()
        {

        }
    }
}