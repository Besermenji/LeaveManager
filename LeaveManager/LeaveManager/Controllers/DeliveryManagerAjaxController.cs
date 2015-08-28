using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaveManager.Controllers
{
    public class DeliveryManagerAjaxController : Controller
    {
        

        [HttpPost]
        public ActionResult Approved(string jsonRequest) {
            var x = jsonRequest;

            return Json(new { });
        }
        [HttpPost]
        public ActionResult Denied() {
            return Json(new { });
        }
    }
}