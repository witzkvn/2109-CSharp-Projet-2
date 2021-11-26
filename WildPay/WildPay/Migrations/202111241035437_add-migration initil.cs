namespace WildPay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmigrationinitil : DbMigration
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
                        Id = c.Int(nullable: false, identity: true),
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
                        Id = c.Int(nullable: false, identity: true),
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
                        Firstname = c.String(nullable: false, maxLength: 20),
                        Lastname = c.String(nullable: false, maxLength: 20),
                        Password = c.String(nullable: false),
                        UserImage = c.Binary(),
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
            
            CreateStoredProcedure(
                "dbo.Category_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 20),
                    },
                body:
                    @"INSERT [dbo].[Category]([Name])
                      VALUES (@Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Category]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Category] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Category_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 20),
                    },
                body:
                    @"UPDATE [dbo].[Category]
                      SET [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Category_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Category]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Expense_Insert",
                p => new
                    {
                        CreatedAt = p.DateTime(),
                        Title = p.String(maxLength: 50),
                        Value = p.Double(),
                        FkUserId = p.Int(),
                        FkCategoryId = p.Int(),
                        FkGroupId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Expense]([CreatedAt], [Title], [Value], [FkUserId], [FkCategoryId], [FkGroupId])
                      VALUES (@CreatedAt, @Title, @Value, @FkUserId, @FkCategoryId, @FkGroupId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Expense]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Expense] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Expense_Update",
                p => new
                    {
                        Id = p.Int(),
                        CreatedAt = p.DateTime(),
                        Title = p.String(maxLength: 50),
                        Value = p.Double(),
                        FkUserId = p.Int(),
                        FkCategoryId = p.Int(),
                        FkGroupId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Expense]
                      SET [CreatedAt] = @CreatedAt, [Title] = @Title, [Value] = @Value, [FkUserId] = @FkUserId, [FkCategoryId] = @FkCategoryId, [FkGroupId] = @FkGroupId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Expense_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Expense]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Group_Insert",
                p => new
                    {
                        CreatedAt = p.DateTime(),
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[Group]([CreatedAt], [Name])
                      VALUES (@CreatedAt, @Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Group]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Group] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Group_Update",
                p => new
                    {
                        Id = p.Int(),
                        CreatedAt = p.DateTime(),
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[Group]
                      SET [CreatedAt] = @CreatedAt, [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Group_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Group]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.User_Insert",
                p => new
                    {
                        Email = p.String(),
                        Firstname = p.String(maxLength: 20),
                        Lastname = p.String(maxLength: 20),
                        Password = p.String(),
                        UserImage = p.Binary(),
                    },
                body:
                    @"INSERT [dbo].[User]([Email], [Firstname], [Lastname], [Password], [UserImage])
                      VALUES (@Email, @Firstname, @Lastname, @Password, @UserImage)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[User]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[User] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.User_Update",
                p => new
                    {
                        Id = p.Int(),
                        Email = p.String(),
                        Firstname = p.String(maxLength: 20),
                        Lastname = p.String(maxLength: 20),
                        Password = p.String(),
                        UserImage = p.Binary(),
                    },
                body:
                    @"UPDATE [dbo].[User]
                      SET [Email] = @Email, [Firstname] = @Firstname, [Lastname] = @Lastname, [Password] = @Password, [UserImage] = @UserImage
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.User_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[User]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.User_Delete");
            DropStoredProcedure("dbo.User_Update");
            DropStoredProcedure("dbo.User_Insert");
            DropStoredProcedure("dbo.Group_Delete");
            DropStoredProcedure("dbo.Group_Update");
            DropStoredProcedure("dbo.Group_Insert");
            DropStoredProcedure("dbo.Expense_Delete");
            DropStoredProcedure("dbo.Expense_Update");
            DropStoredProcedure("dbo.Expense_Insert");
            DropStoredProcedure("dbo.Category_Delete");
            DropStoredProcedure("dbo.Category_Update");
            DropStoredProcedure("dbo.Category_Insert");
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
