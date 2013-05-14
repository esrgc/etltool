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
        public ActionResult UpdateStatus(int? submissionID) {
            if(submissionID == null)
                return Json(new { progress = -1 }, JsonRequestBehavior.AllowGet);

            var submission = _workUnit.SubmissionRepository.GetEntityByID(submissionID);
            if (submission != null) {
                //estimate time left in minute
                float est = -1;
                if (submission.SpeedPerSecond != 0 && submission.RecordsStored.HasValue) {
                    //in seconds
                    est = ((float)(submission.DataCount - submission.RecordsStored.Value) / submission.SpeedPerSecond);
                }
                var estimatedTime = "";// string.Format(@"<span class=""label label-important"">{0} ({1}%)</span> <br/>Time started: {2}. Last update: {3}. ",
                 //submission.Status,
                 //submission.ProgressPercentage,
                 //submission.SubmissionTimeStarted.Value.ToShortTimeString(),
                 //submission.LastStatusUpdate.HasValue ?
                 //    submission.LastStatusUpdate.Value.ToShortTimeString() : "N/A"
                 //);
                if (est != -1) {
                    if (est < 60)
                        estimatedTime += est.ToString("0") + " second(s)";
                    else if (est <= 3600)
                        estimatedTime += (est / 60).ToString("0") + " minute(s)";
                    else if (est > 3600)
                        estimatedTime += (est / 3600).ToString("0") + " hour(s)";
                }
                else
                    estimatedTime = "N/A";
                return Json(
                        new { 
                            estimatedTime = estimatedTime,
                            status = submission.Status,
                            timeStarted = submission.SubmissionTimeStarted.Value.ToShortTimeString(),
                            lastUpdate = submission.LastStatusUpdate.HasValue? 
                                         submission.LastStatusUpdate.Value.ToShortTimeString() : "N/A",
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
