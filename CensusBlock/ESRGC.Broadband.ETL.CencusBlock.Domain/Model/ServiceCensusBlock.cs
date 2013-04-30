﻿using System.ComponentModel.DataAnnotations;
using System;
using ESRGC.Broadband.ETL.CensusBlock.Domain.ValidationAttributes;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.Model
{
    public class ServiceCensusBlock
    {
        private string _fullFipsID, _frn;
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
        [StringLength(200, ErrorMessage = "Name can not exceed 200 characters")]
        [Display(Name = "Provider Name")] 
        //[Name]//checks if name isn't a number or empty
        public string PROVNAME { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Name can not exceed 200 characters")]
        [Display(Name = "DBA Name", Description = "Doing-Business As Name")]
        //[Name]//checks if name isn't a number or empty
        public string DBANAME { get; set; }
        [Required]
        [Display(Name = "Provider Type")]
        [Range(1, 3, ErrorMessage = "Provider type value ranges from 1 to 3 ")]
        public short Provider_Type { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "FRN length can not excceed 10 characters")]
        [MinLength(10, ErrorMessage = "FRN does not meet minimum length (10 characters)")]
        [Display(Description = "FCC Registration Number")]
        public string FRN { 
            get { return _frn; }
            set {
                if (value.Length < 10) {
                    int originalLength = value.Length;
                    for (int i = 0; i < 10 - originalLength; i++) {
                        value = value.Insert(0, "0");
                    }
                }
                _frn = value;
            } 
        }
        [StringLength(2)]
        [Display(Name = "STATE FIPS", Description = "2-digit State ANSI (FIPS) Code")]
        public string STATEFIPS { get; set; }
        [StringLength(3)]
        [Display(Name = "COUNTY FIPS", Description = "3-digit County ANSI (FIPS) Code")]
        public string COUNTYFIPS { get; set; }
        [StringLength(6)]
        [Display(Name = "TRACT", Description = "Census Tract")]
        public string TRACT { get; set; }
        [StringLength(4)]
        [Display(Description = "Census Block ID")]
        public string BLOCKID { get; set; }
        //[StringLength(1)]
        //[Display(Description = "Block Subgroup")]
        //public string BLOCKSUBGROUP { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "FIPS code can not exceed 15 characters")]
        [MinLength(15, ErrorMessage = "FIPS code must be 15 characters")]
        [Display(Name = "FULL CENSUS BLOCK ID", Description = "Current block identifier; a concatenation of Census 2000 state Federal Information Processing Standards (FIPS) code, Census 2000 county FIPS code, Census 2000 census tract code, Census 2000 tabulation block number, and current block suffix 1")]
        public string FULLFIPSID {
            get { return _fullFipsID; }
            set {
                _fullFipsID = value;
                if (value.Length == 15 && value != "9999") {
                    //set State fips, county fips, Tract, and block ID
                    STATEFIPS = _fullFipsID.Substring(0, 2);
                    COUNTYFIPS = _fullFipsID.Substring(2, 3);
                    TRACT = _fullFipsID.Substring(5, 6);
                    BLOCKID = _fullFipsID.Substring(11, 4);
                }
            }
        }
        [Required]
        [Display(Name = "Technology of Transmission")]
        public short TRANSTECH { get; set; }
        [StringLength(2, ErrorMessage = "Speed code can not exceed 2 characters")]
        [Display(Name = "Max. Advertised Downstream Speed")]
        public string MAXADDOWN { get; set; }
        [StringLength(2, ErrorMessage = "Speed code can not exceed 2 characters")]
        [Display(Name = "Max. Advertised Upstream Speed")]
        public string MAXADUP { get; set; }
        [StringLength(2, ErrorMessage = "Speed code can not exceed 2 characters")]
        [Display(Name = "Typical Downstream Speed")]
        public string TYPICDOWN { get; set; }
        [StringLength(2, ErrorMessage = "Speed code can not exceed 2 characters")]
        [Display(Name = "Typical Upstream Speed")]
        public string TYPICUP { get; set; }

        public int SubmissionID { get; set; }
        public Submission Submission { get; set; }
    }
}
