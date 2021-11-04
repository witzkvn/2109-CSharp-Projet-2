namespace WildPay.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WildPay.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WildPay.DAL.WildPayContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WildPay.DAL.WildPayContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //List<User> users = new List<User>
            //{
            //    new User {Firstname = "first_1", Lastname=""}
            //};
        }
    }
}
