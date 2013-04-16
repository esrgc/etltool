using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;

namespace ESRGC.Broadband.ETL.CensusBlock.Controllers
{
    public class BaseController : AsyncController
    {
        protected IUnitOfWork _workUnit; 
        public BaseController(IUnitOfWork workUnit) {
            _workUnit = workUnit;
        }
        public void updateStatusMessage(string message) {
            TempData["message"] = message;
        }
    }
}
