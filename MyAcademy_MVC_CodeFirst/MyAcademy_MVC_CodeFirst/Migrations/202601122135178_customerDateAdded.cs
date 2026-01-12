namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customerDateAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "CreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "CreatedAt");
        }
    }
}
