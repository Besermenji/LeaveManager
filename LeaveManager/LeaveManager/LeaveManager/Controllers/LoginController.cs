using LeaveManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaveManager.Controllers
{
    public class LoginController : Controller
    {
        private LeaveManagerContext db = new LeaveManagerContext();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login([Bind(Include = "email,password")]  LoginViewModel loginViewModel) {
            try {
                Employee emp = db.Employees.Single(x => x.employeeEmail.Equals(loginViewModel.email));
                var r = from l in db.EmployeeRoles
                        where l.employeeID == emp.employeeID
                        select l.role.roleName;
                List<string> roles = r.ToList();

                string pass = EmployeesController.getHashedPassword(loginViewModel.password);
                if (pass.Equals(emp.passwordHash))
                {
                    Session["user"] = emp;
                    Session["roles"] = roles;
                    return RedirectToAction("Index", "EmployeeLeaveRequestViewModels");

                }
                else {
                    TempData["PasswordError"] = "Leave Manager password is incorrect!";
                    TempData["User"] = emp.employeeEmail.ToString();
                    return RedirectToAction("Index", "Login");
                }


            }
            catch(InvalidOperationException ex){

                TempData["EmailError"] = "There is no user with that email!";
                return RedirectToAction("Index", "Login");
            }

           
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["roles"] = null;
            Session.Abandon();

            return View("Index");
        }

        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
