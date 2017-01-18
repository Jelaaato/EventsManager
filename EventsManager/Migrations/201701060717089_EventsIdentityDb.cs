namespace EventsManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventsIdentityDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventsRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.EventsUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.EventsRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.EventsUsers", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.EventsUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventsUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventsUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.EventsUserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.EventsUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventsUsers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Id", "dbo.EventsUsers");
            DropForeignKey("dbo.EventsUserRoles", "IdentityUser_Id", "dbo.EventsUsers");
            DropForeignKey("dbo.EventsUserLogin", "IdentityUser_Id", "dbo.EventsUsers");
            DropForeignKey("dbo.EventsUserClaims", "IdentityUser_Id", "dbo.EventsUsers");
            DropForeignKey("dbo.EventsUserRoles", "RoleId", "dbo.EventsRoles");
            DropIndex("dbo.AspNetUsers", new[] { "Id" });
            DropIndex("dbo.EventsUserLogin", new[] { "IdentityUser_Id" });
            DropIndex("dbo.EventsUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.EventsUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.EventsUserRoles", new[] { "RoleId" });
            DropIndex("dbo.EventsRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.EventsUserLogin");
            DropTable("dbo.EventsUserClaims");
            DropTable("dbo.EventsUsers");
            DropTable("dbo.EventsUserRoles");
            DropTable("dbo.EventsRoles");
        }
    }
}
