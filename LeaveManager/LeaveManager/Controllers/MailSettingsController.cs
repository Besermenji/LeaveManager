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
    public class MailSettingsController : Controller
    {
        private LeaveManagerContext db = new LeaveManagerContext();

        // GET: MailSettings
        public ActionResult Index()
        {
            return View(db.MailSettings.ToList());
        }

        // GET: MailSettings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailSettings mailSettings = db.MailSettings.Find(id);
            if (mailSettings == null)
            {
                return HttpNotFound();
            }
            return View(mailSettings);
        }

        // POST: MailSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MailSettingsID,Host,Port,Username,Password")] MailSettings mailSettings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mailSettings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mailSettings);
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
