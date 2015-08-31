using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LeaveManager.Models;

namespace LeaveManager.Controllers
{
    public class EmployeeLeaveRequestViewModelsController : Controller
    {
        private LeaveManagerContext db = new LeaveManagerContext();

        // GET: EmployeeLeaveRequestViewModels
        public ActionResult Index()
        {
            //var employeeLeaveRequestViewModels = db.LeaveRequests.Include(e => e.leaveReason).Include(e => e.deliveryManagerStatus).Include(e => e.departmentManagerStatus);

            List<EmployeeLeaveRequestViewModel> requestViewModelList = new List<EmployeeLeaveRequestViewModel>();


            foreach (LeaveRequest req in db.LeaveRequests)
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
        public ActionResult Create([Bind(Include = "leaveRequestID,employeeID,allDayEvent,startTime,endTime,leaveReasonID,Description,deliveryManagerID,departmentManagerID")] EmployeeLeaveRequestViewModel employeeLeaveRequestViewModel)
        {
            LeaveRequest newLeaveRequest = mapLeaveRequestViewModel(employeeLeaveRequestViewModel);

            if (ModelState.IsValid)
            {

                db.LeaveRequests.Add(newLeaveRequest);
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            // ViewBag.leaveReasonID = new SelectList(db.LeaveReasons, "leaveReasonID", "leaveReasonName", employeeLeaveRequestViewModel.leaveReasonID);
            //ViewBag.requestStatusID = new SelectList(db.RequestStatus, "requestStatusID", "requestStatusName", employeeLeaveRequestViewModel.requestStatusID);
            return View(employeeLeaveRequestViewModel);
        }

        public RequestStatus GetRequestStatusByName(string requestStatusName)
        {

            RequestStatus request = db.RequestStatus.Single(r => r.requestStatusName == requestStatusName);
            return request;
        }


        private LeaveRequest mapLeaveRequestViewModel(EmployeeLeaveRequestViewModel leaveRequestViewModel)
        {
            var emp = db.Employees.Find(leaveRequestViewModel.employeeID);
            var delMan = db.Employees.Find(leaveRequestViewModel.deliveryManagerID);
            var depMan = db.Employees.Find(leaveRequestViewModel.departmentManagerID);
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
