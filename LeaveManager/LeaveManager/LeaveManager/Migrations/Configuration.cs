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
            /*Employee e1 = new Employee { EmployeeID = 100, EmployeeFirstName = "Dzordz", EmployeeLastName = "Dzonson", EmployeeEmail = "dzordz.dzonson@gmail.com" };
            Employee e2 = new Employee { EmployeeID = 101, EmployeeFirstName = "Beni", EmployeeLastName = "Benic", EmployeeEmail = "beni.benic@gmail.com"};

            Role r1 = new Role { RoleID = 100, RoleName = "TestRole1" };
            Role r2 = new Role { RoleID = 101, RoleName = "TestRole2" };

            EmployeeRole er1 = new EmployeeRole {EmployeeRoleID = 100, Employee = e1, EmployeeID = e1.EmployeeID, Role = r1, RoleID = r1.RoleID};
            EmployeeRole er2 = new EmployeeRole { EmployeeRoleID = 101, Employee = e1, EmployeeID = e1.EmployeeID, Role = r2, RoleID = r2.RoleID };

            LeaveReason lr1 = new LeaveReason { leaveReasonID = 101, LeaveReasonName = "sickness" };


            var db = new LeaveManagerContext();

            db.Employees.AddOrUpdate(e1, e2);
            db.Roles.AddOrUpdate(r1, r2);
            db.EmployeeRoles.AddOrUpdate(er1);
            db.SaveChanges();*/





            /*
             LeaveReason lr1 = new LeaveReason { LeaveReasonName = "died one more time" };
             RequestStatus rs1 = new RequestStatus { RequestStatusName = "pending" };
             var db = new LeaveManagerContext();
             db.LeaveReasons.Add(lr1);
             db.RequestStatus.Add(rs1);
             db.SaveChanges();*/


            //Remove All Tables

            //var db = new LeaveManagerContext();
            //removeAllTables(db);



            //Add data to database
            //var db = new LeaveManagerContext();
            //populateDatabase(db);

            // Initial Mail Settings
            var db = new LeaveManagerContext();
            MailSettings ms = new MailSettings { Host = "smtp.gmail.com", Port = 25, Username = "leavemanager9@gmail.com", Password = "managerleave9" ,CreateDate = DateTime.Now, UpdateDate = DateTime.Now};
            db.MailSettings.AddOrUpdate(ms);
            db.SaveChanges();

        }

        private void populateDatabase(LeaveManagerContext db)
        {
            Employee e1 = new Employee { EmployeeFirstName = "Pera", EmployeeLastName = "Peric", EmployeeEmail = "pera.peric@gmail.com", PasswordHash = EmployeesController.getHashedPassword("pera"), CreateDate = DateTime.Now ,UpdateDate = DateTime.Now};
            Employee e2 = new Employee { EmployeeFirstName = "Mitar", EmployeeLastName = "Mitric", EmployeeEmail = "mitar.mitric@gmail.com", PasswordHash = EmployeesController.getHashedPassword("mitar"), CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            Employee e3 = new Employee { EmployeeFirstName = "Zika", EmployeeLastName = "Zikic", EmployeeEmail = "zika.zikic@gmail.com", PasswordHash = EmployeesController.getHashedPassword("zika"), CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            Employee e4 = new Employee { EmployeeFirstName = "Marina", EmployeeLastName = "Maric", EmployeeEmail = "marina.maric@gmail.com", PasswordHash = EmployeesController.getHashedPassword("marina"), CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            Employee e5 = new Employee { EmployeeFirstName = "Maja", EmployeeLastName = "Majic", EmployeeEmail = "maja.majic@gmail.com", PasswordHash = EmployeesController.getHashedPassword("maja"), CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            Employee e6 = new Employee { EmployeeFirstName = "Nikola", EmployeeLastName = "Nikolic", EmployeeEmail = "nikola.nikolic@gmail.com", PasswordHash = EmployeesController.getHashedPassword("nikola"), CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            Employee e7 = new Employee { EmployeeFirstName = "Marko", EmployeeLastName = "Markovic", EmployeeEmail = "marko.markovic@gmail.com", PasswordHash = EmployeesController.getHashedPassword("marko"), CreateDate = DateTime.Now, UpdateDate = DateTime.Now };

            db.Employees.AddOrUpdate(e1, e2, e3, e4, e5, e6, e7);

            Role r1 = new Role { RoleName = "Worker",CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            Role r2 = new Role { RoleName = "Delivery Manager" ,CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            Role r3 = new Role { RoleName = "Department Manager", CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            Role r4 = new Role { RoleName = "Super User" ,CreateDate = DateTime.Now, UpdateDate = DateTime.Now };

            db.Roles.AddOrUpdate(r1, r2, r3, r4);

            EmployeeRole er1 = new EmployeeRole { Employee = e1, Role = r1, CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            EmployeeRole er2 = new EmployeeRole { Employee = e2, Role = r1, CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            EmployeeRole er3 = new EmployeeRole { Employee = e2, Role = r2, CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            EmployeeRole er4 = new EmployeeRole { Employee = e3, Role = r2, CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            EmployeeRole er5 = new EmployeeRole { Employee = e4, Role = r1, CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            EmployeeRole er6 = new EmployeeRole { Employee = e5, Role = r1, CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            EmployeeRole er7 = new EmployeeRole { Employee = e6, Role = r3, CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            EmployeeRole er8 = new EmployeeRole { Employee = e7, Role = r4, CreateDate = DateTime.Now, UpdateDate = DateTime.Now };

            db.EmployeeRoles.AddOrUpdate(er1, er2, er3, er4, er5, er6, er7, er8);

            LeaveReason lr1 = new LeaveReason() { LeaveReasonName = "Holiday", CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            LeaveReason lr2 = new LeaveReason() { LeaveReasonName = "Sickness",CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            LeaveReason lr3 = new LeaveReason() { LeaveReasonName = "Other reasons", CreateDate = DateTime.Now, UpdateDate = DateTime.Now };

            db.LeaveReasons.AddOrUpdate(lr1, lr2, lr3);

            RequestStatus rs1 = new RequestStatus() { RequestStatusName = "Pending", CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            RequestStatus rs2 = new RequestStatus() { RequestStatusName = "Approved",CreateDate = DateTime.Now, UpdateDate = DateTime.Now };
            RequestStatus rs3 = new RequestStatus() { RequestStatusName = "Denied" ,CreateDate = DateTime.Now, UpdateDate = DateTime.Now };

            db.RequestStatus.AddOrUpdate(rs1, rs2, rs3);

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
