namespace Coop_Listing_Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Opportunity_Update1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Opportunity", "Stippened");
            DropColumn("dbo.Opportunity", "UnPaid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Opportunity", "UnPaid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Opportunity", "Stippened", c => c.Boolean(nullable: false));
        }
    }
}
