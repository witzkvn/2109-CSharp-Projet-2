using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WildPay.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }

        public User()
        {

        }
    }
}