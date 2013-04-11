using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToExcel;
using System.IO;
using ESRGC.Broadband.ETL.CensusBlock.Extension;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    public class UploadController : Controller
    {
        /// <summary>
        /// provides upload form
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFile(bool? newUpload) {
            bool discard = newUpload.HasValue ? newUpload.Value : false;
            if (Session["data"] == null || discard) {
                Session.Clear();
                return View();
            }
            else {
                return RedirectToAction("PreviewData");
            }

        }
        /// <summary>
        /// Handle data upload 
        /// </summary>
        /// <param name="dataInput">Excel uploaded data</param>
        /// <param name="previewCount">Number of rows for previewing</param>
        /// <returns>View that allows user to preview their uploaded data</returns>
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase dataInput, int? previewCount) {
            if (dataInput == null) {
                ModelState.AddModelError("", "No data input. Please select a an excel file to upload");
                return View();
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
                var data = parseToDictionary(filePath);

                var rowCount = previewCount.HasValue ? previewCount.Value : 10;
                Session["data"] = new { data = data, rows = rowCount };//store in session
                return RedirectToAction("PreviewData", new { rows = rowCount });
            }
            catch (Exception ex) {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        public ActionResult PreviewData(int? rows) {
            int rowNum = rows.HasValue ? rows.Value : 10;
            if (Session["data"] != null) {
                var obj = Session["data"] as dynamic;
                var data = obj.data as IEnumerable<IDictionary<string, object>>;
                rowNum = (rowNum == -1 ? data.Count() : rowNum);
                var result = data.Take(rowNum);
                ViewBag.rows = rowNum;
                return View(result);
            }
            else
                return RedirectToAction("UploadFile");
        }
        

        #region Private function

        /*----------------Private functions-----------------*/
        private string parseToJson(string filePath) {
            var excel = new ExcelQueryFactory(filePath);
            var worksheets = excel.GetWorksheetNames();

            var allRows = from c in excel.Worksheet(worksheets.First())
                          select c;

            string jsonResult = "";
            var columnNames = allRows.First().ColumnNames;
            //convert to objects
            List<string> rowData = new List<string>();
            jsonResult += '[';

            foreach (var row in allRows) {
                var str = "{";
                List<string> valuePairs = new List<string>();
                foreach (var name in columnNames) {
                    var value = row[name];
                    valuePairs.Add('"' + name + "\" : \"" + value + '"');
                }
                str += string.Join(",", valuePairs);
                str += '}';
                rowData.Add(str);
            }
            jsonResult += string.Join(",", rowData);
            jsonResult += ']';
            //return Json
            return jsonResult;
        }

        private List<IDictionary<string, object>> parseToDictionary(string filePath) {
            var excel = new ExcelQueryFactory(filePath);
            var worksheets = excel.GetWorksheetNames();

            var allRows = from c in excel.Worksheet(worksheets.First())
                          select c;

            var columnNames = allRows.First().ColumnNames;
            var dataList = new List<IDictionary<string, object>>();
            foreach (var row in allRows) {
                IDictionary<string, object> dict = new Dictionary<string, object>();
                foreach (var name in columnNames) {
                    var value = row[name];
                    dict.Add(name, value);
                }
                dataList.Add(dict);
            }
            return dataList;
        }

        #endregion
    }
}
