namespace Coop_Listing_Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Class_Updates1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Department", "DepartmentName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Department", "DepartmentName", c => c.String());
        }
    }
}
