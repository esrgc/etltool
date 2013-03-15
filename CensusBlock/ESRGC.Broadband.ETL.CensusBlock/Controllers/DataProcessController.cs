using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.Broadband.ETL.CensusBlock.Models;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    public class DataProcessController : BaseController
    {
        public ActionResult MapData() {

            if (Session["data"] != null) {
                var obj = Session["data"] as dynamic;
                var data = obj.data as IEnumerable<IDictionary<string, object>>;

                var dataMapping = new DataMappingModel() {
                    UploadData = data,
                    MappingObject = new ColumnMapping()
                };
                return View(dataMapping);
            }
            else {
                updateStatusMessage("Your session has expired. Please upload a new file.");
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
