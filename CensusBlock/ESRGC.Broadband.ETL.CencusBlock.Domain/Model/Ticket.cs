using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.Model
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime IssueDate { get; set; }
        public int Duration { get; set; }
        public DateTime ExpirationDate { get; set; }

        public virtual IEnumerable<Submission> Submissions { get; set; }
    }
}
