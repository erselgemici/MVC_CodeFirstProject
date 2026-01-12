namespace MyAcademy_MVC_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createAllTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryId" });
            CreateTable(
                "dbo.AdminLogs",
                c => new
                    {
                        LogID = c.Int(nullable: false, identity: true),
                        ActionType = c.String(),
                        Description = c.String(),
                        LogDate = c.DateTime(nullable: false),
                        IpAddress = c.String(),
                    })
                .PrimaryKey(t => t.LogID);
            
            CreateTable(
                "dbo.BlogPosts",
                c => new
                    {
                        BlogPostID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        PublishedDate = c.DateTime(nullable: false),
                        Author = c.String(),
                        ImageUrl = c.String(),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.BlogPostID);
            
            CreateTable(
                "dbo.ContactMessages",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Subject = c.String(),
                        MessageBody = c.String(),
                        SentDate = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        AiCategory = c.String(),
                        AiResponse = c.String(),
                    })
                .PrimaryKey(t => t.MessageID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false),
                        Phone = c.String(),
                        City = c.String(),
                    })
                .PrimaryKey(t => t.CustomerID);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        SaleID = c.Int(nullable: false, identity: true),
                        SaleDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PolicyID = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SaleID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.InsurancePolicies", t => t.PolicyID, cascadeDelete: true)
                .Index(t => t.PolicyID)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.InsurancePolicies",
                c => new
                    {
                        PolicyID = c.Int(nullable: false, identity: true),
                        PolicyName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImageUrl = c.String(),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PolicyID)
                .ForeignKey("dbo.InsuranceCategories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.InsuranceCategories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 100),
                        IconClass = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Faqs",
                c => new
                    {
                        FaqID = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.FaqID);
            
            CreateTable(
                "dbo.TeamMembers",
                c => new
                    {
                        TeamMemberID = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Title = c.String(),
                        ImageUrl = c.String(),
                        TwitterUrl = c.String(),
                        LinkedInUrl = c.String(),
                    })
                .PrimaryKey(t => t.TeamMemberID);
            
            CreateTable(
                "dbo.Testimonials",
                c => new
                    {
                        TestimonialID = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        Profession = c.String(),
                        Comment = c.String(),
                        ImageUrl = c.String(),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TestimonialID);
            
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Sales", "PolicyID", "dbo.InsurancePolicies");
            DropForeignKey("dbo.InsurancePolicies", "CategoryID", "dbo.InsuranceCategories");
            DropForeignKey("dbo.Sales", "CustomerID", "dbo.Customers");
            DropIndex("dbo.InsurancePolicies", new[] { "CategoryID" });
            DropIndex("dbo.Sales", new[] { "CustomerID" });
            DropIndex("dbo.Sales", new[] { "PolicyID" });
            DropTable("dbo.Testimonials");
            DropTable("dbo.TeamMembers");
            DropTable("dbo.Faqs");
            DropTable("dbo.InsuranceCategories");
            DropTable("dbo.InsurancePolicies");
            DropTable("dbo.Sales");
            DropTable("dbo.Customers");
            DropTable("dbo.ContactMessages");
            DropTable("dbo.BlogPosts");
            DropTable("dbo.AdminLogs");
            CreateIndex("dbo.Products", "CategoryId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
