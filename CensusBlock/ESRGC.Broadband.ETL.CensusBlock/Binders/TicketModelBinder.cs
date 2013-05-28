using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;

namespace ESRGC.Broadband.ETL.CensusBlock.Binders
{
    public class TicketModelBinder: IModelBinder
    {
        private const string _ticketSessionKey = "submissionTicket";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            //get ticket from session
            Ticket ticket = (Ticket)controllerContext.HttpContext.Session[_ticketSessionKey];
            return ticket;
        }
    }
}