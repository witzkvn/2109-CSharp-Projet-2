namespace WildPay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Expense",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Value = c.Double(nullable: false),
                        FkUserId = c.Int(nullable: false),
                        FkCategoryId = c.Int(nullable: false),
                        FkGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.FkCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Group", t => t.FkGroupId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.FkUserId, cascadeDelete: true)
                .Index(t => t.FkUserId)
                .Index(t => t.FkCategoryId)
                .Index(t => t.FkGroupId);
            
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Firstname = c.String(maxLength: 20),
                        Lastname = c.String(maxLength: 20),
                        Password = c.String(nullable: false),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupCategory",
                c => new
                    {
                        Group_Id = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.Category_Id })
                .ForeignKey("dbo.Group", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.UserGroup",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Group_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Group_Id })
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Group", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Group_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroup", "Group_Id", "dbo.Group");
            DropForeignKey("dbo.UserGroup", "User_Id", "dbo.User");
            DropForeignKey("dbo.Expense", "FkUserId", "dbo.User");
            DropForeignKey("dbo.Expense", "FkGroupId", "dbo.Group");
            DropForeignKey("dbo.GroupCategory", "Category_Id", "dbo.Category");
            DropForeignKey("dbo.GroupCategory", "Group_Id", "dbo.Group");
            DropForeignKey("dbo.Expense", "FkCategoryId", "dbo.Category");
            DropIndex("dbo.UserGroup", new[] { "Group_Id" });
            DropIndex("dbo.UserGroup", new[] { "User_Id" });
            DropIndex("dbo.GroupCategory", new[] { "Category_Id" });
            DropIndex("dbo.GroupCategory", new[] { "Group_Id" });
            DropIndex("dbo.Expense", new[] { "FkGroupId" });
            DropIndex("dbo.Expense", new[] { "FkCategoryId" });
            DropIndex("dbo.Expense", new[] { "FkUserId" });
            DropTable("dbo.UserGroup");
            DropTable("dbo.GroupCategory");
            DropTable("dbo.User");
            DropTable("dbo.Group");
            DropTable("dbo.Expense");
            DropTable("dbo.Category");
        }
    }
}
