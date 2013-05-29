﻿using System;
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
            //submission is auto created in model binder            
            submission.TicketID = ticketId;
            if (submission.Status == "Initiated")
                insertSubmission(submission);//store to database
            return RedirectToAction("UploadFile", "Upload");
        }

        //
        // POST: /Submission/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection) {
            try {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }

        //
        // GET: /Submission/Edit/5

        public ActionResult Edit(int id) {
            return View();
        }

        //
        // POST: /Submission/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection) {
            try {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }

        //
        // GET: /Submission/Delete/5
        //cancel and delete current submission
        public ActionResult Delete(Submission submission) {
            if (submission != null) {
                _workUnit.SubmissionRepository.DeleteEntity(submission);
                _workUnit.SaveChanges();
                Session.Clear();
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /Submission/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection) {
            try {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }
    }
}