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
    public class DeliveryManagerLeaveRequestViewModelsController : Controller
    {
        private LeaveManagerContext db = new LeaveManagerContext();
        

        // GET: DeliveryManagerLeaveRequestViewModels
        public ActionResult Index()
        {
            //enabling property acces for employee name
            /* int idTmp = GetRequestStatusID("Waiting For Delivery Manager Approve");
             var deliveryManagerLeaveRequestViewModels = from u in db.LeaveRequests
                                                         where u.requestStatusID ==  idTmp select u;


             foreach (LeaveRequest req in deliveryManagerLeaveRequestViewModels)
             {

                     req.employee = db.Employees.Find(req.employeeID);
                     req.deliveryManager = db.Employees.Find(req.deliveryManagerID);

             }
             return View(deliveryManagerLeaveRequestViewModels.ToList());*/
            return View();
        }

        //method that gets id of request id with certain name
        public int GetRequestStatusID(string requestStatusName)
        {
            
            RequestStatus request = db.RequestStatus.Single(r => r.requestStatusName == requestStatusName);
            return request.requestStatusID;
        }


        //public ActionResult Approve(int? id) {
        //     if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    } 
        //    LeaveRequest deliveryManagerLeaveRequestViewModel = db.LeaveRequests.Find(id);
        //    if (deliveryManagerLeaveRequestViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    //TODO1: add some stuff to get comment out, to update LeaveReques db with comment and delete this entery from index because it is processed
        //    //db.LeaveRequests.Remove(deliveryManagerLeaveRequestViewModel);
        //    db.LeaveRequests.Find(id).deliveryManagerStatus.requestStatusID = GetRequestStatusID("Waiting For Department Manager Approve");
        //    db.LeaveRequests.Find(id).requestStatus.requestStatusName = "Waiting For Department Manager Approve";
        //    db.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        //public ActionResult Deny(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    LeaveRequest deliveryManagerLeaveRequestViewModel = db.LeaveRequests.Find(id);

            
        //    if (deliveryManagerLeaveRequestViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    //TODO2: add some stuff to get comment out, to update LeaveReques db with comment and delete this entery from index because it is processed
        //    int reqID = GetRequestStatusID("Declined");
        //    db.LeaveRequests.Find(id).requestStatusID = reqID;
        //    db.LeaveRequests.Find(id).requestStatus.requestStatusName = "Declined";
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        // GET: DeliveryManagerLeaveRequestViewModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryManagerLeaveRequestViewModel deliveryManagerLeaveRequestViewModel = db.DeliveryManagerLeaveRequestViewModels.Find(id);
            if (deliveryManagerLeaveRequestViewModel == null)
            {
                return HttpNotFound();
            }
            return View(deliveryManagerLeaveRequestViewModel);
        }

        // GET: DeliveryManagerLeaveRequestViewModels/Create
        public ActionResult Create()
        {
            return View();
        }
        
        // POST: DeliveryManagerLeaveRequestViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lID,deliveryManagerID,deliveryManagerApproved,deliveryManagerComment")] DeliveryManagerLeaveRequestViewModel deliveryManagerLeaveRequestViewModel)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryManagerLeaveRequestViewModels.Add(deliveryManagerLeaveRequestViewModel);
                //get data from create and write in main database
                var managerID = deliveryManagerLeaveRequestViewModel.deliveryManagerID;
                var isApprowed = deliveryManagerLeaveRequestViewModel.deliveryManagerApproved;
                var comment = deliveryManagerLeaveRequestViewModel.deliveryManagerComment;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deliveryManagerLeaveRequestViewModel);
        }

        // GET: DeliveryManagerLeaveRequestViewModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryManagerLeaveRequestViewModel deliveryManagerLeaveRequestViewModel = db.DeliveryManagerLeaveRequestViewModels.Find(id);
            if (deliveryManagerLeaveRequestViewModel == null)
            {
                return HttpNotFound();
            }
            return View(deliveryManagerLeaveRequestViewModel);
        }

        // POST: DeliveryManagerLeaveRequestViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lID,deliveryManagerID,deliveryManagerApproved,deliveryManagerComment")] DeliveryManagerLeaveRequestViewModel deliveryManagerLeaveRequestViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryManagerLeaveRequestViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deliveryManagerLeaveRequestViewModel);
        }

        // GET: DeliveryManagerLeaveRequestViewModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryManagerLeaveRequestViewModel deliveryManagerLeaveRequestViewModel = db.DeliveryManagerLeaveRequestViewModels.Find(id);
            if (deliveryManagerLeaveRequestViewModel == null)
            {
                return HttpNotFound();
            }
            return View(deliveryManagerLeaveRequestViewModel);
        }

        // POST: DeliveryManagerLeaveRequestViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryManagerLeaveRequestViewModel deliveryManagerLeaveRequestViewModel = db.DeliveryManagerLeaveRequestViewModels.Find(id);
            db.DeliveryManagerLeaveRequestViewModels.Remove(deliveryManagerLeaveRequestViewModel);
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
