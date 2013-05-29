using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    [Authorize]
    public class BaseController : AsyncController
    {
        protected const string uploadKey = "data";
        protected const string mappingKey = "mappingData";
        protected const string ticketKey = "submissionTicket";

        protected IUnitOfWork _workUnit;
        public BaseController(IUnitOfWork workUnit) {
            _workUnit = workUnit;
        }
        public BaseController() {

        }
        public void updateStatusMessage(string message) {
            TempData["message"] = message;
        }
        protected void updateSubmission(Submission submission) {
            if (submission != null) {
                submission.LastStatusUpdate = DateTime.Now;
                _workUnit.SubmissionRepository.UpdateEntity(submission);
                _workUnit.SaveChanges();
            }
        }
        protected void insertSubmission(Submission submission) {
            if (submission != null) {
                submission.LastStatusUpdate = DateTime.Now;
                //store submission to database
                _workUnit.SubmissionRepository.InsertEntity(submission);
                _workUnit.SaveChanges();//update submission status 
            }
        }
    }
}
