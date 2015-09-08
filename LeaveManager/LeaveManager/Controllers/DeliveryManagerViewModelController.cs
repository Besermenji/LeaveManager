using LeaveManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mail;
using System.Net.Mail;
using System.IO;

namespace LeaveManager.Controllers
{
    public class DeliveryManagerViewModelController : Controller
    {
        LeaveManagerContext db = new LeaveManagerContext();
        // GET: DeliveryManagerViewModel
        public ActionResult Index()
        {

            if (!checkAuthorization()) {

                return Content("<script>alert('You are not authorized to see this page!')</script>");

            }

            List<DeliveryManagerLeaveRequestViewModel> deliveryManagerViewModels = new List<DeliveryManagerLeaveRequestViewModel>();

            foreach (LeaveRequest request in db.LeaveRequests)
            {
                if (request.deliveryManagerStatus.requestStatusName.Equals("Pending"))
                {
                    deliveryManagerViewModels.Add(new DeliveryManagerLeaveRequestViewModel
                    {

                        allDayEvent = request.allDayEvent,
                        deliveryManager = request.deliveryManager,
                        deliveryManagerComment = request.deliveryManagerComment,
                        deliveryManagerStatus = request.deliveryManagerStatus,
                        Description = request.Description,
                        employee = request.employee,
                        endTime = request.endTime,
                        leaveReason = request.leaveReason,
                        startTime = request.startTime,
                        LeaveRequestId = request.leaveRequestID,
                        DeliveryManagerID = request.deliveryManager.employeeID,
                        EmployeeID = request.employee.employeeID,
                        LeaveReasonID = request.leaveReason.leaveReasonID,
                        deliveryManagerStatusID = request.deliveryManagerStatus.requestStatusID

                    });
                }

            }
            return View(deliveryManagerViewModels);
        }

        private bool checkAuthorization()
        {

            Employee e = (Employee)Session["user"];

            if (e == null) {
                return false;
            }

            var r = from l in db.EmployeeRoles
                    where l.employeeID == e.employeeID
                    select l.role.roleName;

            List<string> roles = r.ToList();

            if (roles.Contains("Delivery Manager"))
            {
                return true;
            }
            else {
                return false;
            }


        }

        // GET: DeliveryManagerViewModel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeliveryManagerViewModel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeliveryManagerViewModel/Create
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

        // GET: DeliveryManagerViewModel/Edit/5
        public ActionResult Edit(int id)
        {

            return View();
        }

        // POST: DeliveryManagerViewModel/Edit/5
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
        public RequestStatus GetRequestStatusByName(string requestStatusName)
        {

            RequestStatus request = db.RequestStatus.Single(r => r.requestStatusName == requestStatusName);
            return request;
        }
        public ActionResult ProcessRequest(int id)
        {


            var request = db.LeaveRequests.Find(id);
            ViewData["EmployeeName"] = request.employee.employeeName;
            ViewData["allDay"] = request.allDayEvent ? "YES" : "NO";
            ViewData["startDate"] = request.allDayEvent ? request.startTime.ToShortDateString() : request.startTime.ToString();
            ViewData["endDate"] = request.allDayEvent ? request.startTime.ToShortDateString() : request.endTime.ToString();
            ViewData["leaveReason"] = request.leaveReason.leaveReasonName;
            ViewData["description"] = request.Description;
            ViewBag.deliveryManagerStatusID = new SelectList(db.RequestStatus, "requestStatusID", "requestStatusName", GetRequestStatusByName(request.deliveryManagerStatus.requestStatusName).requestStatusID);

            return View();

        }
        [HttpPost]
        public ActionResult ProcessRequest(int id, FormCollection collection)
        {

            // TODO: Add update logic here
            try
            {
                LeaveRequest lr = db.LeaveRequests.Find(id);
                string description = Convert.ToString(collection["deliveryManagerComment"]);
                string status = Convert.ToString(collection["deliveryManagerStatusID"]);
                lr.deliveryManagerComment = description;

                lr.deliveryManagerStatus = db.RequestStatus.Find(Convert.ToInt32(status));
                db.SaveChanges();


                //Send Mail
                //int employeeID = db.LeaveRequests.Find(id).employee.employeeID;

                //sendMailToEmployee(db.LeaveRequests.Find(id).employee.employeeID);
                //if (status.Equals("Approved"))
                //{
                //    sendMailToDepartmentManager(db.LeaveRequests.Find(id));
                //}


                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("ProcessRequest");
            }

        }

