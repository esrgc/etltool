using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.Model
{
    public class Ticket
    {
        public Ticket() {
            IssuedDate = DateTime.Now;
            ExpirationDate = DateTime.Now.AddDays(3);
            Description = string.Empty;
        }
        [Key]
        public int TicketID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime IssuedDate { get; set; }

        DateTime _expirationDate;
        [Display(Name = "Expired on")]
        public DateTime ExpirationDate {
            get { return _expirationDate; }
            set {
                _expirationDate = value;
            }
        }
        [NotMapped]
        public bool Active {
            get {
                return _expirationDate >= DateTime.Now;
            }
        }
        public virtual IEnumerable<Submission> Submissions { get; set; }
    }
}
