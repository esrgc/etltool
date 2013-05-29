using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;
using System.Linq.Expressions;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    [Authorize]
    public class BaseController : AsyncController
    {
        protected const string uploadKey = "data";
        protected const string mappingKey = "mappingData";
        protected const string ticketKey = "submissionTicket";
        protected const string submissionKey = "submission";

        protected IUnitOfWork _workUnit;
        public BaseController(IUnitOfWork workUnit) {
            _workUnit = workUnit;
        }
        public BaseController() {

        }
        public void updateStatusMessage(string message) {
            TempData["message"] = message;
        }
        protected Submission getSubmission(int id) {
            return _workUnit.SubmissionRepository.GetEntityByID(id);
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
        protected void deleteSubmission() {
            var unfinishedSubmissions =_workUnit.SubmissionRepository
                .Entities
                .Where(x => (x.Status == "Initiated" ||
                    x.Status == "Uploaded" ||
                    x.Status == "Uploading" ||
                    x.Status == "Ready"));
            foreach (var s in unfinishedSubmissions) {
                _workUnit.SubmissionRepository.DeleteEntity(s);
            }
            _workUnit.SaveChanges();
                
        }
        protected void deleteSubmission(Expression<Func<Submission, bool>> criteria) {
            var unfinishedSubmissions =_workUnit.SubmissionRepository
                .Entities
                .Where(criteria);
            foreach (var s in unfinishedSubmissions) {
                _workUnit.SubmissionRepository.DeleteEntity(s);
            }
            _workUnit.SaveChanges();
                
        }
        
    }
}
