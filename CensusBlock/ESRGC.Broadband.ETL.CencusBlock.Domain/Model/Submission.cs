using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.Model
{
    public class Submission
    {
        public Submission() {
            SubmissionTimeStarted = DateTime.Now;
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
        public virtual List<ServiceCensusBlock> ServiceCensusBlocks { get; set; }

    }
}
