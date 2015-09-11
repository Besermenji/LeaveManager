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
    [AuthorizeUser(RoleName = new string[] { "Delivery Manager" })]
    public class DeliveryManagerViewModelController : Controller
    {
        LeaveManagerContext db = new LeaveManagerContext();
        
        public ActionResult Index()
        {
            Dictionary<LeaveRequestInfo, LeaveRequestDescription> leaveRequests = new Dictionary<LeaveRequestInfo, LeaveRequestDescription>();
            List<DeliveryManagerLeaveRequestViewModel> deliveryManagerViewModels = new List<DeliveryManagerLeaveRequestViewModel>();
            Employee emp = (Employee)Session["user"];

            leaveRequests = getLRForCurrentDelManager(emp.EmployeeID);
            deliveryManagerViewModels = mapLeaveRequestsToDelManViewModel(leaveRequests);

            return View(deliveryManagerViewModels);
        }

        private List<DeliveryManagerLeaveRequestViewModel> mapLeaveRequestsToDelManViewModel(Dictionary<LeaveRequestInfo, LeaveRequestDescription> leaveRequests)
        {
            List<DeliveryManagerLeaveRequestViewModel> viewModel = new List<DeliveryManagerLeaveRequestViewModel>();
            foreach (KeyValuePair<LeaveRequestInfo, LeaveRequestDescription> pair in leaveRequests)
            {
                LeaveRequestInfo lri = pair.Key;
                LeaveRequestDescription lrd = pair.Value;
                if (lrd.DeliveryManagerStatus.RequestStatusName.Equals("Pending"))
                {
                    viewModel.Add(new DeliveryManagerLeaveRequestViewModel
                    {
                        AllDayEvent = lrd.AllDayEvent,
                        DeliveryManager = lrd.DeliveryManager,
                        DeliveryManagerComment = lrd.DeliveryManagerComment,
                        DeliveryManagerStatus = lrd.DeliveryManagerStatus,
                        Description = lrd.Description,
                        Employee = lri.Employee,
                        EndTime = lrd.EndTime,
                        LeaveReason = lrd.LeaveReason,
                        StartTime = lrd.StartTime,
                        LeaveRequestInfoID = lri.LeaveRequestInfoID
                    });
                }

            }
            return viewModel;
        }

        private Dictionary<LeaveRequestInfo, LeaveRequestDescription> getLRForCurrentDelManager(int id)
        {
            Dictionary<LeaveRequestInfo, LeaveRequestDescription> allRequests = EmployeeLeaveRequestViewModelsController.getAllLeaveRequests(db);
            Dictionary<LeaveRequestInfo, LeaveRequestDescription> delManRequests = new Dictionary<LeaveRequestInfo, LeaveRequestDescription>();

            foreach (KeyValuePair<LeaveRequestInfo, LeaveRequestDescription> pair in allRequests)
            {

                if (pair.Value.DeliveryManager.EmployeeID == id && pair.Value.DeliveryManagerStatus.RequestStatusName.Equals("Pending"))
                {

                    delManRequests.Add(pair.Key, pair.Value);
                }
            }
            return delManRequests;
        }

        private LeaveRequestDescription getLatestDescription(LeaveRequestInfo lri)
        {
            var q = from l in db.LeaveRequestDescription
                    where l.LeaveRequestInfo.LeaveRequestInfoID == lri.LeaveRequestInfoID
                    select l;

            var latest = q.OrderByDescending(i => i.CreateDate).First();

            return latest;
        }

        public RequestStatus GetRequestStatusByName(string requestStatusName)
        {

            RequestStatus request = db.RequestStatus.Single(r => r.RequestStatusName == requestStatusName);
            return request;
        }

        public ActionResult ProcessRequest(int id)
        {
            LeaveRequestInfo reqInfo = db.LeaveRequestInfo.Find(id);
            LeaveRequestDescription reqDes = getLatestDescription(reqInfo);

            ViewData["EmployeeName"] = reqInfo.Employee.EmployeeName;
            ViewData["allDay"] = reqDes.AllDayEvent ? "YES" : "NO";
            ViewData["startDate"] = reqDes.AllDayEvent ? reqDes.StartTime.ToShortDateString() : reqDes.StartTime.ToString();
            ViewData["endDate"] = reqDes.AllDayEvent ? reqDes.EndTime.ToShortDateString() : reqDes.EndTime.ToString();
            ViewData["leaveReason"] = reqDes.LeaveReason.LeaveReasonName;
            ViewData["description"] = reqDes.Description;
            ViewBag.deliveryManagerStatusID = new SelectList(db.RequestStatus, "requestStatusID", "requestStatusName", GetRequestStatusByName(reqDes.DeliveryManagerStatus.RequestStatusName).RequestStatusID);

            return View();
        }
        
        [HttpPost]
        public ActionResult ProcessRequest(int id, FormCollection collection)
        {
            try
            {
                LeaveRequestInfo lri = db.LeaveRequestInfo.Find(id);
                LeaveRequestDescription lrd = getLatestDescription(lri);

                string description = Convert.ToString(collection["deliveryManagerComment"]);
                string status = Convert.ToString(collection["deliveryManagerStatusID"]);
                lrd.DeliveryManagerComment = description;

                lrd.DeliveryManagerStatus = db.RequestStatus.Find(Convert.ToInt32(status));
                db.SaveChanges();
                
                int employeeID = db.LeaveRequestInfo.Find(id).Employee.EmployeeID;
                if (!lrd.DeliveryManagerStatus.RequestStatusName.Equals("Pending"))
                {

                    sendMailToEmployee(db.LeaveRequestInfo.Find(id).Employee.EmployeeID);

                }
                if (lrd.DeliveryManagerStatus.RequestStatusName.Equals("Approved"))
                {
                    sendMailToDepartmentManager(lri, lrd);
                }


                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("ProcessRequest");
            }

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
            string textMessage = "Your request have been updated by <i style=\"color: #FF8000\">delivery manager.</i>";
            string buttonText = "Go to request";

            string buttonLink = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "EmployeeLeaveRequestViewModels/Index/";

            email = email.Replace("[NotificationHeader]", notHeader);
            email = email.Replace("[TextHeader]", textHeader);
            email = email.Replace("[TextMessage]", textMessage);
            email = email.Replace("[ButtonText]", buttonText);
            email = email.Replace("[ButtonLink]", buttonLink);

            return email;
        }

        private void sendMailToDepartmentManager(LeaveRequestInfo info, LeaveRequestDescription desc)
        {
            Employee employee = db.Employees.Single(e => e.EmployeeID == desc.DepartmentManager.EmployeeID);
            string email = employee.EmployeeEmail;
            string subject = "New Leave Request";
            string depManagerHTMLBody = getHTMLEmailForDepManager(info, desc);
            EmployeeLeaveRequestViewModelsController.sendMailUsingDBSettings(db, email, subject, depManagerHTMLBody);
        }

        private string getHTMLEmailForDepManager(LeaveRequestInfo info, LeaveRequestDescription desc)
        {
            StreamReader reader = new StreamReader(Server.MapPath("~/Content/MailTemplate.html"));
            string email = reader.ReadToEnd();

            string notHeader = "You have new leave request";
            string textHeader = "";
            string textMessage = "You have recieved new leave request from:  <i style=\"color: green\">" + info.Employee.EmployeeName + "</i> Approved by delivery manager:  <i style=\"color: #FF8000\">" + desc.DeliveryManager.EmployeeName + "</i><br/>";
            string buttonText = "Go to request";

            string buttonLink = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "DepartmentManagerLeaveRequestViewModels/ProcessRequest/" + info.LeaveRequestInfoID;

            email = email.Replace("[NotificationHeader]", notHeader);
            email = email.Replace("[TextHeader]", textHeader);
            email = email.Replace("[TextMessage]", textMessage);
            email = email.Replace("[ButtonText]", buttonText);
            email = email.Replace("[ButtonLink]", buttonLink);

            return email;

        }
     
    }
}
