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
    [AuthorizeUser(RoleName = new string[] { "Super User" })]
    public class EmployeesController : Controller
    {
        private LeaveManagerContext db = new LeaveManagerContext();

        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        public ActionResult Create()
        {
            List<String> roles = new List<string>();
            foreach (Role r in db.Roles) {
                roles.Add(r.RoleName);
            }
            ViewBag.Roles = roles;        
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,EmployeeFirstName,EmployeeLastName,EmployeeEmail")] Employee employee)
        {
            foreach (Role r in db.Roles)
            {
                string[] tmp = r.RoleName.Split(' ');
                string key = "";
                foreach (string x in tmp)
                {
                    key += x;
                }
                bool exist = Convert.ToBoolean(Request.Form[key].Split(',')[0]);
                if (exist)
                {

                    db.EmployeeRoles.Add(new EmployeeRole { Employee = employee, Role = r ,CreateDate = DateTime.Now,UpdateDate=DateTime.Now});
                }

            }
            if (ModelState.IsValid)
            {
                string password = Membership.GeneratePassword(10, 3);
                employee.CreateDate = DateTime.Now;
                employee.UpdateDate = DateTime.Now;
                employee.PasswordHash = getHashedPassword(password);

                db.Employees.Add(employee);
                db.SaveChanges();

                sendMailToNewEmployee(employee.EmployeeID, password);

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
            Employee employee = db.Employees.Single(e => e.EmployeeID == employeeID);
            string email = employee.EmployeeEmail;
            string subject = "Registration";
            string employeeHTMLBody = getHTMLEmailForNewEmployee(password,employee.EmployeeName);
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

        [AuthorizeUser(RoleName = new string[] { "Worker" , "Delivery Manager","Department Manager","Super User"})]
        public ActionResult AccountSettings()
        {
            Employee emp = (Employee)Session["user"];

            if (emp == null) {

                return RedirectToAction("Index", "Login");

            }

            AccountSettingsViewModel settings = new AccountSettingsViewModel()
            {
                EmployeeID = emp.EmployeeID,
                EmployeeFirstName = emp.EmployeeFirstName,
                EmployeeLastName = emp.EmployeeLastName,
                EmployeeEmail = emp.EmployeeEmail
            };


            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AccountSettings([Bind(Include = "employeeID,employeeEmail,oldPassword,newPassword,confirmPassword")] AccountSettingsViewModel accountSettings)
        {
            Employee emp = db.Employees.Single(e => e.EmployeeID == accountSettings.EmployeeID);

            if (ModelState.IsValid && getHashedPassword(accountSettings.OldPassword).Equals(emp.PasswordHash) && accountSettings.NewPassword.Equals(accountSettings.ConfirmPassword))
            {

                db.Employees.Single(e => e.EmployeeID == accountSettings.EmployeeID).EmployeeEmail = accountSettings.EmployeeEmail;
                db.Employees.Single(e => e.EmployeeID == accountSettings.EmployeeID).PasswordHash = getHashedPassword(accountSettings.NewPassword);
                db.SaveChanges();
                LogoutUser();

               
                return RedirectToAction("Index","Login");
            }
            else {

                return View();
            }
            
        }

        public void LogoutUser() {

            Session["user"] = null;
            Session["roles"] = null;
            Session.Abandon();

            if (Response.Cookies["username"] != null)
            {
                HttpCookie ckUsername = new HttpCookie("username");
                ckUsername.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ckUsername);
            }
            if (Response.Cookies["password"] != null)
            {
                HttpCookie ckPassword = new HttpCookie("password");
                ckPassword.Expires = DateTime.Now.AddSeconds(-1d);
                Response.Cookies.Add(ckPassword);

            }

        }

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

            var roles = db.EmployeeRoles.Where(x => x.Employee.EmployeeID == id);
            foreach (Role r in db.Roles)
            {
                try
                {
                    EmployeeRole tmp = roles.Single(x => x.Role.RoleID == r.RoleID);
                    rolesEdit.Add(r.RoleName, true);

                }
                catch (InvalidOperationException ex) {
                    rolesEdit.Add(r.RoleName, false);
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
                rolesx.Add(r.RoleName);
            }
            ViewBag.Roles = rolesx;
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "employeeID,employeeFirstName,employeeLastName,employeeEmail")] Employee employee)
        {

            Employee tmpEmployee = db.Employees.Find(employee.EmployeeID);
            tmpEmployee.EmployeeFirstName = employee.EmployeeFirstName;
            tmpEmployee.EmployeeLastName = employee.EmployeeLastName;
            tmpEmployee.EmployeeEmail = employee.EmployeeEmail;

            //deleting all roles for this employee
            var roleDelete = db.EmployeeRoles.Where(x => x.Employee.EmployeeID == employee.EmployeeID);
            DateTime dt;
            foreach (var tmp in roleDelete) {
                db.EmployeeRoles.Remove(tmp);
               
            }
            //and adding new ones 
            foreach (Role r in db.Roles)
            {
                string[] tmp = r.RoleName.Split(' ');
                string key = "";
                foreach (string x in tmp)
                {
                    key += x;
                }
                bool exist = Convert.ToBoolean(Request.Form[key].Split(',')[0]);
                if (exist)
                {

                    db.EmployeeRoles.Add(new EmployeeRole { Employee = tmpEmployee,  Role = r , CreateDate = DateTime.Now, UpdateDate = DateTime.Now});
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

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            List<EmployeeRole> toRemove = db.EmployeeRoles.Where(e => e.Employee.EmployeeID == id).ToList();

            foreach (EmployeeRole er in toRemove) {
                db.EmployeeRoles.Remove(er);
            }

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
