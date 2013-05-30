using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using PagedList;
using System.Web.Security;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;
using System.Web.Routing;
using System.Linq.Expressions;
using ESRGC.Broadband.ETL.CensusBlock.Extension;
using System.IO;

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
            ViewBag.ActiveTickets = _workUnit.TicketRepository.Entities.ToList().Where(x => x.Active).Count();
            ViewBag.ExpiredTickets = _workUnit.TicketRepository.Entities.Where(x => x.ExpirationDate < DateTime.Now).Count();

            return PartialView();
        }
        public ActionResult CreateTicket() {
            return View(new Ticket());
        }
        [HttpPost]
        public ActionResult CreateTicket(Ticket ticket) {
            if (ModelState.IsValid) {
                _workUnit.TicketRepository.InsertEntity(ticket);
                _workUnit.SaveChanges();

                updateStatusMessage("A new ticket has been created.");
                return RedirectToAction("Ticket");
            }
            //error
            return View(ticket);
        }
        //ticket manager
        public ActionResult Ticket(int? page, int? pageSize) {
            var tickets = _workUnit.TicketRepository
                .Entities
                .OrderBy(x => x.IssuedDate)
                .ToList();
            //pagedlist
            var pageIndex = page ?? 1;
            var pSize = pageSize ?? 10;
            var pagedList = tickets.ToPagedList(pageIndex, pSize);
            return View(pagedList);
        }
        public ActionResult TicketDetail(int id) {
            var ticket = _workUnit.TicketRepository.GetEntityByID(id);
            return View(ticket);
        }
        public ActionResult EditTicket(int id) {
            var ticket = _workUnit.TicketRepository.GetEntityByID(id);
            return View(ticket);
        }
        [HttpPost]
        public ActionResult EditTicket(int id, FormCollection values) {
            var ticket = _workUnit.TicketRepository.GetEntityByID(id);
            TryUpdateModel(ticket);
            if (ModelState.IsValid) {
                return RedirectToAction("Ticket");
            }
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
            DateTime? date) {
            IDictionary<string, object> filters = new Dictionary<string, object>();
            //get submission ordered by date submitted
            var submissions = _workUnit.SubmissionRepository
                .Entities
                .OrderBy(x => x.SubmissionTimeStarted)
                .ToList();
            //filter
            if (!string.IsNullOrEmpty(status)) {
                if (status == "Incompleted") {
                    submissions = submissions
                        .Where(x => (
                             x.Status == "Initiated" ||
                             x.Status == "Uploaded" ||
                             x.Status == "Uploading" ||
                             x.Status == "Ready"
                         )).ToList();
                }
                else {
                    submissions = submissions
                        .Where(x => x.Status.ToUpper() == status.ToUpper())
                        .ToList();
                }
                filters.Add("status", status);
            }
            if (!string.IsNullOrEmpty(ticket)) {
                submissions = submissions
                    .Where(x => x.Ticket.Name.ToUpper() == ticket.ToUpper())
                    .ToList();
                filters.Add("ticket", ticket);
            }
            if (!string.IsNullOrEmpty(user)) {
                submissions = submissions
                    .Where(x => x.SubmittingUser.ToUpper() == user.ToUpper())
                    .ToList();
                filters.Add("user", user);
            }
            if (date != null) {
                submissions = submissions
                    .Where(x => {
                        if (x.SubmissionTimeStarted != null) {
                            return (x.SubmissionTimeStarted.Value.Date == date.Value.Date);
                        }
                        return false;
                    })
                    .ToList();
                filters.Add("date", date.Value.Date);
            }
            ViewBag.filters = filters;
            //pagedlist
            var pageIndex = page ?? 1;
            var pSize = pageSize ?? 10;
            var pagedList = submissions.ToPagedList(pageIndex, pSize);

            return View(pagedList);
        }
        public ActionResult SubmissionDetail(int id) {
            var submission = _workUnit
                .SubmissionRepository
                .GetEntityByID(id);
            return View(submission);
        }
        public ActionResult DeleteSubmission(int id) {
            deleteSubmission(id);
            return RedirectToAction("Submission");
        }
        public ActionResult RecentSubmission() {
            var completedSubmissions = _workUnit.SubmissionRepository
                .Entities.Where(x => (x.Status == "Submitted"))
                .OrderByDescending(x => x.SubmissionTimeCompleted).ToList();
                
            var recentSubmissions = completedSubmissions.Where(x => {
                if (x.SubmissionTimeCompleted.HasValue) {
                    var mostRecent = x.SubmissionTimeCompleted.Value;
                    return mostRecent > mostRecent.AddDays(-3);
                }
                return false;
            }).ToList();
            ViewBag.total = recentSubmissions.Count;
            return PartialView(recentSubmissions);

        }
        public ActionResult SubmissionInProgress() {
            var submissionInProgress = _workUnit.SubmissionRepository
                .Entities.Where(x => x.Status == "Processing");
                

            return PartialView(submissionInProgress);
        }

        public ActionResult DownloadData(int id) {
            var submission = getSubmission(id);
            var serviceData = submission.ServiceCensusBlocks.ToList();
            var csvStream = (MemoryStream)Helpers.writeToCsv<ServiceCensusBlock>(serviceData);
            byte[] result = csvStream.GetBuffer();
            return File(result,"text/csv", submission.SubmissionID + ".csv");
        }
    }
}
