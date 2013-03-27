﻿using System.ComponentModel.DataAnnotations;
using System;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.Model
{
    public class ServiceCensusBlock
    {
        public ServiceCensusBlock() {
            DBANAME = "N/A";
            FRN = "9999";
            FULLFIPSID = "9999";
            TRANSTECH = -9999;
            MAXADDOWN = "ZZ";
            TYPICDOWN = "ZZ";
            MAXADUP = "ZZ";
            TYPICUP = "ZZ";
        }
        [Key]
        public int ServiceCensusBlockID { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "Provider Name")]
        public string PROVNAME { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "DBA Name", Description = "Doing-Business As Name")]
        public string DBANAME { get; set; }
        [Required]
        [Display(Name = "Provider Type")]
        public short Provider_Type { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Description = "FCC Registration Number")]
        public string FRN { get; set; }
        [Required]
        [StringLength(2)]
        [Display(Name = "STATE FIPS", Description = "2-digit State ANSI (FIPS) Code")]
        public string STATEFIPS { get; set; }
        [Required]
        [StringLength(3)]
        [Display(Name = "COUNTY FIPS", Description = "3-digit County ANSI (FIPS) Code")]
        public string COUNTYFIPS { get; set; }
        [Required]
        [StringLength(6)]
        [Display(Name = "TRACT", Description = "Census Tract")]
        public string TRACT { get; set; }
        [Required]
        [StringLength(4)]
        [Display(Description = "Census Block Full ID")]
        public string BLOCKID { get; set; }
        [StringLength(1)]
        [Display(Description = "Block Subgroup")]
        public string BLOCKSUBGROUP { get; set; }
        [Required]
        [StringLength(16)]
        [Display(Name = "FULL CENSUS BLOCK ID", Description = "Current block identifier; a concatenation of Census 2000 state Federal Information Processing Standards (FIPS) code, Census 2000 county FIPS code, Census 2000 census tract code, Census 2000 tabulation block number, and current block suffix 1")]
        public string FULLFIPSID { get; set; }
        [Required]
        [Display(Name = "Technology of Transmission")]
        public short TRANSTECH { get; set; }
        [StringLength(2)]
        [Display(Name = "Max. Advertised Downstream Speed")]
        public string MAXADDOWN { get; set; }
        [StringLength(2)]
        [Display(Name = "Max. Advertised Upstream Speed")]
        public string MAXADUP { get; set; }
        [StringLength(2)]
        [Display(Name = "Typical Downstream Speed")]
        public string TYPICDOWN { get; set; }
        [StringLength(2)]
        [Display(Name = "Typical Upstream Speed")]
        public string TYPICUP { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}