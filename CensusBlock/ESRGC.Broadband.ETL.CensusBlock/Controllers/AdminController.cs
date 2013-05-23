using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using PagedList;
using System.Web.Security;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : BaseController
    {
        public AdminController(IUnitOfWork workUnit)
            : base(workUnit) {

        }
        //
        // GET: /Admin/
        public ActionResult Index() {
            
            ViewBag.lastUpdate = DateTime.Now;
            return View();
        }
        /// <summary>
        /// Ticket summary 
        /// </summary>
        /// <returns></returns>
        public ActionResult TicketOverview() {
            ViewBag.NumberOfTicket = _workUnit.TicketRepository.Entities.Count();
            ViewBag.ActiveTickets = _workUnit.TicketRepository.Entities.Where(x => x.Active).Count();
            ViewBag.ExpiredTickets = _workUnit.TicketRepository.Entities.Where(x => x.ExpirationDate < DateTime.Now).Count();

            return PartialView();
        }
        public ActionResult CreateTicket() {

            return View();
        }
        [HttpPost]
        public ActionResult CreateTicket(Ticket ticket) {
            return View();
        }
        public ActionResult SubmissionOverview() {
            ViewBag.Total = _workUnit.SubmissionRepository
                .Entities.Count();
            ViewBag.Initiated = _workUnit.SubmissionRepository
                .Entities
                .Where(x => x.Status == "Initiated")
                .Count();
            ViewBag.InProgress = _workUnit.SubmissionRepository
                .Entities
                .Where(x => x.Status == "Processing")
                .Count();
            ViewBag.Completed = _workUnit.SubmissionRepository
                .Entities
                .Where(x => x.Status == "Submitted")
                .Count();
            return PartialView();
        }

        public ActionResult UserOverview() {
            var users = Membership.GetAllUsers();
            ViewBag.Total = users.Count;
            ViewBag.Admins = Roles.GetUsersInRole("admin").Count();
            ViewBag.Users = Roles.GetUsersInRole("user").Count();
            ViewBag.Online = Membership.GetNumberOfUsersOnline();
            return PartialView();
        }
        
        public ActionResult Submission(
            int? page,
            int? pageSize,
            string status,
            string ticket,
            string user,
            DateTime? date) 
        {
            //get submission ordered by date submitted
            var submissions = _workUnit.SubmissionRepository
                .Entities
                .OrderBy(x => x.SubmissionTimeStarted)                
                .ToList();
            if (!string.IsNullOrEmpty(status)) {
                submissions = submissions
                    .Where(x => x.Status.ToUpper() == status.ToUpper())
                    .ToList();
                ViewBag.status = status;
            }
            //pagedlist
            var pageIndex = page ?? 1;
            var pSize = pageSize ?? 10;
            var pagedList = submissions.ToPagedList(pageIndex, pSize);

            return View(pagedList);
        }
        public ActionResult SubmissionInProgress() {
            var submissionInProgress = _workUnit.SubmissionRepository
                .Entities.Where(x => x.Status == "Processing")
                .ToList();

            return PartialView(submissionInProgress);
        }
        
        public ActionResult DownloadData(int id) {
            return View();
        }
    }
}
