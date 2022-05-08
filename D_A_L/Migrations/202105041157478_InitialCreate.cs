namespace D_A_L.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminId = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 10, unicode: false),
                        Password = c.String(maxLength: 10, unicode: false),
                        Authority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdminId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(maxLength: 30, unicode: false),
                        MainCategory = c.Int(nullable: false),
                        Category_CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryId)
                .Index(t => t.Category_CategoryId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(maxLength: 200, unicode: false),
                        ProductCode = c.String(maxLength: 15, unicode: false),
                        Stock = c.Int(nullable: false),
                        Pprice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sprice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Boolean(nullable: false),
                        Image = c.String(maxLength: 250, unicode: false),
                        CriticalPoint = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Color = c.String(),
                        CategoryId = c.Int(nullable: false),
                        Colors_ColorId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.SubCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Colors", t => t.Colors_ColorId)
                .Index(t => t.CategoryId)
                .Index(t => t.Colors_ColorId);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        ColorId = c.Int(nullable: false, identity: true),
                        ColorName = c.String(maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.ColorId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        ClientName = c.String(maxLength: 20, unicode: false),
                        ClientSurname = c.String(maxLength: 30, unicode: false),
                        ClientAddress = c.String(maxLength: 100, unicode: false),
                        PhoneNumber = c.String(maxLength: 11, unicode: false),
                        PhoneNumber1 = c.String(maxLength: 11, unicode: false),
                        PhoneNumber2 = c.String(maxLength: 11, unicode: false),
                        ClientMail = c.String(maxLength: 50, unicode: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientId);
            
            CreateTable(
                "dbo.InvoicePs",
                c => new
                    {
                        InvoicePId = c.Int(nullable: false, identity: true),
                        PDescp = c.String(maxLength: 100, unicode: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvoicePId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.InvoiceId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClientId = c.Int(nullable: false),
                        Nu = c.String(maxLength: 10, unicode: false),
                        Description = c.String(maxLength: 240, unicode: false),
                        StaffId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.Staffs", t => t.StaffId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.StaffId);
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        StaffId = c.Int(nullable: false, identity: true),
                        StaffName = c.String(maxLength: 30, unicode: false),
                        StaffSurname = c.String(maxLength: 30, unicode: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StaffId);
            
            CreateTable(
                "dbo.Product_m",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(maxLength: 200, unicode: false),
                        ProductCode = c.String(maxLength: 15, unicode: false),
                        Stock = c.Int(nullable: false),
                        Pprice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sprice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Boolean(nullable: false),
                        Image = c.String(maxLength: 250, unicode: false),
                        Mt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CriticalPoint = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Color = c.String(),
                        CategoryId = c.Int(nullable: false),
                        Colors_ColorId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.SubCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Colors", t => t.Colors_ColorId)
                .Index(t => t.CategoryId)
                .Index(t => t.Colors_ColorId);
            
            CreateTable(
                "dbo.Product_m2",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(maxLength: 200, unicode: false),
                        ProductCode = c.String(maxLength: 15, unicode: false),
                        Stock = c.Int(nullable: false),
                        Pprice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sprice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Boolean(nullable: false),
                        Image = c.String(maxLength: 250, unicode: false),
                        mt2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tmt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Width = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Height = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CriticalPoint = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Color = c.String(),
                        CategoryId = c.Int(nullable: false),
                        Colors_ColorId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.SubCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Colors", t => t.Colors_ColorId)
                .Index(t => t.CategoryId)
                .Index(t => t.Colors_ColorId);
            
            CreateTable(
                "dbo.Product_kg",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(maxLength: 200, unicode: false),
                        ProductCode = c.String(maxLength: 15, unicode: false),
                        Stock = c.Int(nullable: false),
                        Pprice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sprice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Boolean(nullable: false),
                        Image = c.String(maxLength: 250, unicode: false),
                        Mkg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Mt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CriticalPoint = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Color = c.String(),
                        CategoryId = c.Int(nullable: false),
                        Colors_ColorId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.SubCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Colors", t => t.Colors_ColorId)
                .Index(t => t.CategoryId)
                .Index(t => t.Colors_ColorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Product_kg", "Colors_ColorId", "dbo.Colors");
            DropForeignKey("dbo.Product_kg", "CategoryId", "dbo.SubCategories");
            DropForeignKey("dbo.Product_m2", "Colors_ColorId", "dbo.Colors");
            DropForeignKey("dbo.Product_m2", "CategoryId", "dbo.SubCategories");
            DropForeignKey("dbo.Product_m", "Colors_ColorId", "dbo.Colors");
            DropForeignKey("dbo.Product_m", "CategoryId", "dbo.SubCategories");
            DropForeignKey("dbo.InvoicePs", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Invoices", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.InvoicePs", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Products", "Colors_ColorId", "dbo.Colors");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.SubCategories");
            DropForeignKey("dbo.SubCategories", "Category_CategoryId", "dbo.Categories");
            DropIndex("dbo.Product_kg", new[] { "Colors_ColorId" });
            DropIndex("dbo.Product_kg", new[] { "CategoryId" });
            DropIndex("dbo.Product_m2", new[] { "Colors_ColorId" });
            DropIndex("dbo.Product_m2", new[] { "CategoryId" });
            DropIndex("dbo.Product_m", new[] { "Colors_ColorId" });
            DropIndex("dbo.Product_m", new[] { "CategoryId" });
            DropIndex("dbo.Invoices", new[] { "StaffId" });
            DropIndex("dbo.Invoices", new[] { "ClientId" });
            DropIndex("dbo.InvoicePs", new[] { "ProductId" });
            DropIndex("dbo.InvoicePs", new[] { "InvoiceId" });
            DropIndex("dbo.Products", new[] { "Colors_ColorId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.SubCategories", new[] { "Category_CategoryId" });
            DropTable("dbo.Product_kg");
            DropTable("dbo.Product_m2");
            DropTable("dbo.Product_m");
            DropTable("dbo.Staffs");
            DropTable("dbo.Invoices");
            DropTable("dbo.InvoicePs");
            DropTable("dbo.Clients");
            DropTable("dbo.Colors");
            DropTable("dbo.Products");
            DropTable("dbo.SubCategories");
            DropTable("dbo.Categories");
            DropTable("dbo.Admins");
        }
    }
}
