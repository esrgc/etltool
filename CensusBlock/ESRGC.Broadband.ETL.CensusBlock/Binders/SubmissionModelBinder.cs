using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;

namespace ESRGC.Broadband.ETL.CensusBlock.Binders
{
    public class SubmissionModelBinder : IModelBinder
    {
        const string _sessionKey = "submissionKey";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            //get submission from session
            var submission = (Submission)controllerContext.HttpContext.Session[_sessionKey];
            if (submission == null) {
                submission = new Submission() {
                    Status = "Initiated",
                    SubmittingUser = controllerContext.HttpContext.User.Identity.Name
                };
                controllerContext.HttpContext.Session[_sessionKey] = submission;
            }
            return submission;
        }
    }
}