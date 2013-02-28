using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.Broadband.ETL.CencusBlock.Domain.Model
{
    public class ColumnMapping
    {
        public string PPROVNAMEColumn { get; set; }
        public string DBANAMEColumn { get; set; }
        public string Provider_typeColumn { get; set; }
        public string FRNColumn { get; set; }
        public string STATEFIPSColumn { get; set; }
        public string COUNTYFIPSColumn { get; set; }
        public string TRACTColumn { get; set; }
        public string BLOCKIDColumn { get; set; }
        public string BLOCKSUBGROUPColumn { get; set; }
        public string FULLFIPSIDColumn { get; set; }
        public string TRANSTECHColumn { get; set; }
        public string MAXADDOWNColumn { get; set; }
        public string MAXADUPColumn { get; set; }
        public string TYPICDOWNColumn { get; set; }
        public string TYPICUPColumn { get; set; }
    }
}
