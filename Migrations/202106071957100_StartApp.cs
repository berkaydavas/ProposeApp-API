namespace ProposeAppAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StartApp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false, maxLength: 10),
                        symbol = c.String(maxLength: 10),
                        isPrimary = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.code, unique: true);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        brand = c.String(nullable: false, maxLength: 255),
                        model = c.String(nullable: false, maxLength: 255),
                        stockCode = c.String(maxLength: 30),
                        description = c.String(),
                        pricePer = c.Int(nullable: false),
                        unit = c.String(nullable: false),
                        price = c.Double(nullable: false),
                        currencyId = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        createdDate = c.DateTime(nullable: false),
                        createdById = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.createdById, cascadeDelete: true)
                .ForeignKey("dbo.Currencies", t => t.currencyId, cascadeDelete: true)
                .Index(t => t.model, unique: true)
                .Index(t => t.stockCode, unique: true)
                .Index(t => t.currencyId)
                .Index(t => t.createdById);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.Proposes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        keyName = c.String(nullable: false, maxLength: 255),
                        description = c.String(maxLength: 1000),
                        customer = c.String(nullable: false, maxLength: 255),
                        company = c.String(maxLength: 255),
                        inCharge = c.String(maxLength: 150),
                        project = c.String(maxLength: 255),
                        startDate = c.DateTime(nullable: false),
                        currencyId = c.Int(nullable: false),
                        exrateDate = c.DateTime(nullable: false),
                        createdDate = c.DateTime(nullable: false),
                        createdById = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.createdById, cascadeDelete: true)
                .ForeignKey("dbo.Currencies", t => t.currencyId, cascadeDelete: true)
                .Index(t => t.currencyId)
                .Index(t => t.createdById);
            
            CreateTable(
                "dbo.ProposeVersions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proposeId = c.Int(nullable: false),
                        json = c.String(),
                        type = c.String(maxLength: 10),
                        reviseNumber = c.Int(nullable: false),
                        createdDate = c.DateTime(nullable: false),
                        createdById = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.createdById, cascadeDelete: true)
                .ForeignKey("dbo.Proposes", t => t.proposeId, cascadeDelete: false)
                .Index(t => t.proposeId)
                .Index(t => t.createdById);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "IdentityUser_Id", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "IdentityUser_Id", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "IdentityUser_Id", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.ProposeVersions", "proposeId", "dbo.Proposes");
            DropForeignKey("dbo.ProposeVersions", "createdById", "dbo.Users");
            DropForeignKey("dbo.Proposes", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.Proposes", "createdById", "dbo.Users");
            DropForeignKey("dbo.Products", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.Products", "createdById", "dbo.Users");
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.ProposeVersions", new[] { "createdById" });
            DropIndex("dbo.ProposeVersions", new[] { "proposeId" });
            DropIndex("dbo.Proposes", new[] { "createdById" });
            DropIndex("dbo.Proposes", new[] { "currencyId" });
            DropIndex("dbo.UserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserLogins", new[] { "IdentityUser_Id" });
            DropIndex("dbo.UserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Products", new[] { "createdById" });
            DropIndex("dbo.Products", new[] { "currencyId" });
            DropIndex("dbo.Products", new[] { "stockCode" });
            DropIndex("dbo.Products", new[] { "model" });
            DropIndex("dbo.Currencies", new[] { "code" });
            DropTable("dbo.Roles");
            DropTable("dbo.ProposeVersions");
            DropTable("dbo.Proposes");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Products");
            DropTable("dbo.Currencies");
        }
    }
}
