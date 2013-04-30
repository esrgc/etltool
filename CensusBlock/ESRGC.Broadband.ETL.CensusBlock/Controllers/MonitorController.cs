using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    public class MonitorController : Controller
    {
        public ActionResult UpdateStatus() {
            if (Session["status"] != null) {

                var status = (Session["status"] as dynamic);
                return Json(status, JsonRequestBehavior.AllowGet);

            }
            else//no operation running
                return Json(
                    new { progress = -1 },
                    JsonRequestBehavior.AllowGet
                );
        }
    }
}
