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
    [AuthorizeUser(RoleName = new string[] { "Super User" })]
    public class MailSettingsController : Controller
    {
        private LeaveManagerContext db = new LeaveManagerContext();

       
        public ActionResult Index()
        {
            return View(db.MailSettings.ToList());
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MailSettingsID,Host,Port,Username,Password")] MailSettings mailSettings)
        {
            if (ModelState.IsValid)
            {
                mailSettings.CreateDate = db.MailSettings.Find(mailSettings.MailSettingsID).CreateDate;
                mailSettings.UpdateDate = DateTime.Now;

                db.MailSettings.Find(mailSettings.MailSettingsID).Host = mailSettings.Host;
                db.MailSettings.Find(mailSettings.MailSettingsID).Port = mailSettings.Port;
                db.MailSettings.Find(mailSettings.MailSettingsID).Username = mailSettings.Username;
                db.MailSettings.Find(mailSettings.MailSettingsID).Password = mailSettings.Password;
                db.MailSettings.Find(mailSettings.MailSettingsID).UpdateDate = DateTime.Now;
                
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
