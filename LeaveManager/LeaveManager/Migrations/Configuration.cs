namespace LeaveManager.Migrations
{
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
            
            var db = new LeaveManagerContext();
            removeAllTables(db);
            


            //Add data to database
           /* var db = new LeaveManagerContext();
            populateDatabase(db);*/

        }

        private void populateDatabase(LeaveManagerContext db)
        {
            Employee e1 = new Employee {employeeFirstName = "Pera", employeeLastName = "Peric", employeeEmail = "pera.peric@gmail.com" };
            Employee e2 = new Employee { employeeFirstName = "Mitar", employeeLastName = "Mitric", employeeEmail = "mitar.mitric@gmail.com" };
            Employee e3 = new Employee { employeeFirstName = "Zika", employeeLastName = "Zikic", employeeEmail = "zika.zikic@gmail.com" };
            Employee e4 = new Employee { employeeFirstName = "Marina", employeeLastName = "Maric", employeeEmail = "marina.maric@gmail.com" };
            Employee e5 = new Employee { employeeFirstName = "Maja", employeeLastName = "Majic", employeeEmail = "maja.majic@gmail.com" };
            Employee e6 = new Employee { employeeFirstName = "Nikola", employeeLastName = "Nikolic", employeeEmail = "nikola.nikolic@gmail.com" };

            db.Employees.AddOrUpdate(e1,e2,e3,e4,e5,e6);

            Role r1 = new Role {roleName = "Worker" };
            Role r2 = new Role { roleName = "Delivery Manager" };
            Role r3 = new Role { roleName = "Department Manager" };

            db.Roles.AddOrUpdate(r1, r2, r3);

            EmployeeRole er1 = new EmployeeRole { employee = e1, employeeID = e1.employeeID, role = r1, roleID = r1.roleID };
            EmployeeRole er2 = new EmployeeRole { employee = e2, employeeID = e2.employeeID, role = r1, roleID = r1.roleID };
            EmployeeRole er3 = new EmployeeRole { employee = e2, employeeID = e2.employeeID, role = r2, roleID = r2.roleID };
            EmployeeRole er4 = new EmployeeRole { employee = e3, employeeID = e3.employeeID, role = r2, roleID = r2.roleID };
            EmployeeRole er5 = new EmployeeRole { employee = e4, employeeID = e4.employeeID, role = r1, roleID = r1.roleID };
            EmployeeRole er6 = new EmployeeRole { employee = e5, employeeID = e1.employeeID, role = r1, roleID = r1.roleID };
            EmployeeRole er7 = new EmployeeRole { employee = e6, employeeID = e6.employeeID, role = r3, roleID = r3.roleID };

            db.EmployeeRoles.AddOrUpdate(er1,er2,er3,er4,er5,er6,er7);

            LeaveReason lr1 = new LeaveReason() { leaveReasonName = "Holiday" };
            LeaveReason lr2 = new LeaveReason() { leaveReasonName = "Sickness" };
            LeaveReason lr3 = new LeaveReason() { leaveReasonName = "Other reasons" };

            db.LeaveReasons.AddOrUpdate(lr1, lr2, lr3);

            RequestStatus rs1 = new RequestStatus() { requestStatusName = "Waiting For Delivery Manager Approve" };
            RequestStatus rs2 = new RequestStatus() { requestStatusName = "Waiting For Department Manager Approve" };
            RequestStatus rs3 = new RequestStatus() { requestStatusName = "Approved" };
            RequestStatus rs4 = new RequestStatus() { requestStatusName = "Declined" };

            db.RequestStatus.AddOrUpdate(rs1, rs2, rs3, rs4);

            db.SaveChanges();


        }

        private void removeAllTables(LeaveManagerContext db)
        {
            foreach (var e in db.Employees)
            {
                db.Employees.Remove(e);
            }

           /* foreach (var e in db.EmployeeLeaveRequestViewModels)
            {
                db.EmployeeLeaveRequestViewModels.Remove(e);
            }*/

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