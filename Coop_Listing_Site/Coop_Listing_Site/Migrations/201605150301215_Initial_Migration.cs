namespace Coop_Listing_Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Application",
                c => new
                    {
                        ApplicationId = c.Int(nullable: false, identity: true),
                        Resume = c.Binary(),
                        CoverLetter = c.Binary(),
                        DriverLicense = c.Binary(),
                        Other = c.Binary(),
                        Message = c.String(nullable: false),
                        FileName_Resume = c.String(),
                        FileName_CoverLetter = c.String(),
                        FileName_DriverLicense = c.String(),
                        FileName_Other = c.String(),
                        Resume_ContentType = c.String(),
                        CoverLetter_ContentType = c.String(),
                        DriverLicense_ContentType = c.String(),
                        Other_ContentType = c.String(),
                        Opportunity_OpportunityID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ApplicationId)
                .ForeignKey("dbo.Opportunity", t => t.Opportunity_OpportunityID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Opportunity_OpportunityID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Opportunity",
                c => new
                    {
                        OpportunityID = c.Int(nullable: false, identity: true),
                        PDF = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        CompanyID = c.Int(nullable: false),
                        CompanyName = c.String(),
                        ContactName = c.String(),
                        ContactNumber = c.String(),
                        ContactEmail = c.String(),
                        Location = c.String(),
                        CompanyWebsite = c.String(),
                        AboutCompany = c.String(),
                        AboutDepartment = c.String(),
                        CoopPositionTitle = c.String(),
                        CoopPositionDuties = c.String(),
                        Qualifications = c.String(),
                        GPA = c.Double(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        Wage = c.String(),
                        Stippened = c.Boolean(nullable: false),
                        Amount = c.String(),
                        UnPaid = c.Boolean(nullable: false),
                        Duration = c.String(),
                        OpeningsAvailable = c.Int(nullable: false),
                        TermAvailable = c.String(),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OpportunityID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(maxLength: 256),
                        Enabled = c.Boolean(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.CoordinatorInfo",
                c => new
                    {
                        CoordInfoID = c.Int(nullable: false, identity: true),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CoordInfoID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Major",
                c => new
                    {
                        MajorID = c.Int(nullable: false, identity: true),
                        MajorName = c.String(),
                        Department_DepartmentID = c.Int(),
                        CoordinatorInfo_CoordInfoID = c.Int(),
                    })
                .PrimaryKey(t => t.MajorID)
                .ForeignKey("dbo.Department", t => t.Department_DepartmentID)
                .ForeignKey("dbo.CoordinatorInfo", t => t.CoordinatorInfo_CoordInfoID)
                .Index(t => t.Department_DepartmentID)
                .Index(t => t.CoordinatorInfo_CoordInfoID);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        CourseID = c.Int(nullable: false, identity: true),
                        CourseNumber = c.String(),
                    })
                .PrimaryKey(t => t.CourseID);
            
            CreateTable(
                "dbo.EmailInfo",
                c => new
                    {
                        EmailInfoID = c.Int(nullable: false, identity: true),
                        SMTPAccountName = c.String(),
                        SMTPPassword = c.String(),
                        SMTPAddress = c.String(),
                        Domain = c.String(),
                    })
                .PrimaryKey(t => t.EmailInfoID);
            
            CreateTable(
                "dbo.RegisterInvite",
                c => new
                    {
                        RegisterInviteID = c.String(nullable: false, maxLength: 128),
                        Email = c.String(nullable: false),
                        UserType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RegisterInviteID);
            
            CreateTable(
                "dbo.PasswordReset",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.StudentInfo",
                c => new
                    {
                        LNumber = c.String(nullable: false, maxLength: 128),
                        GPA = c.Double(nullable: false),
                        Major_MajorID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.LNumber)
                .ForeignKey("dbo.Major", t => t.Major_MajorID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Major_MajorID)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentInfo", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentInfo", "Major_MajorID", "dbo.Major");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CoordinatorInfo", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Major", "CoordinatorInfo_CoordInfoID", "dbo.CoordinatorInfo");
            DropForeignKey("dbo.Major", "Department_DepartmentID", "dbo.Department");
            DropForeignKey("dbo.Application", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Application", "Opportunity_OpportunityID", "dbo.Opportunity");
            DropIndex("dbo.StudentInfo", new[] { "User_Id" });
            DropIndex("dbo.StudentInfo", new[] { "Major_MajorID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Major", new[] { "CoordinatorInfo_CoordInfoID" });
            DropIndex("dbo.Major", new[] { "Department_DepartmentID" });
            DropIndex("dbo.CoordinatorInfo", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Application", new[] { "User_Id" });
            DropIndex("dbo.Application", new[] { "Opportunity_OpportunityID" });
            DropTable("dbo.StudentInfo");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PasswordReset");
            DropTable("dbo.RegisterInvite");
            DropTable("dbo.EmailInfo");
            DropTable("dbo.Course");
            DropTable("dbo.Department");
            DropTable("dbo.Major");
            DropTable("dbo.CoordinatorInfo");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Opportunity");
            DropTable("dbo.Application");
        }
    }
}
