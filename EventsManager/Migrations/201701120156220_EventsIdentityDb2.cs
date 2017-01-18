namespace EventsManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventsIdentityDb2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventsRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventsRoles", "Discriminator");
        }
    }
}
