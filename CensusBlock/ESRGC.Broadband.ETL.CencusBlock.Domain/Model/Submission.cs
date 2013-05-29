using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.Model
{
    public class Submission
    {
        public Submission() {
            SubmissionTimeStarted = DateTime.Now;
            SubmissionID = 0;
        }
        [Key]
        public int SubmissionID { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        public DateTime? SubmissionTimeStarted { get; set; }
        public DateTime? SubmissionTimeCompleted { get; set; }
        public DateTime? LastStatusUpdate { get; set; }
        public int? RecordsStored {get; set;}
        public int DataCount { get; set; }
        public int ProgressPercentage { get; set; }
        public int SpeedPerSecond { get; set; }
        public string SubmittingUser { get; set; }
        public virtual List<ServiceCensusBlock> ServiceCensusBlocks { get; set; }
        
        [ForeignKey("Ticket")]
        public int? TicketID { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
