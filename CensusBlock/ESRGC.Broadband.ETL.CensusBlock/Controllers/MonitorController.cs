using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    public class MonitorController : BaseController
    {
        public MonitorController(IUnitOfWork workUnit) : base(workUnit) { }
        public ActionResult UpdateStatus(int submissionID) {
            var submission = _workUnit.SubmissionRepository.GetEntityByID(submissionID);
            if (submission != null) {
                var message = string.Format(@"Status: {0} ({1}%), Time started: {2}, Last update: {3}", 
                submission.Status,
                submission.ProgressPercentage,
                submission.SubmissionTimeStarted.Value.ToShortTimeString(),
                submission.LastStatusUpdate.HasValue? 
                    submission.LastStatusUpdate.Value.ToShortTimeString() : "N/A"
                );
                return Json(
                        new { 
                            message = message,
                            progress = submission.ProgressPercentage
                        }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { progress = -1 }, JsonRequestBehavior.AllowGet);
        }
        //public void UpdateStatusAsync() {
        //    AsyncManager.OutstandingOperations.Increment();
        //    BackgroundWorker worker = new BackgroundWorker();
            
        //    worker.DoWork += (o, e) => {
        //        var w = o as BackgroundWorker;
                
        //        if (Session["status"] != null) {
        //            lock (Session["status"]) {
        //                if (Session["status"] != null) {
        //                    e.Result = Session["status"] as dynamic;
        //                }
        //            }
        //        }
        //    };
            
        //    worker.RunWorkerCompleted += (o, e) => {
        //        AsyncManager.Sync(() => {
        //            var status = e.Result as dynamic;
        //            AsyncManager.Parameters["status"] = status;
        //            AsyncManager.OutstandingOperations.Decrement();
        //        });
        //    };
        //    worker.RunWorkerAsync();
        //}
        //public ActionResult UpdateStatusCompleted(object status) {
        //    if (status != null)
        //        return Json(status, JsonRequestBehavior.AllowGet);
        //    else//no operation running
        //        return Json(
        //            new { progress = -1 },
        //            JsonRequestBehavior.AllowGet
        //        );
        //}
    }
}
