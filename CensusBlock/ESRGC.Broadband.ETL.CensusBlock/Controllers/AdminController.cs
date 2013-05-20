using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using PagedList;
using System.Web.Security;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    [Authorize(Roles="admin")]
    public class AdminController : BaseController
    {
        public AdminController(IUnitOfWork workUnit) : base(workUnit) { 
        
        }
        //
        // GET: /Admin/

        public ActionResult Index(int? page, int? pageSize)
        {
            //get submission ordered by date submitted
            var submissions = _workUnit.SubmissionRepository.
                Entities.
                OrderBy(x=>x.SubmissionTimeCompleted).
                ToList();
            //pagedlist
            var pageIndex = page ?? 1;
            var pSize = pageSize ?? 10;
            var pageList = submissions.ToPagedList(pageIndex, pSize);
            
            return View(pageList);
        }

        public ActionResult Detail(int id) {
            return View();
        }

        public ActionResult ExtractData(int id) {
            return View();
        }
    }
}
