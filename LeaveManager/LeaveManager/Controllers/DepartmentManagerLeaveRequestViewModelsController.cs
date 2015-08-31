using LeaveManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            var m = from e in db.LeaveRequests
                    where (e.deliveryManagerStatus.requestStatusName.Equals("Approved")) && e.departmentManagerStatus.requestStatusName.Equals("Pending")
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


                return RedirectToAction("Index");
            }

            return View();
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
    }
}
