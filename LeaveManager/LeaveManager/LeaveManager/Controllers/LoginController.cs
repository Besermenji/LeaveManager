using LeaveManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LeaveManager.Controllers
{
    public class LoginController : Controller
    {
        private LeaveManagerContext db = new LeaveManagerContext();
       
        public ActionResult Index()
        {

            LoginViewModel loginModel = checkCookie();
            if (loginModel == null)
            {
                return View();
            }
            else
            {

                loginModel.RememberMe = true;
                LoginWithCookie(loginModel);
                return RedirectToAction("Index", "Home");

            }
        }

        private LoginViewModel checkCookie()
        {
            LoginViewModel loginVM = null;
            string username = string.Empty;
            string pass = string.Empty;

            if (Request.Cookies["username"] != null)
            {
                username = Request.Cookies["username"].Value;
            }
            if (!String.IsNullOrEmpty(username))
            {

                loginVM = new LoginViewModel() { Email = username, Password = "" };
            }
            return loginVM;
        }

        public ActionResult Login([Bind(Include = "email,password,rememberMe")]  LoginViewModel loginViewModel)
        {
            try
            {
                Employee emp = db.Employees.Single(e => e.EmployeeEmail.Equals(loginViewModel.Email));
                var r = from l in db.EmployeeRoles
                        where l.Employee.EmployeeID == emp.EmployeeID
                        select l.Role.RoleName;
                List<string> roles = r.ToList();

                if (emp != null)
                {
                    string pass = EmployeesController.getHashedPassword(loginViewModel.Password);
                    if (pass.Equals(emp.PasswordHash))
                    {


                        Session["user"] = emp;
                        Session["roles"] = roles;

                        if (loginViewModel.RememberMe)
                        {

                            HttpCookie ckUsername = new HttpCookie("username");
                            ckUsername.Expires = DateTime.Now.AddSeconds(3600);
                            ckUsername.Value = emp.EmployeeEmail;
                            Response.Cookies.Add(ckUsername);
                            
                        }

                        if (roles.Contains("Super User"))
                        {
                            return RedirectToAction("Index", "EmployeeLeaveRequestViewModels");
                        }
                        else if (roles.Contains("Department Manager"))
                        {
                            return RedirectToAction("Index", "DepartmentManagerLeaveRequestViewModels");
                        }
                        else if (roles.Contains("Delivery Manager"))
                        {
                            return RedirectToAction("Index", "DeliveryManagerViewModel");
                        }
                        else if (roles.Contains("Worker"))
                        {
                            return RedirectToAction("Index", "EmployeeLeaveRequestViewModels");
                        }
                        else
                        {
                            return RedirectToAction("Index", "EmployeeLeaveRequestViewModels");
                        }
                        
                    }
                    else
                    {
                        TempData["PasswordError"] = "Leave Manager password is incorrect!";
                        TempData["User"] = emp.EmployeeEmail.ToString();
                        return RedirectToAction("Index", "Login");
                    }

                   

                }
                return RedirectToAction("Index", "Login");

            }
            catch 
            {

                TempData["EmailError"] = "There is no user with that email!";
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult LoginWithCookie(LoginViewModel loginViewModel)
        {
            Employee emp = db.Employees.Single(e => e.EmployeeEmail.Equals(loginViewModel.Email));
            var r = from l in db.EmployeeRoles
                    where l.Employee.EmployeeID == emp.EmployeeID
                    select l.Role.RoleName;
            List<string> roles = r.ToList();

            if (emp != null)
            {
                Session["user"] = emp;
                Session["roles"] = roles;

                if (loginViewModel.RememberMe)
                {

                    HttpCookie ckUsername = new HttpCookie("username");
                    ckUsername.Expires = DateTime.Now.AddSeconds(3600);
                    ckUsername.Value = emp.EmployeeEmail;
                    Response.Cookies.Add(ckUsername);

                }

            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
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
            return View("Index");
        }

        public ActionResult ForgottenPassword() {

            return View();
        }
        [HttpPost]
        public ActionResult ForgottenPassword([Bind(Include = "email")]  LoginViewModel loginViewModel)
        {
            var r = from l in db.Employees
                    where l.EmployeeEmail == loginViewModel.Email
                    select l;

            if (!r.Any())
            {
                ViewBag.emailError = "Wrong email address.";
                return View();
            }
            else {
                Employee emp = r.First();
                string password = Membership.GeneratePassword(10, 3);
                db.Employees.Single(e=>e.EmployeeID==emp.EmployeeID).PasswordHash = EmployeesController.getHashedPassword(password);
                db.SaveChanges();
                sendMailWithNewPass(emp, password);

                TempData["resetSuccess"] = "New password is send to your email address.";
                return RedirectToAction("Index");
            }
        }

        
        public void sendMailWithNewPass(Employee employee,string password)
        {
            string email = employee.EmployeeEmail;
            string subject = "Request Updated";
            string employeeHTMLBody = getHTMLEmailForNewPass(password,employee.EmployeeName);
            EmployeeLeaveRequestViewModelsController.sendMailUsingDBSettings(db, email, subject, employeeHTMLBody);
        }

        private string getHTMLEmailForNewPass(string password,string name)
        {
            StreamReader reader = new StreamReader(Server.MapPath("~/Content/MailTemplate.html"));
            string email = reader.ReadToEnd();

            string notHeader = "Request For New Password";
            string textHeader = "Password Reset";
            string textMessage = "Your new password is " +  "<b style = 'color: #205478' > " + password+" </b><br/> You can change your password anytime, click on the button below to login on Leave Manager Application";
            string buttonText = "Go To Leave Manager";
            //TODO : edit link
            string buttonLink = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "Login/Index/";

            email = email.Replace("[NotificationHeader]", notHeader);
            email = email.Replace("[TextHeader]", textHeader);
            email = email.Replace("[TextMessage]", textMessage);
            email = email.Replace("[ButtonText]", buttonText);
            email = email.Replace("[ButtonLink]", buttonLink);

            return email;
        }

        public ActionResult NotAuthorized() {

            return View();

        }
        
    }
}
