namespace Coop_Listing_Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Opportunity_Update2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Opportunity", "Approved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Opportunity", "Department_DepartmentID", c => c.Int());
            AddColumn("dbo.Major", "Opportunity_OpportunityID", c => c.Int());
            AddColumn("dbo.StudentInfo", "Opportunity_OpportunityID", c => c.Int());
            AlterColumn("dbo.Opportunity", "GPA", c => c.Double());
            CreateIndex("dbo.Opportunity", "Department_DepartmentID");
            CreateIndex("dbo.Major", "Opportunity_OpportunityID");
            CreateIndex("dbo.StudentInfo", "Opportunity_OpportunityID");
            AddForeignKey("dbo.Opportunity", "Department_DepartmentID", "dbo.Department", "DepartmentID");
            AddForeignKey("dbo.Major", "Opportunity_OpportunityID", "dbo.Opportunity", "OpportunityID");
            AddForeignKey("dbo.StudentInfo", "Opportunity_OpportunityID", "dbo.Opportunity", "OpportunityID");
            DropColumn("dbo.Opportunity", "PDF");
            DropColumn("dbo.Opportunity", "UserID");
            DropColumn("dbo.Opportunity", "CompanyID");
            DropColumn("dbo.Opportunity", "DepartmentID");
            DropTable("dbo.Course");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        CourseID = c.Int(nullable: false, identity: true),
                        CourseNumber = c.String(),
                    })
                .PrimaryKey(t => t.CourseID);
            
            AddColumn("dbo.Opportunity", "DepartmentID", c => c.Int(nullable: false));
            AddColumn("dbo.Opportunity", "CompanyID", c => c.Int(nullable: false));
            AddColumn("dbo.Opportunity", "UserID", c => c.Int(nullable: false));
            AddColumn("dbo.Opportunity", "PDF", c => c.Int(nullable: false));
            DropForeignKey("dbo.StudentInfo", "Opportunity_OpportunityID", "dbo.Opportunity");
            DropForeignKey("dbo.Major", "Opportunity_OpportunityID", "dbo.Opportunity");
            DropForeignKey("dbo.Opportunity", "Department_DepartmentID", "dbo.Department");
            DropIndex("dbo.StudentInfo", new[] { "Opportunity_OpportunityID" });
            DropIndex("dbo.Major", new[] { "Opportunity_OpportunityID" });
            DropIndex("dbo.Opportunity", new[] { "Department_DepartmentID" });
            AlterColumn("dbo.Opportunity", "GPA", c => c.Double(nullable: false));
            DropColumn("dbo.StudentInfo", "Opportunity_OpportunityID");
            DropColumn("dbo.Major", "Opportunity_OpportunityID");
            DropColumn("dbo.Opportunity", "Department_DepartmentID");
            DropColumn("dbo.Opportunity", "Approved");
        }
    }
}
