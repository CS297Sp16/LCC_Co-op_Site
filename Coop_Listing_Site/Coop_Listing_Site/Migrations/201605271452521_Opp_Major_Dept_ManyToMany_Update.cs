namespace Coop_Listing_Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Opp_Major_Dept_ManyToMany_Update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Major", "Opportunity_OpportunityID", "dbo.Opportunity");
            DropIndex("dbo.Major", new[] { "Opportunity_OpportunityID" });
            CreateTable(
                "dbo.OpportunityMajor",
                c => new
                    {
                        Opportunity_OpportunityID = c.Int(nullable: false),
                        Major_MajorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Opportunity_OpportunityID, t.Major_MajorID })
                .ForeignKey("dbo.Opportunity", t => t.Opportunity_OpportunityID, cascadeDelete: true)
                .ForeignKey("dbo.Major", t => t.Major_MajorID, cascadeDelete: true)
                .Index(t => t.Opportunity_OpportunityID)
                .Index(t => t.Major_MajorID);
            
            DropColumn("dbo.Major", "Opportunity_OpportunityID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Major", "Opportunity_OpportunityID", c => c.Int());
            DropForeignKey("dbo.OpportunityMajor", "Major_MajorID", "dbo.Major");
            DropForeignKey("dbo.OpportunityMajor", "Opportunity_OpportunityID", "dbo.Opportunity");
            DropIndex("dbo.OpportunityMajor", new[] { "Major_MajorID" });
            DropIndex("dbo.OpportunityMajor", new[] { "Opportunity_OpportunityID" });
            DropTable("dbo.OpportunityMajor");
            CreateIndex("dbo.Major", "Opportunity_OpportunityID");
            AddForeignKey("dbo.Major", "Opportunity_OpportunityID", "dbo.Opportunity", "OpportunityID");
        }
    }
}
