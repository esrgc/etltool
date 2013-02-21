using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToExcel;
using System.IO;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    public class UploadController : BaseController
    {
        public JsonResult uploadFile(HttpPostedFileBase dataInput) {
            if (dataInput == null) {
                return Json(new { Status = "No valid data input" });
            }

            var binaryData = new byte[dataInput.ContentLength];
            //read the data
            dataInput.InputStream.Read(binaryData, 0, dataInput.ContentLength);

            //save to file
            var filePath = HttpContext.Server.MapPath("~/") + "uploadFile";
            try {
                FileInfo f = new FileInfo(filePath);
                if (f.Exists)//delete if already exists
                    f.Delete();
                //open file and overwrite if exists
                using (var fileStream = f.Open(FileMode.CreateNew, FileAccess.Write, FileShare.None)) {
                    fileStream.Write(binaryData, 0, dataInput.ContentLength);
                    fileStream.Close();
                }
            
                //read excel content
                var excel = new ExcelQueryFactory(filePath);
                var worksheets = excel.GetWorksheetNames();

                var allRows = from c in excel.Worksheet(worksheets.First())
                              select c;
                //convert to objects
                foreach (var row in allRows) { 
                    
                }
                //return Json

                return Json(allRows, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                return Json(new { status = "Failed" , message = ex.Message });
            }
        }
    }
}
