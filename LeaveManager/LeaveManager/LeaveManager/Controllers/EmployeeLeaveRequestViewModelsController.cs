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
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace LeaveManager.Controllers
{
    [AuthorizeUser(RoleName = new string[] { "Worker","Super User" })]
    public class EmployeeLeaveRequestViewModelsController : Controller
    {
        private LeaveManagerContext db = new LeaveManagerContext();
        
       
        public ActionResult Index()
        {
           
            List<String> roles =(List<String>)Session["roles"];
            List<EmployeeLeaveRequestViewModel> requestViewModelList = new List<EmployeeLeaveRequestViewModel>();
            Dictionary<LeaveRequestInfo, LeaveRequestDescription> leaveRequests = new Dictionary<LeaveRequestInfo, LeaveRequestDescription>();

            if (roles == null) {

                return RedirectToAction("Index", "Login");
            }

            foreach (string role in roles) {

                if (role.Equals("Super User")) {
                    
                    leaveRequests = getAllLeaveRequests(db);
                }
            }
            if (leaveRequests.Count == 0)
            {
                leaveRequests = getLRForCurrentUser();

            }
            foreach (KeyValuePair<LeaveRequestInfo,LeaveRequestDescription> pair in leaveRequests)
            {

                LeaveRequestInfo reqInfo = pair.Key;
                LeaveRequestDescription reqDesc = pair.Value;

                EmployeeLeaveRequestViewModel viewModel = new EmployeeLeaveRequestViewModel()
            {
                    AllDayEvent = reqDesc.AllDayEvent,
                    DeliveryManager = reqDesc.DeliveryManager,
                    DeliveryManagerStatus = reqDesc.DeliveryManagerStatus,
                    DepartmentManager = reqDesc.DepartmentManager,
                    DepartmentManagerStatus = reqDesc.DepartmentManagerStatus,
                    Description = reqDesc.Description,
                    Employee = reqInfo.Employee,
                    EndTime = reqDesc.EndTime,
                    LeaveReason = reqDesc.LeaveReason,
                    LeaveRequestID = reqInfo.LeaveRequestInfoID,
                    StartTime = reqDesc.StartTime,
                    DepartmentManagerComment = reqDesc.DepartmentManagerComment,
                    DeliveryManagerComment = reqDesc.DeliveryManagerComment

                };

                requestViewModelList.Add(viewModel);
            }
            return View(requestViewModelList.AsQueryable());
        }

        public static Dictionary<LeaveRequestInfo, LeaveRequestDescription> getAllLeaveRequests(LeaveManagerContext db)
        {
            Dictionary<LeaveRequestInfo, LeaveRequestDescription> leaveRequests = new Dictionary<LeaveRequestInfo, LeaveRequestDescription>();
            
            foreach (LeaveRequestInfo lri in db.LeaveRequestInfo) {

                LeaveRequestDescription latestDesc = getLatestDescription(lri,db);
                leaveRequests.Add(lri, latestDesc);

            }
            return leaveRequests;
        }

        public static LeaveRequestDescription getLatestDescription(LeaveRequestInfo lri,LeaveManagerContext db)
        {
            var q = from l in db.LeaveRequestDescription
                    where l.LeaveRequestInfo.LeaveRequestInfoID == lri.LeaveRequestInfoID
                    select l;

            var latest = q.OrderByDescending(i=>i.CreateDate).First();

            return latest;
        }
        
        private Dictionary<LeaveRequestInfo,LeaveRequestDescription> getLRForCurrentUser()
        {

            Dictionary<LeaveRequestInfo, LeaveRequestDescription> leaveRequests = new Dictionary<LeaveRequestInfo, LeaveRequestDescription>();

            Employee emp = (Employee)Session["user"];
            var e = from l in db.LeaveRequestInfo
                    where l.Employee.EmployeeID == emp.EmployeeID
                    select l;
            
            foreach (LeaveRequestInfo lri in e) {

                LeaveRequestDescription lrd = getLatestDescription(lri,db);
                leaveRequests.Add(lri, lrd);

            }

            return leaveRequests;
        }

        public ActionResult getAllEmployees() {

            List<Employee> employees = db.Employees.ToList();
            employees.Sort((x, y) => string.Compare(x.EmployeeName, y.EmployeeName));
            return Json(employees, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult getAllDelManagers()
        {

            var m = from e in db.EmployeeRoles
                    where e.Role.RoleName == "Delivery Manager"
                    select e.Employee;
            List<Employee> list = new List<Employee>();
            list = m.ToList();
            list.Sort((x, y) => string.Compare(x.EmployeeName, y.EmployeeName));
            return Json(list, JsonRequestBehavior.AllowGet);



        }

        public ActionResult getAllDepManagers()
        {

            var m = from e in db.EmployeeRoles
                    where e.Role.RoleName == "Department Manager"
                    select e.Employee;

            List<Employee> list = new List<Employee>();
            list = m.ToList();
            list.Sort((x, y) => string.Compare(x.EmployeeName, y.EmployeeName));
            return Json(m.ToList(), JsonRequestBehavior.AllowGet);



        }

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
                    where e.Role.RoleName == roleName
                    select e.Employee;
            SelectList sl = new SelectList(m, "employeeID", "employeeName");
            return sl;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "leaveRequestID,employeeName,deliveryManagerName,departmentManagerName,allDayEvent,startTime,endTime,leaveReasonID,Description,deliveryManagerID,departmentManagerID")] EmployeeLeaveRequestViewModel employeeLeaveRequestViewModel)
        {
            if (!serverValidation(employeeLeaveRequestViewModel)) {
                ViewBag.leaveReasonID = new SelectList(db.LeaveReasons, "leaveReasonID", "leaveReasonName");
                return View(employeeLeaveRequestViewModel);
            }
         
            LeaveRequestInfo newLeaveRequest = mapViewModelToLeaveRequestInfo(employeeLeaveRequestViewModel);
            LeaveRequestDescription newLeaveRequestDesc = mapViewModelToLeaveRequestDesc(employeeLeaveRequestViewModel,newLeaveRequest);

            if (ModelState.IsValid)
            {
                db.LeaveRequestInfo.Add(newLeaveRequest);
                db.LeaveRequestDescription.Add(newLeaveRequestDesc);
                db.SaveChanges();
                
                sendMailToDeliveryManager(newLeaveRequest, newLeaveRequestDesc);


                return RedirectToAction("Index");
            }
            return View(employeeLeaveRequestViewModel);
        }

        private LeaveRequestDescription mapViewModelToLeaveRequestDesc(EmployeeLeaveRequestViewModel leaveRequestViewModel, LeaveRequestInfo newLeaveRequest)
        {
            var delMan = getEmployeeByName(leaveRequestViewModel.DeliveryManagerName);//db.Employees.Find(leaveRequestViewModel.deliveryManagerID);
            var depMan = getEmployeeByName(leaveRequestViewModel.DepartmentManagerName);//db.Employees.Find(leaveRequestViewModel.departmentManagerID);
            var lreason = db.LeaveReasons.Find(leaveRequestViewModel.LeaveReasonID);

            RequestStatus initStatus = GetRequestStatusByName("Pending");

            LeaveRequestDescription lrDescription = new LeaveRequestDescription()
            {

                AllDayEvent = leaveRequestViewModel.AllDayEvent,
                StartTime = leaveRequestViewModel.StartTime,
                EndTime = leaveRequestViewModel.EndTime,
                LeaveReason = lreason,
                Description = leaveRequestViewModel.Description,
                DeliveryManager = delMan,
                DeliveryManagerComment = "",
                DepartmentManager = depMan,
                DepartmentManagerComment = "",
                DepartmentManagerStatus = initStatus,
                DeliveryManagerStatus = initStatus,
                LeaveRequestInfo = newLeaveRequest,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
                

            };
            
            return lrDescription;
        }

        private LeaveRequestInfo mapViewModelToLeaveRequestInfo(EmployeeLeaveRequestViewModel leaveRequestViewModel)
        {

            var emp = getEmployeeByName(leaveRequestViewModel.EmployeeName);
      
            LeaveRequestInfo newLeaveRequest = new LeaveRequestInfo()
            {
                LeaveRequestInfoID = leaveRequestViewModel.LeaveRequestID,
                Employee = emp,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
                
            };
            return newLeaveRequest;

            
        }

        private bool serverValidation(EmployeeLeaveRequestViewModel employeeLeaveRequestViewModel)
        {
            int empID = getEmployeeIDByName(employeeLeaveRequestViewModel.EmployeeName);
            int delID = getEmployeeIDByName(employeeLeaveRequestViewModel.DeliveryManagerName);
            int depID = getEmployeeIDByName(employeeLeaveRequestViewModel.DepartmentManagerName);
            if (empID == -1 || delID == -1 || depID == -1) {
                return false;
            }

            Employee emp = db.Employees.Find(getEmployeeIDByName(employeeLeaveRequestViewModel.EmployeeName));
            Employee del = db.Employees.Find(getEmployeeIDByName(employeeLeaveRequestViewModel.DeliveryManagerName));
            Employee dep = db.Employees.Find(getEmployeeIDByName(employeeLeaveRequestViewModel.DepartmentManagerName));


            return !(emp == null || del == null || dep == null);
        }

        private void sendMailToDeliveryManager(LeaveRequestInfo info,LeaveRequestDescription desc)
        {
            Employee employee = db.Employees.Single(e => e.EmployeeID == desc.DeliveryManager.EmployeeID);
            string email = employee.EmployeeEmail;
            string subject = "New Leave Request";
            string HTMLBody = getHTMLEmail(info.Employee.EmployeeName,info.LeaveRequestInfoID);
            sendMailUsingDBSettings(db, employee.EmployeeEmail, subject, HTMLBody);

        }

        private void sendMailToDelManRequestUpdated(LeaveRequestInfo info,LeaveRequestDescription desc)
        {
            Employee employee = db.Employees.Single(e => e.EmployeeID == desc.DeliveryManager.EmployeeID);
            string email = employee.EmployeeEmail;
            string subject = "Request Updated";
            string HTMLBody = getHTMLEmailForRequestUpdate(info.Employee.EmployeeName, info.LeaveRequestInfoID);
            sendMailUsingDBSettings(db, employee.EmployeeEmail, subject, HTMLBody);

        }

        private string getHTMLEmail(string employeeName, int leaveRequestID)
        {

            StreamReader reader = new StreamReader(Server.MapPath("~/Content/MailTemplate.html"));
            string email = reader.ReadToEnd();

            string notHeader = "You have new leave request";
            string textHeader = "";
            string textMessage = "<h3>You have recieved new leave request from: <i style=\"color: green\">" + employeeName + "</i> </h3><br/>";
            string buttonText = "Go to request";
            string buttonLink = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "DeliveryManagerViewModel/ProcessRequest/" + leaveRequestID; ;

            email = email.Replace("[NotificationHeader]",notHeader);
            email = email.Replace("[TextHeader]", textHeader);
            email = email.Replace("[TextMessage]", textMessage);
            email = email.Replace("[ButtonText]", buttonText);
            email = email.Replace("[ButtonLink]", buttonLink);

            return email;
            

        }

        private string getHTMLEmailForRequestUpdate(string employeeName, int leaveRequestID)
        {

            StreamReader reader = new StreamReader(Server.MapPath("~/Content/MailTemplate.html"));
            string email = reader.ReadToEnd();

            string notHeader = "Request Updated";
            string textHeader = "";
            string textMessage = "<h3>Existing request has been updated by: <i style=\"color: green\">" + employeeName + "</i> </h3><br/>";
            string buttonText = "Go to request";
            string buttonLink = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "DeliveryManagerViewModel/ProcessRequest/" + leaveRequestID; ;

            email = email.Replace("[NotificationHeader]", notHeader);
            email = email.Replace("[TextHeader]", textHeader);
            email = email.Replace("[TextMessage]", textMessage);
            email = email.Replace("[ButtonText]", buttonText);
            email = email.Replace("[ButtonLink]", buttonLink);

            return email;
            

        }

        public MailSettings getMailSettings()
        {
            return db.MailSettings.First();
        }

        public static void sendMailUsingDBSettings(LeaveManagerContext database, string to,string subject,string body) {
            try
            {
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
            catch {

            }
        }

        public RequestStatus GetRequestStatusByName(string requestStatusName)
        {
            RequestStatus request = db.RequestStatus.Single(r => r.RequestStatusName == requestStatusName);
            return request;
        }

        public Employee getEmployeeByName(string name) {
            try {
                string firstName = name.Split(' ')[0];
                string lastName = name.Split(' ')[1];
                return db.Employees.Single(e => (e.EmployeeFirstName.Equals(firstName) && e.EmployeeLastName.Equals(lastName)));
            }
            catch {

                return null;

            }

        }

        public int getEmployeeIDByName(string name)
        {
            if (getEmployeeByName(name) != null)
            {

                return getEmployeeByName(name).EmployeeID;
            }
            else {

                return -1;
            }

            

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LeaveRequestInfo reqInfo = db.LeaveRequestInfo.Find(id);
            LeaveRequestDescription reqDes = getLatestDescription(reqInfo, db);
            EmployeeLeaveRequestViewModel viewModel =  mapLeaveRequestToViewModel(reqInfo, reqDes);

            SelectList deliveryManagers = getEmployeeByRoleName("Delivery Manager");
            SelectList departmentManagers = getEmployeeByRoleName("Department Manager");

            ViewBag.departmentManagerID = departmentManagers;
            ViewBag.deliveryManagerID = deliveryManagers;

            ViewBag.employeeID = new SelectList(db.Employees, "employeeID", "employeeName");
            ViewBag.leaveReasonID = new SelectList(db.LeaveReasons, "leaveReasonID", "leaveReasonName");

            if (reqInfo == null)
            {
                return HttpNotFound();
            }
            return View(viewModel);
           
        }

        private EmployeeLeaveRequestViewModel mapLeaveRequestToViewModel(LeaveRequestInfo reqInfo, LeaveRequestDescription reqDes)
        {
            return new EmployeeLeaveRequestViewModel()
            {
                AllDayEvent = reqDes.AllDayEvent,
                DeliveryManager = reqDes.DeliveryManager,
                DeliveryManagerComment = reqDes.DeliveryManagerComment,
                DeliveryManagerID = reqDes.DeliveryManager.EmployeeID,
                DeliveryManagerName = reqDes.DeliveryManager.EmployeeName,
                DeliveryManagerStatus = reqDes.DeliveryManagerStatus,
                DepartmentManager = reqDes.DepartmentManager,
                EmployeeName = reqInfo.Employee.EmployeeName,
                Employee = reqInfo.Employee,
                EmployeeID = reqInfo.Employee.EmployeeID,
                DepartmentManagerComment = reqDes.DepartmentManagerComment,
                DepartmentManagerID = reqDes.DepartmentManager.EmployeeID,
                DepartmentManagerName = reqDes.DepartmentManager.EmployeeName,
                DepartmentManagerStatus = reqDes.DepartmentManagerStatus,
                Description = reqDes.Description,
                EndTime = reqDes.EndTime,
                LeaveReason = reqDes.LeaveReason,
                LeaveReasonID = reqDes.LeaveReason.LeaveReasonID,
                LeaveRequestID = reqDes.LeaveRequestInfo.LeaveRequestInfoID,
                StartTime = reqDes.StartTime
                
            };

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "leaveRequestID,employeeName,deliveryManagerName,departmentManagerName,allDayEvent,startTime,endTime,leaveReasonID,Description,deliveryManagerID,departmentManagerID")] EmployeeLeaveRequestViewModel employeeLeaveRequestViewModel)
        {
            if (!serverValidation(employeeLeaveRequestViewModel))
        {
                ViewBag.leaveReasonID = new SelectList(db.LeaveReasons, "leaveReasonID", "leaveReasonName");
                return View(employeeLeaveRequestViewModel);
            }

            LeaveRequestInfo leaveRequestInfo = db.LeaveRequestInfo.Find(employeeLeaveRequestViewModel.LeaveRequestID);
            LeaveRequestDescription leaveRequestDesc = getLatestDescription(leaveRequestInfo,db);
            LeaveRequestDescription newLeaveRequestDescription = mapViewModelToLeaveRequestDesc(employeeLeaveRequestViewModel, leaveRequestInfo);

            if (ModelState.IsValid)
            {
                db.LeaveRequestDescription.Add(newLeaveRequestDescription);
                db.SaveChanges();


                if (employeeLeaveRequestViewModel.DeliveryManagerName.Equals(leaveRequestDesc.DeliveryManager.EmployeeName))
                {
                    sendMailToDelManRequestUpdated(leaveRequestInfo, newLeaveRequestDescription);
                }
                else {
                    sendMailToDeliveryManager(leaveRequestInfo, newLeaveRequestDescription);
                }

                return RedirectToAction("Index");
            }
            return View(employeeLeaveRequestViewModel);
         
        }

        public static string ReplacePdfForm(LeaveRequestInfo info, LeaveRequestDescription desc)
        {
           
            string dt = DateTime.Now.ToString().Replace('/', ' ').Replace(':', ' ');
            // string fileNameNew = System.AppDomain.CurrentDomain.BaseDirectory +@"\Resources\\"+info.Employee.EmployeeName+" "+dt+".pdf".Replace(' ', '_');

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\LeaveManagerPDF\";

            bool exists = Directory.Exists(path);

            if (!exists)
            {
                Directory.CreateDirectory(path);
            }
               

            string fileNameNew = path + info.Employee.EmployeeName + " " + dt + ".pdf".Replace(' ', '_');

            // using (var existingFileStream = new FileStream(fileNameExisting, FileMode.Open))
            using (var newFileStream = new FileStream(fileNameNew, FileMode.Create))
            {
                // Open existing PDF
                var pdfReader = new PdfReader(Properties.Resources.LeaveRequestTemplatePDF);

                // PdfStamper, which will create
                var stamper = new PdfStamper(pdfReader, newFileStream);

                var form = stamper.AcroFields;
                form.GenerateAppearances = true;
                var fieldKeys = form.Fields.Keys;


                //setting font size and setting field text
                form.SetFieldProperty("Employee", "textsize", 8.0f, null);
                form.SetField("Employee",info.Employee.EmployeeName);

                form.SetFieldProperty("AllDayEvent", "textsize", 8.0f, null);
                form.SetField("AllDayEvent", desc.AllDayEvent ? "YES" : "NO");

                form.SetFieldProperty("StartTime", "textsize", 8.0f, null);
                form.SetField("StartTime", desc.AllDayEvent ? desc.StartTime.ToShortDateString() : desc.StartTime.ToString());

                form.SetFieldProperty("EndTime", "textsize", 8.0f, null);
                form.SetField("EndTime", desc.AllDayEvent ? desc.EndTime.ToShortDateString() : desc.EndTime.ToString());


                form.SetFieldProperty("LeaveReason", "textsize", 8.0f, null);
                form.SetField("LeaveReason",desc.LeaveReason.LeaveReasonName);

                form.SetFieldProperty("LeaveDescription", "textsize", 8.0f, null);
                form.SetField("LeaveDescription", desc.Description);

                form.SetFieldProperty("DeliveryManager", "textsize", 8.0f, null);
                form.SetField("DeliveryManager", desc.DeliveryManager.EmployeeName);

                form.SetFieldProperty("DeliveryManagerComment", "textsize", 8.0f, null);
                form.SetField("DeliveryManagerComment", desc.DeliveryManagerComment);

                form.SetFieldProperty("DepartmentManager", "textsize", 8.0f, null);
                form.SetField("DepartmentManager", desc.DepartmentManager.EmployeeName);

                form.SetFieldProperty("DepartmentManagerComment", "textsize", 8.0f, null);
                form.SetField("DepartmentManagerComment", desc.DepartmentManagerComment);

                form.SetFieldProperty("CreateDate", "textsize", 8.0f, null);
                form.SetField("CreateDate",desc.CreateDate.ToString());


                form.SetFieldProperty("EmployeeSignature", "textsize", 8.0f, null);
                form.SetField("EmployeeSignature",info.Employee.EmployeeName);

                form.SetFieldProperty("DeliveryManagerSignature", "textsize", 8.0f, null);
                form.SetField("DeliveryManagerSignature",desc.DeliveryManager.EmployeeName);


                form.SetFieldProperty("DepartmentManagerSignature", "textsize", 8.0f, null);
                form.SetField("DepartmentManagerSignature", desc.DepartmentManager.EmployeeName);

                form.SetFieldProperty("Date", "textsize", 8.0f, null);
                form.SetField("Date", DateTime.Now.ToShortDateString());
                
                // "Flatten" the form so it wont be editable/usable anymore
                stamper.FormFlattening = true;

                stamper.Close();
                pdfReader.Close();
                return fileNameNew;
            }
        }

        public ActionResult printPDF(int? id)
        {
            LeaveRequestInfo lri = db.LeaveRequestInfo.Find(id);
            LeaveRequestDescription lrd = getLatestDescription(lri,db);
            string tmp = ReplacePdfForm(lri,lrd);
            
            return File(@tmp, "application/pdf", lri.Employee.EmployeeName+" "+DateTime.Now+".pdf");
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
