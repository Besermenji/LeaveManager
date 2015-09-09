using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LeaveManager.Models;

using System.Text;
using System.Net.Mail;
using System.IO;

namespace LeaveManager.Controllers
{
    public class EmployeeLeaveRequestViewModelsController : Controller
    {
        private LeaveManagerContext db = new LeaveManagerContext();
        

        // GET: EmployeeLeaveRequestViewModels
        public ActionResult Index()
        {
            //var employeeLeaveRequestViewModels = db.LeaveRequests.Include(e => e.leaveReason).Include(e => e.deliveryManagerStatus).Include(e => e.departmentManagerStatus);
            List<String> roles =(List<String>)Session["roles"];
            List<EmployeeLeaveRequestViewModel> requestViewModelList = new List<EmployeeLeaveRequestViewModel>();
            List<LeaveRequest> leaveRequests = new List<LeaveRequest>();
            foreach (string role in roles) {

                if (role.Equals("Super User")) {
                    leaveRequests = db.LeaveRequests.ToList();
                }
         

            }
            if (leaveRequests.Count == 0) {
                leaveRequests = getLRForCurrentUser();
            }

            foreach (LeaveRequest req in leaveRequests)
            {

                EmployeeLeaveRequestViewModel viewModel = new EmployeeLeaveRequestViewModel()
                {
                    allDayEvent = req.allDayEvent,
                    deliveryManager = req.deliveryManager,
                    deliveryManagerStatus = req.deliveryManagerStatus,
                    departmentManager = req.departmentManager,
                    departmentManagerStatus = req.departmentManagerStatus,
                    Description = req.Description,
                    employee = req.employee,
                    endTime = req.endTime,
                    leaveReason = req.leaveReason,
                    leaveRequestID = req.leaveRequestID,
                    startTime = req.startTime,
                    departmentManagerComment = req.departmentManagerComment,
                    deliveryManagerComment = req.deliveryManagerComment



                };

                requestViewModelList.Add(viewModel);


            }
            return View(requestViewModelList.AsQueryable());
        }

        private List<LeaveRequest> getLRForCurrentUser()
        {

            Employee emp = (Employee)Session["user"];

            


            var e = from l in db.LeaveRequests
                    where l.employee.employeeID == emp.employeeID
                    select l;

            return e.ToList();
        }

        public ActionResult getAllEmployees() {

            List<Employee> employees = db.Employees.ToList();
            employees.Sort((x, y) => string.Compare(x.employeeName, y.employeeName));
            return Json(employees, JsonRequestBehavior.AllowGet);



        }

        public ActionResult getAllDelManagers()
        {

            var m = from e in db.EmployeeRoles
                    where e.role.roleName == "Delivery Manager"
                    select e.employee;
            List<Employee> list = new List<Employee>();
            list = m.ToList();
            list.Sort((x, y) => string.Compare(x.employeeName, y.employeeName));
            return Json(list, JsonRequestBehavior.AllowGet);



        }

        public ActionResult getAllDepManagers()
        {

            var m = from e in db.EmployeeRoles
                    where e.role.roleName == "Department Manager"
                    select e.employee;

            List<Employee> list = new List<Employee>();
            list = m.ToList();
            list.Sort((x, y) => string.Compare(x.employeeName, y.employeeName));
            return Json(m.ToList(), JsonRequestBehavior.AllowGet);



        }

        // GET: EmployeeLeaveRequestViewModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //EmployeeLeaveRequestViewModel employeeLeaveRequestViewModel = db.EmployeeLeaveRequestViewModels.Find(id);
            //if (employeeLeaveRequestViewModel == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(employeeLeaveRequestViewModel);

            return null;
        }

        // GET: EmployeeLeaveRequestViewModels/Create
        public ActionResult Create()
        {
           

            SelectList deliveryManagers = getEmployeeByRoleName("Delivery Manager");
            SelectList departmentManagers = getEmployeeByRoleName("Department Manager");

            ViewBag.departmentManagerID = departmentManagers;
            ViewBag.deliveryManagerID = deliveryManagers;

            ViewBag.employeeID = new SelectList(db.Employees, "employeeID", "employeeName");
            ViewBag.leaveReasonID = new SelectList(db.LeaveReasons, "leaveReasonID", "leaveReasonName");

            return View();
        }

        private SelectList getEmployeeByRoleName(string roleName)
        {

            var m = from e in db.EmployeeRoles
                    where e.role.roleName == roleName
                    select e.employee;
            SelectList sl = new SelectList(m, "employeeID", "employeeName");
            return sl;

        }



        //}

        // POST: EmployeeLeaveRequestViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "leaveRequestID,employeeName,deliveryManagerName,departmentManagerName,allDayEvent,startTime,endTime,leaveReasonID,Description,deliveryManagerID,departmentManagerID")] EmployeeLeaveRequestViewModel employeeLeaveRequestViewModel)
        {
            if (!serverValidation(employeeLeaveRequestViewModel)) {
                ViewBag.leaveReasonID = new SelectList(db.LeaveReasons, "leaveReasonID", "leaveReasonName");
                return View(employeeLeaveRequestViewModel);
            }
            LeaveRequest newLeaveRequest = mapLeaveRequestViewModel(employeeLeaveRequestViewModel);
            newLeaveRequest.CreateDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                
                db.LeaveRequests.Add(newLeaveRequest);
                db.SaveChanges();

                //int newRequestID = newLeaveRequest.leaveRequestID;
                //sendMailToDeliveryManager(getEmployeeIDByName(employeeLeaveRequestViewModel.deliveryManagerName), newLeaveRequest);

                return RedirectToAction("Index");
            }
            return View(employeeLeaveRequestViewModel);
        }

        private bool serverValidation(EmployeeLeaveRequestViewModel employeeLeaveRequestViewModel)
        {
            int empID = getEmployeeIDByName(employeeLeaveRequestViewModel.employeeName);
            int delID = getEmployeeIDByName(employeeLeaveRequestViewModel.deliveryManagerName);
            int depID = getEmployeeIDByName(employeeLeaveRequestViewModel.departmentManagerName);

         

            if (empID == -1 || delID == -1 || depID == -1) {
                return false;
            }

            Employee emp = db.Employees.Find(getEmployeeIDByName(employeeLeaveRequestViewModel.employeeName));
            Employee del = db.Employees.Find(getEmployeeIDByName(employeeLeaveRequestViewModel.deliveryManagerName));
            Employee dep = db.Employees.Find(getEmployeeIDByName(employeeLeaveRequestViewModel.departmentManagerName));


            return !(emp == null || del == null || dep == null);
        }

        private void sendMailToDeliveryManager(int deliveryManagerID, LeaveRequest leaveRequest)
        {

            string linkToProcessRequest = getLink(leaveRequest.leaveRequestID);
            Employee employee = db.Employees.Single(e => e.employeeID == deliveryManagerID);
            string email = employee.employeeEmail;
            string subject = "New Leave Request";
            string body = "<h3>You have recieved new leave request from: <i style=\"color: green\">" + leaveRequest.employee.employeeName + "</i> </h3><br/>" + getLink(leaveRequest.leaveRequestID);
            string smtpHost = "smtp.gmail.com";

            string HTMLBody = getHTMLEmail(leaveRequest.employee.employeeName,leaveRequest.leaveRequestID);

            //sendMail("leavemanager9@gmail.com", employee.employeeEmail, subject, body, smtpHost);
            sendMailUsingDBSettings(db, employee.employeeEmail, subject, HTMLBody);

        }

        private string getHTMLEmail(string employeeName, int leaveRequestID)
        {

            StreamReader reader = new StreamReader(Server.MapPath("~/Content/MailTemplate.html"));
            string email = reader.ReadToEnd();

            string notHeader = "You have new leave request";
            string textHeader = "";
            string textMessage = "<h3>You have recieved new leave request from: <i style=\"color: green\">" + employeeName + "</i> </h3><br/>";
            string buttonText = "Go to request";
            string buttonLink = getLink(leaveRequestID);

            email = email.Replace("[NotificationHeader]",notHeader);
            email = email.Replace("[TextHeader]", textHeader);
            email = email.Replace("[TextMessage]", textMessage);
            email = email.Replace("[ButtonText]", buttonText);
            email = email.Replace("[ButtonLink]", "http://localhost:9877/DeliveryManagerViewModel/ProcessRequest/" + leaveRequestID);

            return email;
            

        }

        public MailSettings getMailSettings()
        {
            return db.MailSettings.First();
        }

        public static void sendMailUsingDBSettings(LeaveManagerContext database, string to,string subject,string body) {

            MailSettings ms = new MailSettings();
            ms = database.MailSettings.First();

            MailMessage message = new MailMessage();
            message.From = new MailAddress(ms.Username);
            message.To.Add(new MailAddress(to));
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = subject;
            message.Body = body;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {

                    UserName = ms.Username,
                    Password = ms.Password

                };
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.Host = ms.Host;
                smtp.Port = ms.Port;
                smtp.EnableSsl = true;
                smtp.Send(message);
            }


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

        private string getLink(int leaveRequestID)
        {
            string url = "http://localhost:9877/DeliveryManagerViewModel/ProcessRequest/" + leaveRequestID;


            string HTMLlink = "<a href=\"" + url + "\">Go to request.</a>";

            return HTMLlink;
        }

        public RequestStatus GetRequestStatusByName(string requestStatusName)
        {
            RequestStatus request = db.RequestStatus.Single(r => r.requestStatusName == requestStatusName);
            return request;
        }


        private LeaveRequest mapLeaveRequestViewModel(EmployeeLeaveRequestViewModel leaveRequestViewModel)
        {
            //var emp = db.Employees.Find(leaveRequestViewModel.employeeID);
            var emp = getEmployeeByName(leaveRequestViewModel.employeeName);
            var delMan = getEmployeeByName(leaveRequestViewModel.deliveryManagerName);//db.Employees.Find(leaveRequestViewModel.deliveryManagerID);
            var depMan = getEmployeeByName(leaveRequestViewModel.departmentManagerName);//db.Employees.Find(leaveRequestViewModel.departmentManagerID);
            var lreason = db.LeaveReasons.Find(leaveRequestViewModel.leaveReasonID);

            RequestStatus initStatus = GetRequestStatusByName("Pending");
            // RequestStatus approvedStatus = GetRequestStatusByName("Approved");
            LeaveRequest newLeaveRequst = new LeaveRequest()
            {
                leaveRequestID = leaveRequestViewModel.leaveRequestID,
                employee = emp,
                allDayEvent = leaveRequestViewModel.allDayEvent,
                startTime = leaveRequestViewModel.startTime,
                endTime = leaveRequestViewModel.endTime,
                leaveReason = lreason,
                Description = leaveRequestViewModel.Description,
                deliveryManager = delMan,
                deliveryManagerComment = "",
                departmentManager = depMan,
                departmentManagerComment = "",
                departmentManagerStatus = initStatus,
                deliveryManagerStatus = initStatus
            };


            return newLeaveRequst;
        }

        public Employee getEmployeeByName(string name) {
            try {
                string firstName = name.Split(' ')[0];
                string lastName = name.Split(' ')[1];
                return db.Employees.Single(e => (e.employeeFirstName.Equals(firstName) && e.employeeLastName.Equals(lastName)));
            }
            catch {

                return null;

            }

        }

        public int getEmployeeIDByName(string name)
        {
            if (getEmployeeByName(name) != null)
            {

                return getEmployeeByName(name).employeeID;
            }
            else {

                return -1;
            }

            

        }

        // GET: EmployeeLeaveRequestViewModels/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //EmployeeLeaveRequestViewModel employeeLeaveRequestViewModel = db.EmployeeLeaveRequestViewModels.Find(id);
            //if (employeeLeaveRequestViewModel == null)
            //{
            //    return HttpNotFound();
            //}
            ////ViewBag.leaveReasonID = new SelectList(db.LeaveReasons, "leaveReasonID", "leaveReasonName", employeeLeaveRequestViewModel.leaveReasonID);
            ////ViewBag.requestStatusID = new SelectList(db.RequestStatus, "requestStatusID", "requestStatusName", employeeLeaveRequestViewModel.requestStatusID);
            //return View(employeeLeaveRequestViewModel);
            return null;
        }

        // POST: EmployeeLeaveRequestViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "leaveRequestID,employeeID,allDayEvent,startTime,endTime,leaveReasonID,Description,deliveryManagerID,requestStatusID")] EmployeeLeaveRequestViewModel employeeLeaveRequestViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeLeaveRequestViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.leaveReasonID = new SelectList(db.LeaveReasons, "leaveReasonID", "leaveReasonName", employeeLeaveRequestViewModel.leaveReasonID);
            //ViewBag.requestStatusID = new SelectList(db.RequestStatus, "requestStatusID", "requestStatusName", employeeLeaveRequestViewModel.requestStatusID);
            return View(employeeLeaveRequestViewModel);
        }

        // GET: EmployeeLeaveRequestViewModels/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //EmployeeLeaveRequestViewModel employeeLeaveRequestViewModel = db.EmployeeLeaveRequestViewModels.Find(id);
            //if (employeeLeaveRequestViewModel == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(employeeLeaveRequestViewModel);
            return null;
        }

        // POST: EmployeeLeaveRequestViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //EmployeeLeaveRequestViewModel employeeLeaveRequestViewModel = db.EmployeeLeaveRequestViewModels.Find(id);
            //db.EmployeeLeaveRequestViewModels.Remove(employeeLeaveRequestViewModel);
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
