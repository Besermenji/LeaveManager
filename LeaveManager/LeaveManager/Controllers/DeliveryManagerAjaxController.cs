using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaveManager.Controllers
{
    public class DeliveryManagerAjaxController : Controller
    {
        // GET: Ajax
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Approved() {

            return Json(new { });
        }
        public ActionResult Denied() {
            return Json(new { });
        }
    }
}