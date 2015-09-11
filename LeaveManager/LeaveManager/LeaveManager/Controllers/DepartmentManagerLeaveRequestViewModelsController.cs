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
    [AuthorizeUser(RoleName = new string[] { "Department Manager" })]
    public class DepartmentManagerLeaveRequestViewModelsController : Controller
    {

        private LeaveManagerContext db = new LeaveManagerContext();
        // GET: DepartmentManagerLeaveRequestViewModels
        public ActionResult Index()
        {
            List<DepartmentManagerLeaveRequestViewModel> requstsListViewModel = new List<DepartmentManagerLeaveRequestViewModel>();
            Dictionary<LeaveRequestInfo, LeaveRequestDescription> leaveRequests = new Dictionary<LeaveRequestInfo, LeaveRequestDescription>();
            Employee emp = (Employee)Session["user"];

            leaveRequests = getLatestApprovedByDelManRequests(emp.EmployeeID);
            requstsListViewModel = mapLRToDepManViewModel(leaveRequests);
       
            return View(requstsListViewModel.AsQueryable());
        }

        private List<DepartmentManagerLeaveRequestViewModel> mapLRToDepManViewModel(Dictionary<LeaveRequestInfo, LeaveRequestDescription> leaveRequests)
        {
            List<DepartmentManagerLeaveRequestViewModel> viewModelList = new List<DepartmentManagerLeaveRequestViewModel>();

            foreach (KeyValuePair<LeaveRequestInfo,LeaveRequestDescription> pair in leaveRequests)
            {
                LeaveRequestInfo lri = pair.Key;
                LeaveRequestDescription lrd = pair.Value;

                DepartmentManagerLeaveRequestViewModel viewModel = new DepartmentManagerLeaveRequestViewModel()
                {
                    AllDayEvent = lrd.AllDayEvent,
                    DepartmentManager = lrd.DepartmentManager,
                    DepartmentManagerStatus = lrd.DepartmentManagerStatus,
                    Employee = lri.Employee,
                    EndTime = lrd.EndTime,
                    Description = lrd.Description,
                    LeaveReason = lrd.LeaveReason,
                    StartTime = lrd.StartTime,
                    DepartmentManagerComment = lrd.DepartmentManagerComment,
                    LeaveRequestID = lri.LeaveRequestInfoID

                };

                viewModelList.Add(viewModel);
            }

            return viewModelList;
        }

        private Dictionary<LeaveRequestInfo, LeaveRequestDescription> getLatestApprovedByDelManRequests(int id)
        {
            Dictionary<LeaveRequestInfo, LeaveRequestDescription> allRequests = EmployeeLeaveRequestViewModelsController.getAllLeaveRequests(db);
            Dictionary<LeaveRequestInfo, LeaveRequestDescription> approvedByDelManRequests = new Dictionary<LeaveRequestInfo, LeaveRequestDescription>();

            foreach (KeyValuePair<LeaveRequestInfo, LeaveRequestDescription> pair in allRequests) {

                if (pair.Value.DepartmentManager.EmployeeID == id && pair.Value.DeliveryManagerStatus.RequestStatusName.Equals("Approved") && pair.Value.DepartmentManagerStatus.RequestStatusName.Equals("Pending")) {

                    approvedByDelManRequests.Add(pair.Key,pair.Value);
                }
            }
            return approvedByDelManRequests;

        }

        public ActionResult ProcessRequest(int id)
        {
           
            LeaveRequestInfo reqInfo = db.LeaveRequestInfo.Find(id);
            LeaveRequestDescription reqDes = EmployeeLeaveRequestViewModelsController.getLatestDescription(reqInfo,db);
            DepartmentManagerLeaveRequestViewModel viewModel = new DepartmentManagerLeaveRequestViewModel()
            {

                AllDayEvent = reqDes.AllDayEvent,
                DepartmentManager = reqDes.DepartmentManager,
                DepartmentManagerStatus = reqDes.DepartmentManagerStatus,
                Employee = reqInfo.Employee,
                EndTime = reqDes.EndTime,
                Description = reqDes.Description,
                LeaveReason = reqDes.LeaveReason,
                StartTime = reqDes.StartTime,
                DepartmentManagerComment = reqDes.DepartmentManagerComment,
                LeaveRequestID = reqInfo.LeaveRequestInfoID

            };
            
            ViewBag.departmentManagerStatusID = new SelectList(db.RequestStatus, "requestStatusID", "requestStatusName", GetRequestStatusByName(reqDes.DepartmentManagerStatus.RequestStatusName).RequestStatusID);
            ViewData["employeeName"] = reqInfo.Employee.EmployeeName;
            ViewData["startTime"] = reqDes.StartTime;
            ViewData["endTime"] = reqDes.EndTime;
            ViewData["allDayEvent"] = reqDes.AllDayEvent;


            return View("DepartmentManagerProcessRequest", viewModel);
        }

        [HttpPost]
        public ActionResult ProcessRequest([Bind(Include = "leaveRequestID,departmentManagerComment,departmentManagerStatusID")] DepartmentManagerLeaveRequestViewModel departmentManagerLeaveRequestViewModel)
        {
            if (ModelState.IsValid)
            {
               
                LeaveRequestInfo requestToUpdate = db.LeaveRequestInfo.Find(departmentManagerLeaveRequestViewModel.LeaveRequestID);
                LeaveRequestDescription descToUpdate = EmployeeLeaveRequestViewModelsController.getLatestDescription(requestToUpdate,db);
                descToUpdate.DepartmentManagerComment = departmentManagerLeaveRequestViewModel.DepartmentManagerComment;
                descToUpdate.DepartmentManagerStatus = db.RequestStatus.Find(departmentManagerLeaveRequestViewModel.DepartmentManagerStatusID);
                db.SaveChanges();

                if (!descToUpdate.DepartmentManagerStatus.RequestStatusName.Equals("Pending"))
                {

                    sendMailToEmployee(requestToUpdate.Employee.EmployeeID);

                }
                return RedirectToAction("Index");
            }

            return View();
        }
        
        private void sendMailToEmployee(int employeeID)
        {
            Employee employee = db.Employees.Single(e => e.EmployeeID == employeeID);
            string email = employee.EmployeeEmail;
            string subject = "Request Updated";
            string employeeHTMLBody = getHTMLEmailForEmployee();
       
            EmployeeLeaveRequestViewModelsController.sendMailUsingDBSettings(db, email, subject, employeeHTMLBody);
        }

        private string getHTMLEmailForEmployee()
        {
            StreamReader reader = new StreamReader(Server.MapPath("~/Content/MailTemplate.html"));
            string email = reader.ReadToEnd();

            string notHeader = "Request Updated";
            string textHeader = "";
            string textMessage = "Your leave request have been updated by <i style=\"color: red\">department manager</i>.";
            string buttonText = "Go to request";
           // string buttonLink = "http://localhost:9877/EmployeeLeaveRequestViewModels/Index/";

            string buttonLink = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "EmployeeLeaveRequestViewModels/Index/";

       

            email = email.Replace("[NotificationHeader]", notHeader);
            email = email.Replace("[TextHeader]", textHeader);
            email = email.Replace("[TextMessage]", textMessage);
            email = email.Replace("[ButtonText]", buttonText);
            email = email.Replace("[ButtonLink]", buttonLink);

            return email;
        }
        
        public RequestStatus GetRequestStatusByName(string requestStatusName)
        {

            RequestStatus request = db.RequestStatus.Single(r => r.RequestStatusName == requestStatusName);
            return request;
        }
       
    }
}
