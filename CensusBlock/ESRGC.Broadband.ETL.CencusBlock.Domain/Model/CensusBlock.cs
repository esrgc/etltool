using System.ComponentModel.DataAnnotations;

namespace ESRGC.Broadband.ETL.CencusBlock.Domain.Model
{
    public class CensusBlock
    {
        public int CensusBlockID { get; set; }
        [Required]
        [MaxLength(200)]
        [Display(Name="Provider Name")]
        public string PROVNAME { get; set; }
        [Required]
        [MaxLength(200)]
        [Display(Name = "DBA Name")]
        public string DBANAME { get; set; }
        [Required]
        [Display(Name = "Provider Type")]
        public short Provider_Type { get; set; }
        [Required]
        [MaxLength(10)]
        public string FRN { get; set; }
        [Required]
        [MaxLength(2)]
        [Display(Name = "2-digit State ANSI (FIPS) Code")]
        public string STATEFIPS { get; set; }
        [Required]
        [MaxLength(3)]
        [Display(Name = "3-digit County ANSI (FIPS) Code")]
        public string COUNTYFIPS { get; set; }
        [Required]
        [MaxLength(6)]
        [Display(Name = "Census Tract")]
        public string TRACT { get; set; }
        [Required]
        [MaxLength(4)]
        [Display(Name = "Census_Block_Full_ID")]
        public string BLOCKID { get; set; }
        [MaxLength(1)]
        [Display(Name = "Block Subgroup")]
        public string BLOCKSUBGROUP { get; set; }
        [Required]
        [MaxLength(16)]
        public string FULLFIPSID { get; set; }
        [Required]
        [Display(Name = "End user category")]
        public string ENDUSERCAT { get; set; }
        [Required]
        [Display(Name = "Transmission Technology")]
        public short TRANSTECH { get; set; }
        [MaxLength(2)]
        [Display(Name = "Max. Advertised Downstream Speed")]
        public string MAXADDOWN { get; set; }
        [MaxLength(2)]
        [Display(Name = "Max. Advertised Upstream Speed")]
        public string MAXADUP { get; set; }
        [MaxLength(2)]
        [Display(Name = "Typical Downstream Speed")]
        public string TYPICDOWN { get; set; }
        [MaxLength(2)]
        [Display(Name = "Typical Upstream Speed")]
        public string TYPICUP { get; set; }
    }
}
