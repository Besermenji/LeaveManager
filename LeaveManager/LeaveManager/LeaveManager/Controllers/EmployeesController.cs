using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LeaveManager.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.IO;

namespace LeaveManager.Controllers
{
    public class EmployeesController : Controller
    {
        private LeaveManagerContext db = new LeaveManagerContext();

        // GET: Employees
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            List<String> roles = new List<string>();
            foreach (Role r in db.Roles) {
                roles.Add(r.roleName);
            }
            ViewBag.Roles = roles;        
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "employeeID,employeeFirstName,employeeLastName,employeeEmail")] Employee employee)
        {

             
            foreach (Role r in db.Roles)
            {
                string[] tmp = r.roleName.Split(' ');
                string key = "";
                foreach (string x in tmp) {
                    key += x;
                }
                bool exist = Convert.ToBoolean(Request.Form[key].Split(',')[0]);
                if (exist) {
                    
                    db.EmployeeRoles.Add(new EmployeeRole {employee = employee, employeeID = employee.employeeID, role = r, roleID = r.roleID,CreateDate = DateTime.Now });
                }
                
            }


            if (ModelState.IsValid)
            {
                string password = Membership.GeneratePassword(10, 3);
                employee.CreateDate = DateTime.Now;
                employee.passwordHash = getHashedPassword(password);

                db.Employees.Add(employee);
                db.SaveChanges();

                sendMailToNewEmployee(employee.employeeID, password);

                return RedirectToAction("Index");
            }

            return View(employee);
        }

        public static string getHashedPassword(string password) {
            var data = Encoding.ASCII.GetBytes(password);
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(data);
            string hashedPass = Encoding.UTF8.GetString(md5data);
            return hashedPass;
        }

        public void sendMailToNewEmployee(int employeeID, string password)
        {
            Employee employee = db.Employees.Single(e => e.employeeID == employeeID);
            string email = employee.employeeEmail;
            string subject = "Registration";
            string employeeHTMLBody = getHTMLEmailForNewEmployee(password,employee.employeeName);
            EmployeeLeaveRequestViewModelsController.sendMailUsingDBSettings(db, email, subject, employeeHTMLBody);
        }

        private string getHTMLEmailForNewEmployee(string password,string name)
        {
            StreamReader reader = new StreamReader(Server.MapPath("~/Content/MailTemplate.html"));
            string email = reader.ReadToEnd();

            string notHeader = "Welcome to Levi9";
            string textHeader = "Registration to Leave Manager";
            string textMessage = "Hello <i style='color:green'>" +name+ ",</i> You have been registered by our administrator, your initial password is: <b style='color: #205478'>" + password+"</b><br/>You can change your password anytime, click on the button below to login on Leave Manager Application";
            string buttonText = "Go To Leave Manager";
            string buttonLink = "http://localhost:9877/Login/Index/";

            email = email.Replace("[NotificationHeader]", notHeader);
            email = email.Replace("[TextHeader]", textHeader);
            email = email.Replace("[TextMessage]", textMessage);
            email = email.Replace("[ButtonText]", buttonText);
            email = email.Replace("[ButtonLink]", buttonLink);

            return email;
        }


        public ActionResult AccountSettings()
        {
            Employee emp = (Employee)Session["user"];
            AccountSettingsViewModel settings = new AccountSettingsViewModel()
            {
                employeeID = emp.employeeID,
                employeeFirstName = emp.employeeFirstName,
                employeeLastName = emp.employeeLastName,
                employeeEmail = emp.employeeEmail
            };


            return View(settings);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AccountSettings([Bind(Include = "employeeID,employeeEmail,oldPassword,newPassword,confirmPassword")] AccountSettingsViewModel accountSettings)
        {
            Employee emp = db.Employees.Single(e => e.employeeID == accountSettings.employeeID);

            if (ModelState.IsValid && getHashedPassword(accountSettings.oldPassword).Equals(emp.passwordHash) && accountSettings.newPassword.Equals(accountSettings.confirmPassword))
            {

                db.Employees.Single(e => e.employeeID == accountSettings.employeeID).employeeEmail = accountSettings.employeeEmail;
                db.Employees.Single(e => e.employeeID == accountSettings.employeeID).passwordHash = getHashedPassword(accountSettings.newPassword);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else {

                return View();
            }


           
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            Dictionary<string, bool> rolesEdit = new Dictionary<string, bool>();

            var roles = db.EmployeeRoles.Where(x => x.employeeID == id);
            foreach (Role r in db.Roles)
            {
                try
                {
                    EmployeeRole tmp = roles.Single(x => x.roleID == r.roleID);
                    rolesEdit.Add(r.roleName, true);

                }
                catch (InvalidOperationException ex) {
                    rolesEdit.Add(r.roleName, false);
                }

            }


            foreach (var x in rolesEdit) {
                string[] tmp = x.Key.Split(' ');
                string www = "";
                foreach (string y in tmp) {
                    www += y;
                }
                ViewData[www] = x.Value;
            }
            List<String> rolesx = new List<string>();
            foreach (Role r in db.Roles)
            {
                rolesx.Add(r.roleName);
            }
            ViewBag.Roles = rolesx;
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "employeeID,employeeFirstName,employeeLastName,employeeEmail")] Employee employee)
        {

            Employee tmpEmployee = db.Employees.Find(employee.employeeID);
            tmpEmployee.employeeFirstName = employee.employeeFirstName;
            tmpEmployee.employeeLastName = employee.employeeLastName;
            tmpEmployee.employeeEmail = employee.employeeEmail;


            //deleting all roles for this employee
            var roleDelete = db.EmployeeRoles.Where(x => x.employeeID == employee.employeeID);
            foreach (var tmp in roleDelete) {
                db.EmployeeRoles.Remove(tmp);
            }

            //and adding new ones 
            foreach (Role r in db.Roles)
            {
                string[] tmp = r.roleName.Split(' ');
                string key = "";
                foreach (string x in tmp)
                {
                    key += x;
                }
                bool exist = Convert.ToBoolean(Request.Form[key].Split(',')[0]);
                if (exist)
                {

                    db.EmployeeRoles.Add(new EmployeeRole { employee = tmpEmployee, employeeID = tmpEmployee.employeeID, role = r, roleID = r.roleID,CreateDate = DateTime.Now });
                }

            }


            if (ModelState.IsValid)
            {
                db.Entry(tmpEmployee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {



            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
