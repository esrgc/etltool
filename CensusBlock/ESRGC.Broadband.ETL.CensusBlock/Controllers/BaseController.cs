using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    public class BaseController : Controller
    {
        public void updateStatusMessage(string message) {
            TempData["message"] = message;
        }
    }
}
