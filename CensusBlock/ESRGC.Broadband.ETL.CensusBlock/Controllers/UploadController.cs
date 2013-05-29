using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToExcel;
using System.IO;
using ESRGC.Broadband.ETL.CensusBlock.Extension;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    [Authorize]
    public class UploadController : BaseController
    {
        public UploadController(IUnitOfWork workUnit)
            : base(workUnit) {

        }

        /// <summary>
        /// provides upload form
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFile(bool? newUpload) {
            bool discard = newUpload.HasValue ? newUpload.Value : false;
            if (Session[uploadKey] == null || discard) {
                Session[uploadKey] = null;

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
                ModelState.AddModelError("", "No data input. Please select a file to upload");
                return View();
            }

            var binaryData = new byte[dataInput.ContentLength];
            //read the data
            dataInput.InputStream.Read(binaryData, 0, dataInput.ContentLength);

            try {
                List<IDictionary<string, object>> data = null;
                if (dataInput.FileName.Contains(".xls") || dataInput.FileName.Contains(".xlsx")) {
                    //save to file
                    var filePath = HttpContext.Server.MapPath("~/") + "uploadFile";
                    FileInfo f = new FileInfo(filePath);
                    if (f.Exists)//delete if already exists
                        f.Delete();
                    //open file and overwrite if exists
                    using (var fileStream = f.Open(FileMode.CreateNew, FileAccess.Write, FileShare.None)) {
                        fileStream.Write(binaryData, 0, dataInput.ContentLength);
                        fileStream.Close();
                    }
                    //read excel content
                    data = Helpers.parseToDictionary(filePath);
                }
                else if (dataInput.FileName.Contains(".csv") || dataInput.FileName.Contains(".txt")) {
                    data = Helpers.parseToDictionary(dataInput.InputStream);
                }
                else {
                    throw new Exception("The uploaded file format isn't supported. Please upload a csv, text or an excel file.");
                }
                if (data == null)
                    throw new Exception("An error occured processing the upload file.");
                var rowCount = previewCount.HasValue ? previewCount.Value : 10;
                Session[uploadKey] = new { data = data, rows = rowCount };//store in session
                //update submission status

                return RedirectToAction("PreviewData", new { rows = rowCount });
            }
            catch (Exception ex) {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        public ActionResult PreviewData(int? rows) {
            int rowNum = rows.HasValue ? rows.Value : 10;
            if (Session[uploadKey] != null) {
                var obj = Session[uploadKey] as dynamic;
                var data = obj.data as IEnumerable<IDictionary<string, object>>;
                rowNum = (rowNum == -1 ? data.Count() : rowNum);
                var result = data.Take(rowNum);
                ViewBag.previewRows = rowNum;
                ViewBag.dataCount = data.Count();
                return View(result);
            }
            else
                return RedirectToAction("UploadFile");
        }


        #region Private function

        #endregion
    }
}
