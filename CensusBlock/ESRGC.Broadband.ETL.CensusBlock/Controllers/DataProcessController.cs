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
using System.ComponentModel;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    public class DataProcessController : BaseController
    {
        public DataProcessController(IUnitOfWork workUnit) : base(workUnit) { }

        public ActionResult MapData(int? submissionID) {

            if (Session["data"] != null) {
                var obj = Session["data"] as dynamic;
                var data = obj.data as IEnumerable<IDictionary<string, object>>;
                DataMappingModel model = new DataMappingModel();
                List<string> uploadedColumns = data.First().Keys.ToList();
                //add 'use default' value to apply default values
                uploadedColumns.Add("Use default");
                model.UploadDataColumns = uploadedColumns;
                //check for existing mapping object
                if (Session["mappingData"] != null) {
                    var dynObj = Session["mappingData"] as dynamic;
                    var mappingObject = dynObj.mappingObject as ColumnMapping;
                    model.MappingObject = mappingObject;
                    model.DefaultData = dynObj.defaultData as ServiceCensusBlock;
                }
                else {
                    model.MappingObject = new ColumnMapping();
                    model.DefaultData = new ServiceCensusBlock();
                }
                //data first row
                ViewBag.firstRowData = data.First().ToJSon();
                ViewBag.submissionID = submissionID;
                return View(model);
            }
            else {
                updateStatusMessage("Your session has expired. Please upload a new file.");
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult MapData(DataMappingModel mappingModel, int? submissionID) {
            if (Session["data"] == null) {
                updateStatusMessage("Your session has expired. Please upload a new file.");
                return RedirectToAction("Index", "Home");
            }
            //get mappnig columns
            var columns = mappingModel.MappingObject;
            var defaultData = mappingModel.DefaultData;
            //get upload data
            var obj = Session["data"] as dynamic;
            var data = obj.data as IEnumerable<IDictionary<string, object>>;
            //data to be stored
            IList<ServiceCensusBlock> dataList = new List<ServiceCensusBlock>();
            IDictionary<int, object> errorList = new Dictionary<int, object>();
            //by pass DefaultData validation
            var defaultDataModelStates = ModelState.Where(x => x.Key.Contains("DefaultData")).ToList();
            foreach (var state in defaultDataModelStates) {
                ModelState.Remove(state.Key);//remove these DefaultData states
            }
            //now that the model is valid we can start mapping data
            if (ModelState.IsValid) {
                int count = 1;//start at line 1
                foreach (var entry in data) {
                    short tempShort;
                    object tempValue;
                    string str, key, useDefault = "Use default";
                    try {
                        ModelState.Clear();//clear model state to validate transfer data
                        #region data transfer
                        var dataEntry = new ServiceCensusBlock();
                        //PROVIDER NAME
                        key = columns.PROVNAMEColumn;
                        tempValue = key == useDefault? defaultData.PROVNAME :  entry[key];                        
                        dataEntry.PROVNAME = tempValue != null ? tempValue.ToString() : string.Empty;

                        key = columns.DBANAMEColumn;
                        tempValue = key == useDefault ? defaultData.DBANAME : entry[key];
                        dataEntry.DBANAME = tempValue != null ? tempValue.ToString() : dataEntry.DBANAME;

                        key = columns.Provider_typeColumn;
                        tempValue = key == useDefault ? defaultData.Provider_Type : entry[key];
                        str = tempValue != null ? tempValue.ToString() : "";
                        dataEntry.Provider_Type =
                            short.TryParse(str, out tempShort) ?
                            tempShort : (short)-9999;

                        key = columns.FRNColumn;
                        tempValue = key == useDefault ? defaultData.FRN : entry[key];
                        dataEntry.FRN = tempValue != null ? tempValue.ToString().Replace("-", "") : dataEntry.FRN;

                        key = columns.FULLFIPSIDColumn;
                        tempValue = key == useDefault ? defaultData.FULLFIPSID : entry[key];
                        dataEntry.FULLFIPSID = tempValue != null ? tempValue.ToString() : dataEntry.FULLFIPSID;

                        key = columns.TRANSTECHColumn;
                        tempValue = key == useDefault ? defaultData.TRANSTECH : entry[key];
                        str = tempValue != null? tempValue.ToString(): "";
                        dataEntry.TRANSTECH =
                            short.TryParse(str, out tempShort) ?
                            tempShort : dataEntry.TRANSTECH;

                        key = columns.MAXADDOWNColumn;
                        tempValue = key == useDefault ? defaultData.MAXADDOWN : entry[key];
                        dataEntry.MAXADDOWN = tempValue != null ? tempValue.ToString() : dataEntry.MAXADDOWN;

                        key = columns.MAXADUPColumn;
                        tempValue = key == useDefault ? defaultData.MAXADUP : entry[key];
                        dataEntry.MAXADUP = tempValue != null ? tempValue.ToString() : dataEntry.MAXADUP;

                        try {
                            key = columns.TYPICDOWNColumn;
                            tempValue = key == useDefault ? defaultData.TYPICDOWN : entry[key];
                            dataEntry.TYPICDOWN = tempValue != null ? tempValue.ToString() : dataEntry.TYPICDOWN;
                        }
                        catch {
                            //reserve default value if error occurs
                        }

                        try {
                            key = columns.TYPICUPColumn;
                            tempValue = key == useDefault ? defaultData.TYPICUP : entry[key];
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
                //Session["data"] = null;//discard session data
                //store data to be committed to session
                Session["mappingData"] = new {
                    mappingObject = columns,
                    validData = dataList,
                    defaultData = defaultData
                };
                //initiate submission if there's no error
                Submission submission;
                if (submissionID.HasValue) {
                    submission = _workUnit.SubmissionRepository.GetEntityByID(submissionID);
                }
                else {
                    submission = new Submission() {
                        Status = "Initiated",
                        DataCount = dataList.Count,
                        RecordsStored = 0,
                        ProgressPercentage = 0
                    };
                    _workUnit.SubmissionRepository.InsertEntity(submission);
                    _workUnit.SaveChanges();//save initiated submission
                }
                //prepare view model
                var previewData = new PreviewMappingModel();
                previewData.SuccessCount = dataList.Count();
                previewData.ErrorList = errorList;
                previewData.Data = dataList;
                previewData.SubmissionID = submission.SubmissionID;
                return View("ReviewMapping", previewData);
            }
            //error has occured
            else {
                //re-populate view data
                List<string> uploadedColumns = data.First().Keys.ToList();
                //add 'use default' value to apply default values
                uploadedColumns.Add("Use default");
                mappingModel.UploadDataColumns = uploadedColumns;
                //data first row
                ViewBag.firstRowData = data.First().ToJSon();
                return View(mappingModel);
            }
        }

        [HttpPost]
        [NoAsyncTimeout]  
        public void CommitDataAsync(int submissionID) {
            //discard uploaded file
            Session["data"] = null;
            //check for mapped data
            if (Session["mappingData"] == null)
                return;

            var dynObj = Session["mappingData"] as dynamic;
            var data = dynObj.validData as List<ServiceCensusBlock>;

            AsyncManager.OutstandingOperations.Increment();//notify async operation
            //initialize data commit service
            var service = new DataCommit(submissionID);
            //BackgroundWorker worker = new BackgroundWorker();
            //status report handler
            //service.DataCommitProgressChanged += (e) => {
                //AsyncManager.Sync(() => {
                //    var eventArg = e as OperationProgressChangedEventArgs;
                //    var message = 
                //        "Progress: " +
                //        eventArg.ProgressPercentage +
                //        "%. " + 
                //        eventArg.Status ;

                //    Session["status"] = new {
                //        message = message,
                //        progress = eventArg.ProgressPercentage
                //    };
                //});
            //};
            //completed handler
            service.DataCommitCompleted += (o, e) => {
                AsyncManager.Sync(() => {
                    try {
                        AsyncManager.Parameters["result"] = e.Data;
                        AsyncManager.Parameters["status"] = "success";
                        Session.Clear();
                    }
                    catch (Exception ex) {
                        AsyncManager.Parameters["data"] = null;
                        AsyncManager.Parameters["status"] = ex.ToString();
                        Session.Clear();                        
                    }
                    AsyncManager.OutstandingOperations.Decrement();
                });
            };
            service.CommitDataAsync(data, Guid.NewGuid());
        }

        public ActionResult CommitDataCompleted(Submission result, string status) {
            ViewBag.Status = status;
            if (result != null)
                return View(result);
            else {
                ViewBag.errorMsg = "Error submitting your data. Please try again";
                return View("Error");
            }
        }
        
    }
}
