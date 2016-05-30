namespace Coop_Listing_Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Major_Update1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Major", name: "CoordinatorInfo_CoordInfoID", newName: "Coordinator_CoordInfoID");
            RenameIndex(table: "dbo.Major", name: "IX_CoordinatorInfo_CoordInfoID", newName: "IX_Coordinator_CoordInfoID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Major", name: "IX_Coordinator_CoordInfoID", newName: "IX_CoordinatorInfo_CoordInfoID");
            RenameColumn(table: "dbo.Major", name: "Coordinator_CoordInfoID", newName: "CoordinatorInfo_CoordInfoID");
        }
    }
}
