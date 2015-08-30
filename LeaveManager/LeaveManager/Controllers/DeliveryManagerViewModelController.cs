using LeaveManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaveManager.Controllers
{
    public class DeliveryManagerViewModelController : Controller
    {
        LeaveManagerContext db = new LeaveManagerContext();
        // GET: DeliveryManagerViewModel
        public ActionResult Index()
        {
            List<DeliveryManagerLeaveRequestViewModel> deliveryManagerViewModels = new List<DeliveryManagerLeaveRequestViewModel>();

            foreach (LeaveRequest request in db.LeaveRequests) {
                if (request.deliveryManagerStatus.requestStatusName.Equals("pending"))
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

        public ActionResult ProcessRequest(int id) {


            var request = db.LeaveRequests.Find(id);
            ViewData["EmployeeName"] = request.employee.employeeName;
            ViewData["allDay"] = request.allDayEvent ? "YES" : "NO";
            ViewData["startDate"] = request.startTime;
            ViewData["endDate"] = request.endTime;
            ViewData["leaveReason"] = request.leaveReason.leaveReasonName;
            ViewData["description"] = request.Description;
            

            return View();

        }
        [HttpPost]
        public ActionResult ProcessRequest(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                
                string description = Convert.ToString(collection["deliveryManagerComment"]);
                string status = Convert.ToString(collection["status"]);
                db.LeaveRequests.Find(id).deliveryManagerComment = description;
                db.LeaveRequests.Find(id).deliveryManagerStatus = db.RequestStatus.Single(s => s.requestStatusName.Equals(status));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult RequestInfo()
        {
            return View();
        }
    }
}
