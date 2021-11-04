﻿using System;
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

        //public int FkUserId { get; set; }
        //[ForeignKey("FkUserId")]
        //public User Author { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }

    }
}