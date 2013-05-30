using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    public class SubmissionController : BaseController
    {
        public SubmissionController(IUnitOfWork workUnit) : base(workUnit) { }
        //
        // GET: /Submission/

        public ActionResult Index() {
            var tickets = _workUnit.TicketRepository
                .Entities
                .Include(x => x.Submissions)
                .ToList() //load fresh data from database
                .Where(x => x.Active)
                .ToList(); //cast to list type

            return View(tickets);
        }

        //
        // GET: /Submission/Details/5

        public ActionResult Details(int id) {
            return View();
        }

        //
        // GET: /Submission/Create
        //create submission
        public ActionResult Create(int ticketId, Submission submission) {
            submission.TicketID = ticketId;
            return RedirectToAction("UploadFile", "Upload");
        }

        //
        // GET: /Submission/Delete/5
        //cancel and delete current submission
        public ActionResult Delete(int submissionId) {
            deleteSubmission(submissionId);
            return RedirectToAction("Index");
        }

    }
}
