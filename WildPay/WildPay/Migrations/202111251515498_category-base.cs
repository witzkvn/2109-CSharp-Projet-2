namespace WildPay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categorybase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "IsBase", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Category", "IsBase");
        }
    }
}
