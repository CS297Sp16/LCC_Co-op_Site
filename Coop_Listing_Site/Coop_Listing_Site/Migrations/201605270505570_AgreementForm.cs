namespace Coop_Listing_Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgreementForm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgreementForm",
                c => new
                    {
                        AgreementFormID = c.Int(nullable: false, identity: true),
                        Term = c.Int(nullable: false),
                        TodaysDate = c.DateTime(nullable: false),
                        SubjectNumber = c.String(),
                        CRN = c.Int(nullable: false),
                        StudentName = c.String(),
                        LNumber = c.String(),
                        StudentPhone = c.String(),
                        StudentAddress = c.String(),
                        StudentCity = c.String(),
                        StudentState = c.String(),
                        StudentZipCode = c.Int(nullable: false),
                        StudentZipCodeExtention = c.Int(),
                        StudentEmail = c.String(),
                        Credits = c.Int(nullable: false),
                        ClockHours = c.Int(nullable: false),
                        CompanyName = c.String(),
                        Supervisor = c.String(),
                        CompanyEmail = c.String(),
                        CompanyAddress = c.String(),
                        CompanyCity = c.String(),
                        CompanyState = c.String(),
                        CompanyZipCode = c.Int(nullable: false),
                        CompanyZipCodeExtention = c.Int(),
                        CompanyPhone = c.String(),
                        CompanyFax = c.String(),
                        WorkAssignmentDutyDescription = c.String(),
                        Wage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WageRate = c.String(),
                        PaidPosition = c.Boolean(nullable: false),
                        WorkCompProv = c.Int(nullable: false),
                        SpecialNotes = c.String(),
                        WorkSiteSupervisorSignature = c.String(),
                        CoopCoordinatorSignature = c.String(),
                        StudentSignature = c.String(),
                        Coordinator_CoordInfoID = c.Int(),
                        Major_MajorID = c.Int(),
                        WeeklySchedule_WeeklyScheduleID = c.Int(),
                    })
                .PrimaryKey(t => t.AgreementFormID)
                .ForeignKey("dbo.CoordinatorInfo", t => t.Coordinator_CoordInfoID)
                .ForeignKey("dbo.Major", t => t.Major_MajorID)
                .ForeignKey("dbo.WeeklySchedule", t => t.WeeklySchedule_WeeklyScheduleID)
                .Index(t => t.Coordinator_CoordInfoID)
                .Index(t => t.Major_MajorID)
                .Index(t => t.WeeklySchedule_WeeklyScheduleID);
            
            CreateTable(
                "dbo.WeeklySchedule",
                c => new
                    {
                        WeeklyScheduleID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.WeeklyScheduleID);
            
            CreateTable(
                "dbo.WorkDay",
                c => new
                    {
                        WorkDayID = c.Int(nullable: false, identity: true),
                        WeekDay = c.Int(nullable: false),
                        Arrival = c.Time(nullable: false, precision: 7),
                        Departure = c.Time(nullable: false, precision: 7),
                        WeeklySchedule_WeeklyScheduleID = c.Int(),
                    })
                .PrimaryKey(t => t.WorkDayID)
                .ForeignKey("dbo.WeeklySchedule", t => t.WeeklySchedule_WeeklyScheduleID)
                .Index(t => t.WeeklySchedule_WeeklyScheduleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AgreementForm", "WeeklySchedule_WeeklyScheduleID", "dbo.WeeklySchedule");
            DropForeignKey("dbo.WorkDay", "WeeklySchedule_WeeklyScheduleID", "dbo.WeeklySchedule");
            DropForeignKey("dbo.AgreementForm", "Major_MajorID", "dbo.Major");
            DropForeignKey("dbo.AgreementForm", "Coordinator_CoordInfoID", "dbo.CoordinatorInfo");
            DropIndex("dbo.WorkDay", new[] { "WeeklySchedule_WeeklyScheduleID" });
            DropIndex("dbo.AgreementForm", new[] { "WeeklySchedule_WeeklyScheduleID" });
            DropIndex("dbo.AgreementForm", new[] { "Major_MajorID" });
            DropIndex("dbo.AgreementForm", new[] { "Coordinator_CoordInfoID" });
            DropTable("dbo.WorkDay");
            DropTable("dbo.WeeklySchedule");
            DropTable("dbo.AgreementForm");
        }
    }
}
