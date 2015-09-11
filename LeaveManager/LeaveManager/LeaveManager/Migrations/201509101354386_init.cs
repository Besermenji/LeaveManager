namespace LeaveManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.EmployeeRoles",
            //    c => new
            //        {
            //            EmployeeRoleID = c.Int(nullable: false, identity: true),
            //            CreateDate = c.DateTime(nullable: false),
            //            UpdateTime = c.DateTime(nullable: false),
            //            Employee_EmployeeID = c.Int(),
            //            Role_RoleID = c.Int(),
            //        })
            //    .PrimaryKey(t => t.EmployeeRoleID)
            //    .ForeignKey("dbo.Employees", t => t.Employee_EmployeeID)
            //    .ForeignKey("dbo.Roles", t => t.Role_RoleID)
            //    .Index(t => t.Employee_EmployeeID)
            //    .Index(t => t.Role_RoleID);

            //CreateTable(
            //    "dbo.Employees",
            //    c => new
            //        {
            //            EmployeeID = c.Int(nullable: false, identity: true),
            //            EmployeeFirstName = c.String(nullable: false),
            //            EmployeeLastName = c.String(nullable: false),
            //            EmployeeEmail = c.String(nullable: false),
            //            PasswordHash = c.String(),
            //            CreateDate = c.DateTime(nullable: false),
            //            UpdateDate = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.EmployeeID);

            //CreateTable(
            //    "dbo.Roles",
            //    c => new
            //        {
            //            RoleID = c.Int(nullable: false, identity: true),
            //            RoleName = c.String(),
            //            CreateDate = c.DateTime(nullable: false),
            //            UpdateTime = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.RoleID);

            //CreateTable(
            //    "dbo.LeaveReasons",
            //    c => new
            //        {
            //            LeaveReasonID = c.Int(nullable: false, identity: true),
            //            LeaveReasonName = c.String(),
            //            CreateDate = c.DateTime(nullable: false),
            //            UpdateTime = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.LeaveReasonID);

            //CreateTable(
            //    "dbo.LeaveRequestDescriptions",
            //    c => new
            //        {
            //            LeaveRequestDescriptionID = c.Int(nullable: false, identity: true),
            //            AllDayEvent = c.Boolean(nullable: false),
            //            StartTime = c.DateTime(nullable: false),
            //            EndTime = c.DateTime(nullable: false),
            //            Description = c.String(),
            //            DeliveryManagerComment = c.String(),
            //            DepartmentManagerComment = c.String(),
            //            CreateDate = c.DateTime(nullable: false),
            //            UpdateDate = c.DateTime(nullable: false),
            //            DeliveryManager_EmployeeID = c.Int(),
            //            DeliveryManagerStatus_RequestStatusID = c.Int(),
            //            DepartmentManager_EmployeeID = c.Int(),
            //            DepartmentManagerStatus_RequestStatusID = c.Int(),
            //            LeaveReason_LeaveReasonID = c.Int(),
            //            LeaveRequestInfo_LeaveRequestInfoID = c.Int(),
            //        })
            //    .PrimaryKey(t => t.LeaveRequestDescriptionID)
            //    .ForeignKey("dbo.Employees", t => t.DeliveryManager_EmployeeID)
            //    .ForeignKey("dbo.RequestStatus", t => t.DeliveryManagerStatus_RequestStatusID)
            //    .ForeignKey("dbo.Employees", t => t.DepartmentManager_EmployeeID)
            //    .ForeignKey("dbo.RequestStatus", t => t.DepartmentManagerStatus_RequestStatusID)
            //    .ForeignKey("dbo.LeaveReasons", t => t.LeaveReason_LeaveReasonID)
            //    .ForeignKey("dbo.LeaveRequestInfoes", t => t.LeaveRequestInfo_LeaveRequestInfoID)
            //    .Index(t => t.DeliveryManager_EmployeeID)
            //    .Index(t => t.DeliveryManagerStatus_RequestStatusID)
            //    .Index(t => t.DepartmentManager_EmployeeID)
            //    .Index(t => t.DepartmentManagerStatus_RequestStatusID)
            //    .Index(t => t.LeaveReason_LeaveReasonID)
            //    .Index(t => t.LeaveRequestInfo_LeaveRequestInfoID);

            //CreateTable(
            //    "dbo.RequestStatus",
            //    c => new
            //        {
            //            RequestStatusID = c.Int(nullable: false, identity: true),
            //            RequestStatusName = c.String(),
            //            CreateDate = c.DateTime(nullable: false),
            //            UpdateTime = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.RequestStatusID);

            //CreateTable(
            //    "dbo.LeaveRequestInfoes",
            //    c => new
            //        {
            //            LeaveRequestInfoID = c.Int(nullable: false, identity: true),
            //            CreateDate = c.DateTime(nullable: false),
            //            UpdateDate = c.DateTime(nullable: false),
            //            Employee_EmployeeID = c.Int(),
            //        })
            //    .PrimaryKey(t => t.LeaveRequestInfoID)
            //    .ForeignKey("dbo.Employees", t => t.Employee_EmployeeID)
            //    .Index(t => t.Employee_EmployeeID);

            //CreateTable(
            //    "dbo.MailSettings",
            //    c => new
            //    {
            //        MailSettingsID = c.Int(nullable: false, identity: true),
            //        Host = c.String(nullable: false),
            //        Port = c.Int(nullable: false),
            //        Username = c.String(nullable: false),
            //        Password = c.String(nullable: false),
            //        CreateDate = c.DateTime(nullable: false),
            //        UpdateDate = c.DateTime(nullable: false),
            //    })
            //    .PrimaryKey(t => t.MailSettingsID);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeaveRequestDescriptions", "LeaveRequestInfo_LeaveRequestInfoID", "dbo.LeaveRequestInfoes");
            DropForeignKey("dbo.LeaveRequestInfoes", "Employee_EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.LeaveRequestDescriptions", "LeaveReason_LeaveReasonID", "dbo.LeaveReasons");
            DropForeignKey("dbo.LeaveRequestDescriptions", "DepartmentManagerStatus_RequestStatusID", "dbo.RequestStatus");
            DropForeignKey("dbo.LeaveRequestDescriptions", "DepartmentManager_EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.LeaveRequestDescriptions", "DeliveryManagerStatus_RequestStatusID", "dbo.RequestStatus");
            DropForeignKey("dbo.LeaveRequestDescriptions", "DeliveryManager_EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.EmployeeRoles", "Role_RoleID", "dbo.Roles");
            DropForeignKey("dbo.EmployeeRoles", "Employee_EmployeeID", "dbo.Employees");
            DropIndex("dbo.LeaveRequestInfoes", new[] { "Employee_EmployeeID" });
            DropIndex("dbo.LeaveRequestDescriptions", new[] { "LeaveRequestInfo_LeaveRequestInfoID" });
            DropIndex("dbo.LeaveRequestDescriptions", new[] { "LeaveReason_LeaveReasonID" });
            DropIndex("dbo.LeaveRequestDescriptions", new[] { "DepartmentManagerStatus_RequestStatusID" });
            DropIndex("dbo.LeaveRequestDescriptions", new[] { "DepartmentManager_EmployeeID" });
            DropIndex("dbo.LeaveRequestDescriptions", new[] { "DeliveryManagerStatus_RequestStatusID" });
            DropIndex("dbo.LeaveRequestDescriptions", new[] { "DeliveryManager_EmployeeID" });
            DropIndex("dbo.EmployeeRoles", new[] { "Role_RoleID" });
            DropIndex("dbo.EmployeeRoles", new[] { "Employee_EmployeeID" });
            DropTable("dbo.MailSettings");
            DropTable("dbo.LeaveRequestInfoes");
            DropTable("dbo.RequestStatus");
            DropTable("dbo.LeaveRequestDescriptions");
            DropTable("dbo.LeaveReasons");
            DropTable("dbo.Roles");
            DropTable("dbo.Employees");
            DropTable("dbo.EmployeeRoles");
        }
    }
}
