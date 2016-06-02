namespace Coop_Listing_Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nullable_Student_GPA : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StudentInfo", "GPA", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StudentInfo", "GPA", c => c.Double(nullable: false));
        }
    }
}
