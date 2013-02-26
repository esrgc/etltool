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
    public class UploadController : BaseController
    {
        public ActionResult uploadFile(HttpPostedFileBase dataInput, int? previewCount) {
            if (dataInput == null) {
                return Json(new { Status = "No valid data input" }, JsonRequestBehavior.AllowGet);
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
                //var json = parseToJson(filePath);
                //return Content(json, "application/json");
                var data = parseToDictionary(filePath);
                if (previewCount.HasValue) { 
                    var result = data.Take(previewCount.Value);
                    return Content(result.ToJSon(), "application/json");
                }
                else return Content(data.ToJSon(), "application/json");
            }
            catch (Exception ex) {
                return Json(new { status = "Failed", message = ex.Message });
            }
        }

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
    }
}
