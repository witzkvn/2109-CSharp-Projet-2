using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WildPay.BDD
{
    public class WildPayContext : DbContext
    {
        public WildPayContext() : base("WildPayContext")
        {

        }

        public DbSet<User> User { get; set; }
    }
}