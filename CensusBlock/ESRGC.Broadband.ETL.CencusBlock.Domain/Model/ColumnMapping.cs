using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.Model
{
    public class ColumnMapping
    {
        [DisplayName("Provider Name")]
        public string PPROVNAMEColumn { get; set; }
        [DisplayName("DBA Name")]
        public string DBANAMEColumn { get; set; }
        [DisplayName("Provider Type")]
        public string Provider_typeColumn { get; set; }
        [DisplayName("FRN")]
        public string FRNColumn { get; set; }
        [DisplayName("State FIPS")]
        public string STATEFIPSColumn { get; set; }
        [DisplayName("County FIPS")]
        public string COUNTYFIPSColumn { get; set; }
        [DisplayName("TRACT")]
        public string TRACTColumn { get; set; }
        [DisplayName("Block ID")]
        public string BLOCKIDColumn { get; set; }
        [DisplayName("Block Subgroup")]
        public string BLOCKSUBGROUPColumn { get; set; }
        [DisplayName("Full FIPS ID")]
        public string FULLFIPSIDColumn { get; set; }
        [DisplayName("Transmission Tech")]
        public string TRANSTECHColumn { get; set; }
        [DisplayName("Max Advertised Download")]
        public string MAXADDOWNColumn { get; set; }
        [DisplayName("Max Advertised Upload")]
        public string MAXADUPColumn { get; set; }
        [DisplayName("Typical Download")]
        public string TYPICDOWNColumn { get; set; }
        [DisplayName("Typical Upload")]
        public string TYPICUPColumn { get; set; }
    }
}
