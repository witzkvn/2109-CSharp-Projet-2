namespace WildPay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutAttributs : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Expense", "Title", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Expense", "Title", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
