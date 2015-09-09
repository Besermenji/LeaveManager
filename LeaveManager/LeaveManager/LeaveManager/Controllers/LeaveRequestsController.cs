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
    public class LeaveRequestsController : Controller
    {
        private LeaveManagerContext db = new LeaveManagerContext();

        // GET: LeaveRequests
        public ActionResult Index()
        {
            // removed Include(l => l.requestStatus).
            var leaveRequests = db.LeaveRequests.Include(l => l.leaveReason).Include(l => l.employee).Include(l => l.deliveryManager).Include(l => l.departmentManager);
            return View(leaveRequests.ToList());
        }

        // GET: LeaveRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaveRequest = db.LeaveRequests.Find(id);
            if (leaveRequest == null)
            {
                return HttpNotFound();
            }
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Create
        public ActionResult Create()
        {
            
            ViewBag.departmentManagerID = new SelectList(db.Employees, "employeeID", "employeeName");
            ViewBag.deliveryManagerID = new SelectList(db.Employees, "employeeID", "employeeName");
            ViewBag.employeeID = new SelectList(db.Employees, "employeeID", "employeeName");
            ViewBag.leaveReasonID = new SelectList(db.LeaveReasons, "leaveReasonID", "leaveReasonName");
            ViewBag.requestStatusID = new SelectList(db.RequestStatus, "requestStatusID", "requestStatusName");
            return View();
        }

        // POST: LeaveRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "leaveRequestID,employeeID,allDayEvent,startTime,endTime,leaveReasonID,Description,deliveryManagerID,deliveryManagerApproved,departmentManagerID,departmentManagerApproved,requestStatusID")] LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                db.LeaveRequests.Add(leaveRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           /* ViewBag.departmentManagerID = new SelectList(db.Employees, "departmentManagerID", "employeeName", leaveRequest.departmentManagerID);
            ViewBag.deliveryManagerID = new SelectList(db.Employees, "deliveryManagerID", "employeeName", leaveRequest.deliveryManagerID);
            ViewBag.employeeID = new SelectList(db.Employees, "employeeID", "employeeName", leaveRequest.employeeID);
            ViewBag.leaveReasonID = new SelectList(db.LeaveReasons, "leaveReasonID", "leaveReasonName", leaveRequest.leaveReasonID);
            ViewBag.requestStatusID = new SelectList(db.RequestStatus, "requestStatusID", "requestStatusName", leaveRequest.requestStatusID);*/
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaveRequest = db.LeaveRequests.Find(id);
            if (leaveRequest == null)
            {
                return HttpNotFound();
            }
          /*  ViewBag.departmentManagerID = new SelectList(db.Employees, "departmentManagerID", "employeeName", leaveRequest.departmentManagerID);
            ViewBag.deliveryManagerID = new SelectList(db.Employees, "deliveryManagerID", "employeeName", leaveRequest.deliveryManagerID);
            ViewBag.employeeID = new SelectList(db.Employees, "employeeID", "employeeName", leaveRequest.employeeID);
            ViewBag.leaveReasonID = new SelectList(db.LeaveReasons, "leaveReasonID", "leaveReasonName", leaveRequest.leaveReasonID);
            ViewBag.requestStatusID = new SelectList(db.RequestStatus, "requestStatusID", "requestStatusName", leaveRequest.requestStatusID);*/
            return View(leaveRequest);
        }

        // POST: LeaveRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "leaveRequestID,employeeID,allDayEvent,startTime,endTime,leaveReasonID,Description,deliveryManagerID,deliveryManagerApproved,departmentManagerID,departmentManagerApproved,requestStatusID")] LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leaveRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            /*ViewBag.departmentManagerID = new SelectList(db.Employees, "departmentManagerID", "employeeName", leaveRequest.departmentManagerID);
            ViewBag.deliveryManagerID = new SelectList(db.Employees, "deliveryManagerID", "employeeName", leaveRequest.deliveryManagerID);
            ViewBag.employeeID = new SelectList(db.Employees, "employeeID", "employeeName", leaveRequest.employeeID);
            ViewBag.leaveReasonID = new SelectList(db.LeaveReasons, "leaveReasonID", "leaveReasonName", leaveRequest.leaveReasonID);
            ViewBag.requestStatusID = new SelectList(db.RequestStatus, "requestStatusID", "requestStatusName", leaveRequest.requestStatusID);*/
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveRequest leaveRequest = db.LeaveRequests.Find(id);
            if (leaveRequest == null)
            {
                return HttpNotFound();
            }
            return View(leaveRequest);
        }

        // POST: LeaveRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LeaveRequest leaveRequest = db.LeaveRequests.Find(id);
            db.LeaveRequests.Remove(leaveRequest);
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
