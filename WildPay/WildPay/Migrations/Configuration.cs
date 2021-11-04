namespace WildPay.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using WildPay.Models;
    using WildPay.Tools;

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

            User adminUser = new User { Firstname = "admin", Lastname = "ADMIN", Email = "admin@test.com", Password = FormatTools.HashPassword("admin1*") };

            context.Users.AddOrUpdate(adminUser);
            context.SaveChanges();

            List<Category> categories = new List<Category>
            {
                new Category {Name = "Cat1" },
                new Category {Name = "Cat2" },
                new Category {Name = "Cat3" },
            };

            categories.ForEach(c => context.Categories.AddOrUpdate(p => p.Id, c));
            context.SaveChanges();

            List<Group> groups = new List<Group>
            {
                new Group {Author = adminUser, Name = "First", Categories = categories},
                new Group {Author = adminUser, Name = "Second", Categories = categories},
            };
            groups.ForEach(g => context.Groups.AddOrUpdate(p => p.Id, g));
            context.SaveChanges();


            List<User> users = new List<User>
            {
                new User {Firstname = "first_1", Lastname="last_1", Email="test1@test.com", Password=FormatTools.HashPassword("test1*") },
                new User {Firstname = "first_2", Lastname="last_2", Email="test2@test.com", Password=FormatTools.HashPassword("test2*") },
                new User {Firstname = "first_3", Lastname="last_3", Email="test3@test.com", Password=FormatTools.HashPassword("test3*") },
                new User {Firstname = "first_4", Lastname="last_4", Email="test4@test.com", Password=FormatTools.HashPassword("test4*") },
                new User {Firstname = "first_5", Lastname="last_5", Email="test5@test.com", Password=FormatTools.HashPassword("test5*") },
            };

            users.ForEach(u => context.Users.AddOrUpdate(p => p.Id, u));
            context.SaveChanges();

            AddDefaultImgToUser(context, "first_1");
            AddDefaultImgToUser(context, "first_2");
            AddDefaultImgToUser(context, "first_5");


            context.SaveChanges();

            AddUserToGroup(context, "first_1", "First");
            AddUserToGroup(context, "first_2", "First");
            AddUserToGroup(context, "first_3", "First");
            AddUserToGroup(context, "first_4", "Second");
            AddUserToGroup(context, "first_5", "Second");
            AddUserToGroup(context, "first_1", "Second");

            context.SaveChanges();

            List<Expense> expenses = new List<Expense>
            {
                new Expense {Title = "exp1", Value=22.43 },
                new Expense {Title = "exp2", Value=2.55 },
                new Expense {Title = "exp3", Value=550 },
                new Expense {Title = "exp4", Value=0.67 },
                new Expense {Title = "exp5", Value=64.64 },
                new Expense {Title = "exp6", Value=64.64 },
                new Expense {Title = "exp7", Value=10.02 },
            };

            expenses.ForEach(e => context.Expenses.AddOrUpdate(p => p.Id, e));
            context.SaveChanges();

            LinkExpenseToUser(context, "exp1", "first_1", "First");
            LinkExpenseToUser(context, "exp2", "first_1", "First");
            LinkExpenseToUser(context, "exp3", "first_2", "First");
            LinkExpenseToUser(context, "exp4", "first_4", "First");
            LinkExpenseToUser(context, "exp5", "first_5", "Second");
            LinkExpenseToUser(context, "exp6", "first_1", "Second");
            LinkExpenseToUser(context, "exp7", "first_5", "First");

            context.SaveChanges();
        }

        void AddUserToGroup(WildPay.DAL.WildPayContext context, string firstname, string groupname)
        {
            Group grp = context.Groups.SingleOrDefault(g => g.Name == groupname);
            User usr = grp.Users.SingleOrDefault(u => u.Firstname == firstname);
            if (usr == null)
                grp.Users.Add(context.Users.Single(u => u.Firstname == firstname));
        }

        void AddDefaultImgToUser(WildPay.DAL.WildPayContext context, string firstname)
        {
            string combinedPath = Path.Combine("Content\\Images", "default.png");
            byte[] imagebyte = File.ReadAllBytes(combinedPath);
            User usr = context.Users.SingleOrDefault(u => u.Firstname == firstname);
            usr.Image = imagebyte;
        }

        void LinkExpenseToUser(WildPay.DAL.WildPayContext context, string expenseTitle, string userFirstname, string groupName)
        {
            Group grp = context.Groups.SingleOrDefault(g => g.Name == groupName);
            Expense exp = context.Expenses.SingleOrDefault(e => e.Title == expenseTitle);
            User usr = context.Users.SingleOrDefault(u => u.Firstname == userFirstname);

            exp.User = usr;
            exp.Group = grp;
        }
    }
}
