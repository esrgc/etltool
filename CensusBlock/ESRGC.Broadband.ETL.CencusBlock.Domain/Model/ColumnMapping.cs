using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.Model
{
    public class ColumnMapping
    {
        [Required]
        [DisplayName("Provider Name")]
        public string PPROVNAMEColumn { get; set; }
        [Required]
        [Display(Name = "DBA Name", Description = "Doing-Business As Name")]
        public string DBANAMEColumn { get; set; }
        [Required]
        [DisplayName("Provider Type")]
        public string Provider_typeColumn { get; set; }
        [Required]
        [Display(Name = "FRN", Description = "FCC Registration Number")]
        public string FRNColumn { get; set; }
        [Required]
        [Display(Name = "State FIPS", Description = "2-digit State ANSI (FIPS) Code")]
        public string STATEFIPSColumn { get; set; }
        [Required]
        [Display(Name = "Count FIPS", Description = "3-digit County ANSI (FIPS) Code")]
        public string COUNTYFIPSColumn { get; set; }
        [Required]
        [Display(Name = "TRACT", Description = "Census Tract")]
        public string TRACTColumn { get; set; }
        [Display(Name = "Block ID", Description = "Census Block Full ID")]
        public string BLOCKIDColumn { get; set; }
        [DisplayName("Block Subgroup")]
        public string BLOCKSUBGROUPColumn { get; set; }
        [Required]
        [Display(Name = "Full Census Block ID", Description = "Current block identifier; a concatenation of Census 2000 state Federal Information Processing Standards (FIPS) code, Census 2000 county FIPS code, Census 2000 census tract code, Census 2000 tabulation block number, and current block suffix 1")]
        public string FULLFIPSIDColumn { get; set; }
        [Required]
        [Display(Name= "Trans Tech", Description = "Technology of Transmission")]
        public string TRANSTECHColumn { get; set; }
        [Display(Name = "Max. Ad. Download", Description = "Max. Advertised Download Speed")]
        public string MAXADDOWNColumn { get; set; }
        [Display(Name = "Max. Ad. Upload", Description = "Max. Advertised Upload Speed")]
        public string MAXADUPColumn { get; set; }
        [Display(Name = "Typical Download", Description = "Typical Download Speed")]
        public string TYPICDOWNColumn { get; set; }
        [Display(Name = "Typical Upload", Description = "Typical Download Speed")]
        public string TYPICUPColumn { get; set; }
    }
}
