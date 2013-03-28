using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.Broadband.ETL.CensusBlock.Models;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    public class DataProcessController : BaseController
    {
        public DataProcessController(IUnitOfWork workUnit) : base(workUnit) { }

        public ActionResult MapData() {
            if (Session["data"] != null) {
                var obj = Session["data"] as dynamic;
                var data = obj.data as IEnumerable<IDictionary<string, object>>;

                var model = new DataMappingModel() {
                    MappingObject = new ColumnMapping(),
                    UploadDataColumns = data.First().Keys
                };

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
            DateTime timeSubmitted = DateTime.Now;
            if (ModelState.IsValid) {
                ModelState.Clear();//clear model state to validate transfer data
                int count = 1;//start at line 1
                foreach (var entry in data) {
                    short tempShort;
                    object tempValue;
                    try {

                        #region data transfer
                        var dataEntry = new ServiceCensusBlock();

                        tempValue = entry[columns.PPROVNAMEColumn];
                        dataEntry.PROVNAME = tempValue != null ? tempValue.ToString() : string.Empty;

                        tempValue = entry[columns.DBANAMEColumn];
                        dataEntry.DBANAME = tempValue != null ? tempValue.ToString() : dataEntry.DBANAME;

                        dataEntry.Provider_Type =
                            short.TryParse(entry[columns.Provider_typeColumn].ToString(), out tempShort) ?
                            tempShort : (short)-9999;

                        tempValue = entry[columns.FRNColumn];
                        dataEntry.FRN = tempValue != null ? tempValue.ToString() : dataEntry.FRN;

                        tempValue = entry[columns.STATEFIPSColumn];
                        dataEntry.STATEFIPS = tempValue != null ? tempValue.ToString() : string.Empty;

                        tempValue = entry[columns.COUNTYFIPSColumn];
                        dataEntry.COUNTYFIPS = tempValue != null ? tempValue.ToString() : string.Empty;

                        tempValue = entry[columns.TRACTColumn];
                        dataEntry.TRACT = tempValue != null ? tempValue.ToString() : string.Empty;

                        tempValue = entry[columns.BLOCKIDColumn];
                        dataEntry.BLOCKID = tempValue != null ? tempValue.ToString() : string.Empty;

                        tempValue = entry[columns.BLOCKSUBGROUPColumn];
                        dataEntry.BLOCKSUBGROUP = tempValue != null ? tempValue.ToString() : string.Empty;

                        tempValue = entry[columns.FULLFIPSIDColumn];
                        dataEntry.FULLFIPSID = tempValue != null ? tempValue.ToString() : dataEntry.FULLFIPSID;

                        dataEntry.TRANSTECH =
                            short.TryParse(entry[columns.TRANSTECHColumn].ToString(), out tempShort) ?
                            tempShort : dataEntry.TRANSTECH;

                        tempValue = entry[columns.MAXADDOWNColumn];
                        dataEntry.MAXADDOWN = tempValue != null ? tempValue.ToString() : dataEntry.MAXADDOWN;

                        tempValue = entry[columns.MAXADUPColumn];
                        dataEntry.MAXADUP = tempValue != null ? tempValue.ToString() : dataEntry.MAXADUP;

                        tempValue = entry[columns.TYPICDOWNColumn];
                        dataEntry.TYPICDOWN = tempValue != null ? tempValue.ToString() : dataEntry.TYPICDOWN;

                        tempValue = entry[columns.TYPICUPColumn];
                        dataEntry.TYPICUP = tempValue != null ? tempValue.ToString() : dataEntry.TYPICUP;

                        dataEntry.TimeStamp = timeSubmitted; 
                        #endregion

                        //validate data for each entry
                        TryValidateModel(dataEntry);
                        if (ModelState.IsValid) {
                            dataList.Add(dataEntry);
                            _workUnit.ServiceCensusRepository.InsertEntity(dataEntry);
                        }
                        else {
                            //redirect to report errors
                            errorList.Add(count, dataEntry);                            
                        }
                        
                    }
                    catch (Exception ex) {
                        ModelState.AddModelError("", "An error has occured. " + ex.Message);
                        return View("Error");
                    }
                    count++;//increase count
                }
                _workUnit.SaveChanges();//push data to database
                return RedirectToAction("PreviewMapping");
            }
            //error has occured
            else {
                return View();
            }
        }

        public ActionResult PreviewMapping(IEnumerable<ServiceCensusBlock> data) {
            return View();
        }
    }
}
