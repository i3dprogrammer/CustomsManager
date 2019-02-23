namespace CustomsManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.Deposits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Single(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Bank = c.String(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Operations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        Number = c.Int(nullable: false),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Single(nullable: false),
                        Receipt_Id = c.Int(),
                        Operation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PDFs", t => t.Receipt_Id)
                .ForeignKey("dbo.Operations", t => t.Operation_Id)
                .Index(t => t.Receipt_Id)
                .Index(t => t.Operation_Id);
            
            CreateTable(
                "dbo.PDFs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SectionData = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operations", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Sections", "Operation_Id", "dbo.Operations");
            DropForeignKey("dbo.Sections", "Receipt_Id", "dbo.PDFs");
            DropForeignKey("dbo.Deposits", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Sections", new[] { "Operation_Id" });
            DropIndex("dbo.Sections", new[] { "Receipt_Id" });
            DropIndex("dbo.Operations", new[] { "Customer_Id" });
            DropIndex("dbo.Deposits", new[] { "Customer_Id" });
            DropIndex("dbo.Customers", new[] { "Code" });
            DropTable("dbo.PDFs");
            DropTable("dbo.Sections");
            DropTable("dbo.Operations");
            DropTable("dbo.Deposits");
            DropTable("dbo.Customers");
        }
    }
}
