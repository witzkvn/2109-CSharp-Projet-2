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

        public int FkUserId { get; set; }
        [ForeignKey("FkUserId")]
        public User User { get; set; }

        public int FkCategoryId { get; set; }
        [ForeignKey("FkCategoryId")]
        public Category Category { get; set; }

        public int FkGroupId { get; set; }
        [ForeignKey("FkGroupId")]
        public Group Group { get; set; }

        public Expense()
        {

        }
    }
}