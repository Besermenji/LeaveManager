using LeaveManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LeaveManager.Controllers
{
    public class DepartmentManagerLeaveRequestViewModelsController : Controller
    {

        private LeaveManagerContext db = new LeaveManagerContext();
        // GET: DepartmentManagerLeaveRequestViewModels
        public ActionResult Index()
        {
            List<DepartmentManagerLeaveRequestViewModel> requstsList = new List<DepartmentManagerLeaveRequestViewModel>();
            Employee emp = (Employee)Session["user"];
            var m = from e in db.LeaveRequests
                    where (e.deliveryManagerStatus.requestStatusName.Equals("Approved")) && e.departmentManagerStatus.requestStatusName.Equals("Pending") && (e.departmentManager.employeeID == emp.employeeID)
                    select e;

            

            foreach (LeaveRequest req in m)
            {
                DepartmentManagerLeaveRequestViewModel viewModel = new DepartmentManagerLeaveRequestViewModel()
                {
                    allDayEvent = req.allDayEvent,
                    departmentManager = req.departmentManager,
                    departmentManagerStatus = req.departmentManagerStatus,
                    employee = req.employee,
                    endTime = req.endTime,
                    description = req.Description,
                    leaveReason = req.leaveReason,
                    startTime = req.startTime,
                    departmentManagerComment = req.departmentManagerComment,
                    leaveRequestID = req.leaveRequestID

                };

                requstsList.Add(viewModel);


            }

            return View(requstsList.AsQueryable());
        }

        // GET: DepartmentManagerLeaveRequestViewModels/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DepartmentManagerLeaveRequestViewModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DepartmentManagerLeaveRequestViewModels/Create
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

        public ActionResult ProcessRequest(int id)
        {
            LeaveRequest leaveRequest = db.LeaveRequests.Find(id);



            DepartmentManagerLeaveRequestViewModel viewModel = new DepartmentManagerLeaveRequestViewModel()
            {

                allDayEvent = leaveRequest.allDayEvent,
                departmentManager = leaveRequest.departmentManager,
                departmentManagerStatus = leaveRequest.departmentManagerStatus,
                employee = leaveRequest.employee,
                endTime = leaveRequest.endTime,
                description = leaveRequest.Description,
                leaveReason = leaveRequest.leaveReason,
                startTime = leaveRequest.startTime,
                departmentManagerComment = leaveRequest.departmentManagerComment,
                leaveRequestID = leaveRequest.leaveRequestID

            };

            // ViewBag.possibleStatuses = new SelectList(db.RequestStatus, "requestStatusID", "requestStatusName",GetRequestStatusByName(leaveRequest.departmentManagerStatus.requestStatusName).requestStatusID);
            ViewBag.departmentManagerStatusID = new SelectList(db.RequestStatus, "requestStatusID", "requestStatusName", GetRequestStatusByName(leaveRequest.departmentManagerStatus.requestStatusName).requestStatusID);
            ViewData["employeeName"] = leaveRequest.employee.employeeName;
            ViewData["startTime"] = leaveRequest.startTime;
            ViewData["endTime"] = leaveRequest.endTime;
            ViewData["allDayEvent"] = leaveRequest.allDayEvent;


            return View("DepartmentManagerProcessRequest", viewModel);
        }

        [HttpPost]
        public ActionResult ProcessRequest([Bind(Include = "leaveRequestID,departmentManagerComment,departmentManagerStatusID")] DepartmentManagerLeaveRequestViewModel departmentManagerLeaveRequestViewModel)
        {
            if (ModelState.IsValid)
            {

                var requestToUpdate = db.LeaveRequests.Find(departmentManagerLeaveRequestViewModel.leaveRequestID);
                requestToUpdate.departmentManagerComment = departmentManagerLeaveRequestViewModel.departmentManagerComment;
                requestToUpdate.departmentManagerStatus = db.RequestStatus.Find(departmentManagerLeaveRequestViewModel.departmentManagerStatusID);

                db.SaveChanges();


                if (!requestToUpdate.departmentManagerStatus.requestStatusName.Equals("Pending"))
                {

                    sendMailToEmployee(db.LeaveRequests.Find(requestToUpdate.leaveRequestID).employee.employeeID);

                }


                return RedirectToAction("Index");
            }

            return View();
        }


        private void sendMailToEmployee(int employeeID)
        {
            //Employee employee = db.Employees.Single(e => e.employeeID == employeeID);
            //string email = employee.employeeEmail;
            //string subject = "Request Updated";
            //string body = "<h3>Your leave request have been updated by <i style=\"color: red\">department manager</i>.</h3>" + DeliveryManagerViewModelController.getLinkToRequestList();
            //string smtpHost = "smtp.gmail.com";

            //string employeeHTMLBody = getHTMLEmailForEmployee();
            //// sendMail("leavemanager9@gmail.com", email, subject, body, smtpHost
            //EmployeeLeaveRequestViewModelsController.sendMailUsingDBSettings(db, email, subject, employeeHTMLBody);
        }

        private string getHTMLEmailForEmployee()
        {
            StreamReader reader = new StreamReader(Server.MapPath("~/Content/MailTemplate.html"));
            string email = reader.ReadToEnd();

            string notHeader = "Request Updated";
            string textHeader = "";
            string textMessage = "Your leave request have been updated by <i style=\"color: red\">department manager</i>.";
            string buttonText = "Go to request";
            string buttonLink = "http://localhost:9877/EmployeeLeaveRequestViewModels/Index/";

            email = email.Replace("[NotificationHeader]", notHeader);
            email = email.Replace("[TextHeader]", textHeader);
            email = email.Replace("[TextMessage]", textMessage);
            email = email.Replace("[ButtonText]", buttonText);
            email = email.Replace("[ButtonLink]", buttonLink);

            return email;
        }

        public void sendMail(string from, string to, string subject, string body, string smtpHost)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.From = new System.Net.Mail.MailAddress(from);
            message.To.Add(new System.Net.Mail.MailAddress(to));
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = subject;
            message.Body = body;



            using (var smtp = new SmtpClient())
            {

                var credential = new NetworkCredential
                {

                    UserName = "leavemanager9@gmail.com",
                    Password = "managerleave9"

                };
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 25;
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
        }
        public RequestStatus GetRequestStatusByName(string requestStatusName)
        {

            RequestStatus request = db.RequestStatus.Single(r => r.requestStatusName == requestStatusName);
            return request;
        }
        // GET: DepartmentManagerLeaveRequestViewModels/Edit/5
        public ActionResult Edit(int id)
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

        // POST: DepartmentManagerLeaveRequestViewModels/Edit/5
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

        // GET: DepartmentManagerLeaveRequestViewModels/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DepartmentManagerLeaveRequestViewModels/Delete/5
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


        private List<LeaveRequest> getLRForCurrentUser()
        {

            Employee emp = (Employee)Session["user"];




            var e = from l in db.LeaveRequests
                    where l.employee.employeeID == emp.employeeID
                    select l;

            return e.ToList();
        }

    }
}
