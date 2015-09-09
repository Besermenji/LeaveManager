namespace LeaveManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeliveryManagerLeaveRequestViewModels",
                c => new
                    {
                        DeliveryManagerLeaveRequestViewModelID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        allDayEvent = c.Boolean(nullable: false),
                        startTime = c.DateTime(nullable: false),
                        endTime = c.DateTime(nullable: false),
                        LeaveReasonID = c.Int(nullable: false),
                        Description = c.String(),
                        DeliveryManagerID = c.Int(nullable: false),
                        deliveryManagerStatusID = c.Int(nullable: false),
                        deliveryManagerComment = c.String(),
                        LeaveRequestId = c.Int(nullable: false),
                        deliveryManager_employeeID = c.Int(),
                        deliveryManagerStatus_requestStatusID = c.Int(),
                        employee_employeeID = c.Int(),
                    })
                .PrimaryKey(t => t.DeliveryManagerLeaveRequestViewModelID)
                .ForeignKey("dbo.Employees", t => t.deliveryManager_employeeID)
                .ForeignKey("dbo.RequestStatus", t => t.deliveryManagerStatus_requestStatusID)
                .ForeignKey("dbo.Employees", t => t.employee_employeeID)
                .ForeignKey("dbo.LeaveReasons", t => t.LeaveReasonID, cascadeDelete: true)
                .Index(t => t.LeaveReasonID)
                .Index(t => t.deliveryManager_employeeID)
                .Index(t => t.deliveryManagerStatus_requestStatusID)
                .Index(t => t.employee_employeeID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        employeeID = c.Int(nullable: false, identity: true),
                        employeeFirstName = c.String(),
                        employeeLastName = c.String(),
                        employeeEmail = c.String(),
                    })
                .PrimaryKey(t => t.employeeID);
            
            CreateTable(
                "dbo.RequestStatus",
                c => new
                    {
                        requestStatusID = c.Int(nullable: false, identity: true),
                        requestStatusName = c.String(),
                    })
                .PrimaryKey(t => t.requestStatusID);
            
            CreateTable(
                "dbo.LeaveReasons",
                c => new
                    {
                        leaveReasonID = c.Int(nullable: false, identity: true),
                        leaveReasonName = c.String(),
                    })
                .PrimaryKey(t => t.leaveReasonID);
            
            CreateTable(
                "dbo.EmployeeRoles",
                c => new
                    {
                        employeeRoleID = c.Int(nullable: false, identity: true),
                        roleID = c.Int(nullable: false),
                        employeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.employeeRoleID)
                .ForeignKey("dbo.Employees", t => t.employeeID, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.roleID, cascadeDelete: true)
                .Index(t => t.roleID)
                .Index(t => t.employeeID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        roleID = c.Int(nullable: false, identity: true),
                        roleName = c.String(),
                    })
                .PrimaryKey(t => t.roleID);
            
            CreateTable(
                "dbo.LeaveRequests",
                c => new
                    {
                        leaveRequestID = c.Int(nullable: false, identity: true),
                        allDayEvent = c.Boolean(nullable: false),
                        startTime = c.DateTime(nullable: false),
                        endTime = c.DateTime(nullable: false),
                        Description = c.String(),
                        deliveryManagerComment = c.String(),
                        departmentManagerComment = c.String(),
                        deliveryManager_employeeID = c.Int(),
                        deliveryManagerStatus_requestStatusID = c.Int(),
                        departmentManager_employeeID = c.Int(),
                        departmentManagerStatus_requestStatusID = c.Int(),
                        employee_employeeID = c.Int(),
                        leaveReason_leaveReasonID = c.Int(),
                    })
                .PrimaryKey(t => t.leaveRequestID)
                .ForeignKey("dbo.Employees", t => t.deliveryManager_employeeID)
                .ForeignKey("dbo.RequestStatus", t => t.deliveryManagerStatus_requestStatusID)
                .ForeignKey("dbo.Employees", t => t.departmentManager_employeeID)
                .ForeignKey("dbo.RequestStatus", t => t.departmentManagerStatus_requestStatusID)
                .ForeignKey("dbo.Employees", t => t.employee_employeeID)
                .ForeignKey("dbo.LeaveReasons", t => t.leaveReason_leaveReasonID)
                .Index(t => t.deliveryManager_employeeID)
                .Index(t => t.deliveryManagerStatus_requestStatusID)
                .Index(t => t.departmentManager_employeeID)
                .Index(t => t.departmentManagerStatus_requestStatusID)
                .Index(t => t.employee_employeeID)
                .Index(t => t.leaveReason_leaveReasonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeaveRequests", "leaveReason_leaveReasonID", "dbo.LeaveReasons");
            DropForeignKey("dbo.LeaveRequests", "employee_employeeID", "dbo.Employees");
            DropForeignKey("dbo.LeaveRequests", "departmentManagerStatus_requestStatusID", "dbo.RequestStatus");
            DropForeignKey("dbo.LeaveRequests", "departmentManager_employeeID", "dbo.Employees");
            DropForeignKey("dbo.LeaveRequests", "deliveryManagerStatus_requestStatusID", "dbo.RequestStatus");
            DropForeignKey("dbo.LeaveRequests", "deliveryManager_employeeID", "dbo.Employees");
            DropForeignKey("dbo.EmployeeRoles", "roleID", "dbo.Roles");
            DropForeignKey("dbo.EmployeeRoles", "employeeID", "dbo.Employees");
            DropForeignKey("dbo.DeliveryManagerLeaveRequestViewModels", "LeaveReasonID", "dbo.LeaveReasons");
            DropForeignKey("dbo.DeliveryManagerLeaveRequestViewModels", "employee_employeeID", "dbo.Employees");
            DropForeignKey("dbo.DeliveryManagerLeaveRequestViewModels", "deliveryManagerStatus_requestStatusID", "dbo.RequestStatus");
            DropForeignKey("dbo.DeliveryManagerLeaveRequestViewModels", "deliveryManager_employeeID", "dbo.Employees");
            DropIndex("dbo.LeaveRequests", new[] { "leaveReason_leaveReasonID" });
            DropIndex("dbo.LeaveRequests", new[] { "employee_employeeID" });
            DropIndex("dbo.LeaveRequests", new[] { "departmentManagerStatus_requestStatusID" });
            DropIndex("dbo.LeaveRequests", new[] { "departmentManager_employeeID" });
            DropIndex("dbo.LeaveRequests", new[] { "deliveryManagerStatus_requestStatusID" });
            DropIndex("dbo.LeaveRequests", new[] { "deliveryManager_employeeID" });
            DropIndex("dbo.EmployeeRoles", new[] { "employeeID" });
            DropIndex("dbo.EmployeeRoles", new[] { "roleID" });
            DropIndex("dbo.DeliveryManagerLeaveRequestViewModels", new[] { "employee_employeeID" });
            DropIndex("dbo.DeliveryManagerLeaveRequestViewModels", new[] { "deliveryManagerStatus_requestStatusID" });
            DropIndex("dbo.DeliveryManagerLeaveRequestViewModels", new[] { "deliveryManager_employeeID" });
            DropIndex("dbo.DeliveryManagerLeaveRequestViewModels", new[] { "LeaveReasonID" });
            DropTable("dbo.LeaveRequests");
            DropTable("dbo.Roles");
            DropTable("dbo.EmployeeRoles");
            DropTable("dbo.LeaveReasons");
            DropTable("dbo.RequestStatus");
            DropTable("dbo.Employees");
            DropTable("dbo.DeliveryManagerLeaveRequestViewModels");
        }
    }
}