        private void sendMailToEmployee(int employeeID)
        {
            //Employee employee = db.Employees.Single(e => e.employeeID == employeeID);
            //string email = employee.employeeEmail;
            //string subject = "Request Updated";
            //string body = "<h3>Your leave request have been updated by <i style=\"color: #FF8000\">delivery manager.</i><h3>" + getLinkToRequestList();
            //string smtpHost = "smtp.gmail.com";

            //string employeeHTMLBody = getHTMLEmailForEmployee();

            ////sendMail("leavemanager9@gmail.com", email, subject, body, smtpHost);
            //EmployeeLeaveRequestViewModelsController.sendMailUsingDBSettings(db, email, subject, employeeHTMLBody);
        }

        private string getHTMLEmailForEmployee()
        {
            StreamReader reader = new StreamReader(Server.MapPath("~/Content/MailTemplate.html"));
            string email = reader.ReadToEnd();

            string notHeader = "Request Updated";
            string textHeader = "";
            string textMessage = "Your request have been updated by <i style=\"color: #FF8000\">delivery manager.</i>";
            string buttonText = "Go to request";
            string buttonLink = "http://localhost:9877/EmployeeLeaveRequestViewModels/Index/";

            email = email.Replace("[NotificationHeader]", notHeader);
            email = email.Replace("[TextHeader]", textHeader);
            email = email.Replace("[TextMessage]", textMessage);
            email = email.Replace("[ButtonText]", buttonText);
            email = email.Replace("[ButtonLink]",  buttonLink);

            return email;
        }

        private void sendMailToDepartmentManager(LeaveRequest leaveRequest)
        {

            //string linkToProcessRequest = getLinkForRequest(leaveRequest.leaveRequestID);
            //Employee employee = db.Employees.Single(e => e.employeeID == leaveRequest.departmentManager.employeeID);
            //string email = employee.employeeEmail;
            //string subject = "New Leave Request";
            //string body = "You have recieved new leave request from:  <i style=\"color: green\">" + leaveRequest.employee.employeeName + "</i> Approved by delivery manager:  <i style=\"color: #FF8000\">" + leaveRequest.deliveryManager.employeeName + "</i><br/>" + getLinkForRequest(leaveRequest.leaveRequestID);

            //string depManagerHTMLBody = getHTMLEmailForDepManager(leaveRequest);

            ////sendMail("leavemanager9@gmail.com", employee.employeeEmail, subject, body, smtpHost);
            //EmployeeLeaveRequestViewModelsController.sendMailUsingDBSettings(db, email, subject, depManagerHTMLBody);
        }

        private string getHTMLEmailForDepManager(LeaveRequest leaveRequest)
        {
            StreamReader reader = new StreamReader(Server.MapPath("~/Content/MailTemplate.html"));
            string email = reader.ReadToEnd();

            string notHeader = "You have new leave request";
            string textHeader = "";
            string textMessage = "You have recieved new leave request from:  <i style=\"color: green\">" + leaveRequest.employee.employeeName + "</i> Approved by delivery manager:  <i style=\"color: #FF8000\">" + leaveRequest.deliveryManager.employeeName + "</i><br/>";
            string buttonText = "Go to request";
            string buttonLink = "http://localhost:9877/DepartmentManagerLeaveRequestViewModels/ProcessRequest/" + leaveRequest.leaveRequestID;

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

        private string getLinkForRequest(int leaveRequestID)
        {
            string url = "http://localhost:9877/DepartmentManagerLeaveRequestViewModels/ProcessRequest/" + leaveRequestID;


            string HTMLlink = "<a href=\"" + url + "\">Go to request.</a>";

            return HTMLlink;
        }

       

        public static string getLinkToRequestList()
        {

            string url = "http://localhost:9877/EmployeeLeaveRequestViewModels/Index/";


            string HTMLlink = "<br/><a href=\"" + url + "\">Go to requests list.</a>";

            return HTMLlink;

        }

        public ActionResult RequestInfo()
        {
            return View();
        }
    }
}
