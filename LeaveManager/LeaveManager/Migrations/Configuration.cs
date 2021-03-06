namespace LeaveManager.Migrations
{
    using Controllers;
    using LeaveManager.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LeaveManager.Models.LeaveManagerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "LeaveManager.Models.LeaveManagerContext";
        }

        protected override void Seed(LeaveManager.Models.LeaveManagerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            /*Employee e1 = new Employee { employeeID = 100, employeeFirstName = "Dzordz", employeeLastName = "Dzonson", employeeEmail = "dzordz.dzonson@gmail.com" };
            Employee e2 = new Employee { employeeID = 101, employeeFirstName = "Beni", employeeLastName = "Benic", employeeEmail = "beni.benic@gmail.com"};

            Role r1 = new Role { roleID = 100, roleName = "TestRole1" };
            Role r2 = new Role { roleID = 101, roleName = "TestRole2" };

            EmployeeRole er1 = new EmployeeRole {employeeRoleID = 100, employee = e1, employeeID = e1.employeeID, role = r1, roleID = r1.roleID};
            EmployeeRole er2 = new EmployeeRole { employeeRoleID = 101, employee = e1, employeeID = e1.employeeID, role = r2, roleID = r2.roleID };

            LeaveReason lr1 = new LeaveReason { leaveReasonID = 101, leaveReasonName = "sickness" };


            var db = new LeaveManagerContext();

            db.Employees.AddOrUpdate(e1, e2);
            db.Roles.AddOrUpdate(r1, r2);
            db.EmployeeRoles.AddOrUpdate(er1);
            db.SaveChanges();*/





            /*
             LeaveReason lr1 = new LeaveReason { leaveReasonName = "died one more time" };
             RequestStatus rs1 = new RequestStatus { requestStatusName = "pending" };
             var db = new LeaveManagerContext();
             db.LeaveReasons.Add(lr1);
             db.RequestStatus.Add(rs1);
             db.SaveChanges();*/


            //Remove All Tables

            //var db = new LeaveManagerContext();
            //removeAllTables(db);



            ////Add data to database
            var db = new LeaveManagerContext();
            populateDatabase(db);

            // ////Initial Mail Settings
            //////var db = new LeaveManagerContext();
            // MailSettings ms = new MailSettings { Host = "smtp.gmail.com", Port = 25, Username = "leavemanager9@gmail.com", Password = "managerleave9" };
            // db.MailSettings.AddOrUpdate(ms);
            // db.SaveChanges();

        }

        private void populateDatabase(LeaveManagerContext db)
        {
            Employee e1 = new Employee { employeeFirstName = "Pera", employeeLastName = "Peric", employeeEmail = "pera.peric@gmail.com", passwordHash = EmployeesController.getHashedPassword("pera"), CreateDate = DateTime.Now };
            Employee e2 = new Employee { employeeFirstName = "Mitar", employeeLastName = "Mitric", employeeEmail = "mitar.mitric@gmail.com", passwordHash = EmployeesController.getHashedPassword("mitar"), CreateDate = DateTime.Now };
            Employee e3 = new Employee { employeeFirstName = "Zika", employeeLastName = "Zikic", employeeEmail = "zika.zikic@gmail.com", passwordHash = EmployeesController.getHashedPassword("zika"), CreateDate = DateTime.Now };
            Employee e4 = new Employee { employeeFirstName = "Marina", employeeLastName = "Maric", employeeEmail = "marina.maric@gmail.com", passwordHash = EmployeesController.getHashedPassword("marina"), CreateDate = DateTime.Now };
            Employee e5 = new Employee { employeeFirstName = "Maja", employeeLastName = "Majic", employeeEmail = "maja.majic@gmail.com", passwordHash = EmployeesController.getHashedPassword("maja"), CreateDate = DateTime.Now };
            Employee e6 = new Employee { employeeFirstName = "Nikola", employeeLastName = "Nikolic", employeeEmail = "nikola.nikolic@gmail.com", passwordHash = EmployeesController.getHashedPassword("nikola"), CreateDate = DateTime.Now };
            Employee e7 = new Employee { employeeFirstName = "Marko", employeeLastName = "Markovic", employeeEmail = "marko.markovic@gmail.com", passwordHash = EmployeesController.getHashedPassword("marko"), CreateDate = DateTime.Now };

            db.Employees.AddOrUpdate(e1, e2, e3, e4, e5, e6, e7);

            Role r1 = new Role { roleName = "Worker", CreateDate=DateTime.Now  };
            Role r2 = new Role { roleName = "Delivery Manager", CreateDate = DateTime.Now };
            Role r3 = new Role { roleName = "Department Manager", CreateDate = DateTime.Now };
            Role r4 = new Role { roleName = "Super User", CreateDate = DateTime.Now };

            db.Roles.AddOrUpdate(r1, r2, r3, r4);

            EmployeeRole er1 = new EmployeeRole { employee = e1, employeeID = e1.employeeID, role = r1, roleID = r1.roleID, CreateDate = DateTime.Now };
            EmployeeRole er2 = new EmployeeRole { employee = e2, employeeID = e2.employeeID, role = r1, roleID = r1.roleID, CreateDate = DateTime.Now };
            EmployeeRole er3 = new EmployeeRole { employee = e2, employeeID = e2.employeeID, role = r2, roleID = r2.roleID, CreateDate = DateTime.Now };
            EmployeeRole er4 = new EmployeeRole { employee = e3, employeeID = e3.employeeID, role = r2, roleID = r2.roleID, CreateDate = DateTime.Now };
            EmployeeRole er5 = new EmployeeRole { employee = e4, employeeID = e4.employeeID, role = r1, roleID = r1.roleID, CreateDate = DateTime.Now };
            EmployeeRole er6 = new EmployeeRole { employee = e5, employeeID = e1.employeeID, role = r1, roleID = r1.roleID, CreateDate = DateTime.Now };
            EmployeeRole er7 = new EmployeeRole { employee = e6, employeeID = e6.employeeID, role = r3, roleID = r3.roleID, CreateDate = DateTime.Now };
            EmployeeRole er8 = new EmployeeRole { employee = e7, employeeID = e7.employeeID, role = r4, roleID = r4.roleID, CreateDate = DateTime.Now };

            db.EmployeeRoles.AddOrUpdate(er1, er2, er3, er4, er5, er6, er7, er8);

            LeaveReason lr1 = new LeaveReason() { leaveReasonName = "Holiday", CreateDate = DateTime.Now };
            LeaveReason lr2 = new LeaveReason() { leaveReasonName = "Sickness", CreateDate = DateTime.Now };
            LeaveReason lr3 = new LeaveReason() { leaveReasonName = "Other reasons", CreateDate = DateTime.Now };

            db.LeaveReasons.AddOrUpdate(lr1, lr2, lr3);

            RequestStatus rs1 = new RequestStatus() { requestStatusName = "Pending", CreateDate = DateTime.Now };
            RequestStatus rs2 = new RequestStatus() { requestStatusName = "Approved", CreateDate = DateTime.Now };
            RequestStatus rs3 = new RequestStatus() { requestStatusName = "Denied", CreateDate = DateTime.Now };
 

            db.RequestStatus.AddOrUpdate(rs1, rs2, rs3);

            LeaveRequest leaveRequest1 = new LeaveRequest {
                allDayEvent = true,
                deliveryManager = e3, 
               
                deliveryManagerComment = "",
                departmentManager = e6,
                deliveryManagerStatus = rs2,
                departmentManagerStatus = rs1,
                departmentManagerComment = "",
                Description = "My wifes birthday",
                employee = e1,
                endTime =new DateTime(2012,12,12),
                leaveReason =lr1,
                startTime =new DateTime(2012,12,22),
                CreateDate = DateTime.Now
            };

            LeaveRequest leaveRequest2 = new LeaveRequest
            {
                allDayEvent = true,
                deliveryManager = e3,
                deliveryManagerStatus = rs2,
                departmentManagerStatus = rs1,
                deliveryManagerComment = "",
                departmentManager = e6,

                departmentManagerComment = "",
                Description = "My wifes birthday",
                employee = e1,
                endTime = new DateTime(2012, 12, 12),
                leaveReason = lr1,
                startTime = new DateTime(2012, 12, 22),
                CreateDate = DateTime.Now
            };


            //db.LeaveRequests.AddOrUpdate(leaveRequest1,leaveRequest2);
            db.SaveChanges();


        }

        private void removeAllTables(LeaveManagerContext db)
        {
            foreach (var e in db.Employees)
            {
                db.Employees.Remove(e);
            }

           

            foreach (var e in db.EmployeeRoles)
            {
                db.EmployeeRoles.Remove(e);
            }

            foreach (var e in db.LeaveReasons)
            {
                db.LeaveReasons.Remove(e);
            }

            foreach (var e in db.LeaveRequests)
            {
                db.LeaveRequests.Remove(e);
            }

            foreach (var e in db.RequestStatus)
            {
                db.RequestStatus.Remove(e);
            }
            foreach (var e in db.Roles)
            {
                db.Roles.Remove(e);
            }


            db.SaveChanges();
        }
    }
}
