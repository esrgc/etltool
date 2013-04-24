using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.Broadband.ETL.CensusBlock.Models;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using ESRGC.Broadband.ETL.CensusBlock.Extension;
using ESRGC.Broadband.ETL.CensusBlock.Services;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    public class DataProcessController : BaseController
    {
        public DataProcessController(IUnitOfWork workUnit) : base(workUnit) { }

        public ActionResult MapData() {

            if (Session["data"] != null) {
                var obj = Session["data"] as dynamic;
                var data = obj.data as IEnumerable<IDictionary<string, object>>;
                DataMappingModel model = new DataMappingModel();
                model.UploadDataColumns = data.First().Keys;
                //check for existing mapping object
                if (Session["mappingData"] != null) {
                    var dynObj = Session["mappingData"] as dynamic;
                    var mappingObject = dynObj.mappingObject as ColumnMapping;
                    model.MappingObject = mappingObject;
                }
                else
                    model.MappingObject = new ColumnMapping();
                //data first row
                ViewBag.firstRowData = data.First().ToJSon();
                return View(model);
            }
            else {
                updateStatusMessage("Your session has expired. Please upload a new file.");
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult MapData(DataMappingModel mappingModel) {
            if (Session["data"] == null) {
                updateStatusMessage("Your session has expired. Please upload a new file.");
                return RedirectToAction("Index", "Home");
            }
            //get mappnig columns
            var columns = mappingModel.MappingObject;
            //get upload data
            var obj = Session["data"] as dynamic;
            var data = obj.data as IEnumerable<IDictionary<string, object>>;
            //data to be stored
            IList<ServiceCensusBlock> dataList = new List<ServiceCensusBlock>();
            IDictionary<int, object> errorList = new Dictionary<int, object>();
            if (ModelState.IsValid) {
                int count = 1;//start at line 1
                foreach (var entry in data) {
                    short tempShort;
                    object tempValue;
                    try {
                        ModelState.Clear();//clear model state to validate transfer data
                        #region data transfer
                        var dataEntry = new ServiceCensusBlock();

                        tempValue = entry[columns.PROVNAMEColumn];
                        dataEntry.PROVNAME = tempValue != null ? tempValue.ToString() : string.Empty;

                        tempValue = entry[columns.DBANAMEColumn];
                        dataEntry.DBANAME = tempValue != null ? tempValue.ToString() : dataEntry.DBANAME;

                        dataEntry.Provider_Type =
                            short.TryParse(entry[columns.Provider_typeColumn].ToString(), out tempShort) ?
                            tempShort : (short)-9999;

                        tempValue = entry[columns.FRNColumn];
                        dataEntry.FRN = tempValue != null ? tempValue.ToString().Replace("-", "") : dataEntry.FRN;

                        tempValue = entry[columns.FULLFIPSIDColumn];
                        dataEntry.FULLFIPSID = tempValue != null ? tempValue.ToString() : dataEntry.FULLFIPSID;

                        dataEntry.TRANSTECH =
                            short.TryParse(entry[columns.TRANSTECHColumn].ToString(), out tempShort) ?
                            tempShort : dataEntry.TRANSTECH;

                        tempValue = entry[columns.MAXADDOWNColumn];
                        dataEntry.MAXADDOWN = tempValue != null ? tempValue.ToString() : dataEntry.MAXADDOWN;

                        tempValue = entry[columns.MAXADUPColumn];
                        dataEntry.MAXADUP = tempValue != null ? tempValue.ToString() : dataEntry.MAXADUP;

                        try {
                            tempValue = entry[columns.TYPICDOWNColumn];
                            dataEntry.TYPICDOWN = tempValue != null ? tempValue.ToString() : dataEntry.TYPICDOWN;
                        }
                        catch {
                            //reserve default value if error occurs
                        }

                        try {
                            tempValue = entry[columns.TYPICUPColumn];
                            dataEntry.TYPICUP = tempValue != null ? tempValue.ToString() : dataEntry.TYPICUP;
                        }
                        catch {
                            //reserve default value if error occurs
                        }

                        #endregion

                        //validate data for each entry
                        TryValidateModel(dataEntry);
                        if (ModelState.IsValid) {
                            dataList.Add(dataEntry);
                            //_workUnit.ServiceCensusRepository.InsertEntity(dataEntry);
                        }
                        else {
                            //redirect to report errors
                            errorList.Add(count, ModelState.ToList());
                        }
                    }
                    catch (Exception ex) {
                        ViewBag.errorMsg = "Error processing submitted data. " + ex.Message;
                        return View("Error");
                    }
                    count++;//increase count
                }
                //_workUnit.SaveChanges();//push data to database
                //Session["data"] = null;//discard session data
                //store data to be committed to session
                Session["mappingData"] = new {
                    mappingObject = columns,
                    validData = dataList
                };

                var previewData = new PreviewMappingModel();
                previewData.SuccessCount = dataList.Count();
                previewData.ErrorList = errorList;
                previewData.Data = dataList;
                return View("PreviewMapping", previewData);
            }
            //error has occured
            else {
                return View(mappingModel);
            }
        }

        [NoAsyncTimeout]
        [HttpPost]
        public void CommitDataAsync() {
            //discard uploaded file
            Session["data"] = null;
            //check for mapped data
            if (Session["mappingData"] == null)
                return;

            var dynObj = Session["mappingData"] as dynamic;
            var data = dynObj.validData as List<ServiceCensusBlock>;

            AsyncManager.OutstandingOperations.Increment();//notify async operation
            var service = new DataCommit(_workUnit);

            //status report handler
            service.DataCommitProgressChanged += (e) => {
                var eventArg = e as OperationProgressChangedEventArgs;
                var message = eventArg.Status +
                    ". Progress: " +
                    eventArg.ProgressPercentage +
                    "%";

                Session["status"] = new {
                    message = message,
                    progress = eventArg.ProgressPercentage
                };

            };
            //completed handler
            service.DataCommitCompleted += (o, e) => {
                try {
                    AsyncManager.Parameters["result"] = e.Data;
                    AsyncManager.Parameters["status"] = "success";
                    Session.Clear();
                    Session["status"] = new {
                        message = "Finished successfully.",
                        progress = 100
                    };
                }
                catch (Exception ex) {
                    AsyncManager.Parameters["data"] = null;
                    AsyncManager.Parameters["status"] = ex.ToString();
                    Session.Clear();
                    Session["status"] = new {
                        message = "Finished with exceptions.",
                        progress = 100
                    };
                }
                AsyncManager.OutstandingOperations.Decrement();
            };
            service.CommitDataAsync(data, Guid.NewGuid());
        }

        public ActionResult CommitDataCompleted(Submission result, string status) {
            ViewBag.Status = status;
            return View(result);
        }
        public ActionResult UpdateStatus() {
            if (Session["status"] != null) {
                lock (Session["status"]) {
                    var status = (Session["status"] as dynamic);
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
            }
            else//no operation running
                return Json(
                    new { progress = -1 },
                    JsonRequestBehavior.AllowGet
                );
        }
    }
}
