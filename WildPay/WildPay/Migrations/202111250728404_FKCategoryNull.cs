namespace WildPay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKCategoryNull : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Expense", "FkCategoryId", "dbo.Category");
            DropIndex("dbo.Expense", new[] { "FkCategoryId" });
            AlterColumn("dbo.Expense", "FkCategoryId", c => c.Int());
            CreateIndex("dbo.Expense", "FkCategoryId");
            AddForeignKey("dbo.Expense", "FkCategoryId", "dbo.Category", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expense", "FkCategoryId", "dbo.Category");
            DropIndex("dbo.Expense", new[] { "FkCategoryId" });
            AlterColumn("dbo.Expense", "FkCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Expense", "FkCategoryId");
            AddForeignKey("dbo.Expense", "FkCategoryId", "dbo.Category", "Id", cascadeDelete: true);
        }
    }
}
