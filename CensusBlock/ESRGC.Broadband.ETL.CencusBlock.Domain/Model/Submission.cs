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
            SubmissionTime = DateTime.Now;
        }
        [Key]
        public int SubmissionID { get; set; }
        [StringLength(30)]
        public string Status { get; set; }
        [Required]
        public DateTime SubmissionTime { get; set; }
        public virtual List<ServiceCensusBlock> ServiceCensusBlocks { get; set; }
    }
}
