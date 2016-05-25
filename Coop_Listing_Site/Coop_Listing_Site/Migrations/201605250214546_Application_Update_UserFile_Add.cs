namespace Coop_Listing_Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Application_Update_UserFile_Add : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Application", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Application", new[] { "User_Id" });
            CreateTable(
                "dbo.UserFile",
                c => new
                    {
                        UserFileID = c.Int(nullable: false, identity: true),
                        ContentType = c.String(),
                        FileName = c.String(),
                        FileData = c.Binary(),
                        Application_ApplicationId = c.Int(),
                    })
                .PrimaryKey(t => t.UserFileID)
                .ForeignKey("dbo.Application", t => t.Application_ApplicationId)
                .Index(t => t.Application_ApplicationId);
            
            AddColumn("dbo.Application", "Student_LNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.Application", "Student_LNumber");
            AddForeignKey("dbo.Application", "Student_LNumber", "dbo.StudentInfo", "LNumber");
            DropColumn("dbo.Application", "Resume");
            DropColumn("dbo.Application", "CoverLetter");
            DropColumn("dbo.Application", "DriverLicense");
            DropColumn("dbo.Application", "Other");
            DropColumn("dbo.Application", "FileName_Resume");
            DropColumn("dbo.Application", "FileName_CoverLetter");
            DropColumn("dbo.Application", "FileName_DriverLicense");
            DropColumn("dbo.Application", "FileName_Other");
            DropColumn("dbo.Application", "Resume_ContentType");
            DropColumn("dbo.Application", "CoverLetter_ContentType");
            DropColumn("dbo.Application", "DriverLicense_ContentType");
            DropColumn("dbo.Application", "Other_ContentType");
            DropColumn("dbo.Application", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Application", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Application", "Other_ContentType", c => c.String());
            AddColumn("dbo.Application", "DriverLicense_ContentType", c => c.String());
            AddColumn("dbo.Application", "CoverLetter_ContentType", c => c.String());
            AddColumn("dbo.Application", "Resume_ContentType", c => c.String());
            AddColumn("dbo.Application", "FileName_Other", c => c.String());
            AddColumn("dbo.Application", "FileName_DriverLicense", c => c.String());
            AddColumn("dbo.Application", "FileName_CoverLetter", c => c.String());
            AddColumn("dbo.Application", "FileName_Resume", c => c.String());
            AddColumn("dbo.Application", "Other", c => c.Binary());
            AddColumn("dbo.Application", "DriverLicense", c => c.Binary());
            AddColumn("dbo.Application", "CoverLetter", c => c.Binary());
            AddColumn("dbo.Application", "Resume", c => c.Binary());
            DropForeignKey("dbo.Application", "Student_LNumber", "dbo.StudentInfo");
            DropForeignKey("dbo.UserFile", "Application_ApplicationId", "dbo.Application");
            DropIndex("dbo.UserFile", new[] { "Application_ApplicationId" });
            DropIndex("dbo.Application", new[] { "Student_LNumber" });
            DropColumn("dbo.Application", "Student_LNumber");
            DropTable("dbo.UserFile");
            CreateIndex("dbo.Application", "User_Id");
            AddForeignKey("dbo.Application", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
