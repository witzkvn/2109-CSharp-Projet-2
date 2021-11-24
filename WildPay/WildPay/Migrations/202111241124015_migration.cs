namespace WildPay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration : DbMigration
    {
        public override void Up()
        {
            DropStoredProcedure("dbo.Category_Insert");
            DropStoredProcedure("dbo.Category_Update");
            DropStoredProcedure("dbo.Category_Delete");
            DropStoredProcedure("dbo.Expense_Insert");
            DropStoredProcedure("dbo.Expense_Update");
            DropStoredProcedure("dbo.Expense_Delete");
            DropStoredProcedure("dbo.Group_Insert");
            DropStoredProcedure("dbo.Group_Update");
            DropStoredProcedure("dbo.Group_Delete");
            DropStoredProcedure("dbo.User_Insert");
            DropStoredProcedure("dbo.User_Update");
            DropStoredProcedure("dbo.User_Delete");
        }
        
        public override void Down()
        {
            throw new NotSupportedException("La génération de modèles automatique d'opérations de création ou de modification de procédure n'est pas prise en charge dans les méthodes descendantes.");
        }
    }
}
