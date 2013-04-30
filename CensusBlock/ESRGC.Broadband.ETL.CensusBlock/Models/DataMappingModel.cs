using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;

namespace ESRGC.Broadband.ETL.CensusBlock.Models
{
    public class DataMappingModel
    {
        public IEnumerable<string> UploadDataColumns { get; set; }
        public ColumnMapping MappingObject { get; set; }
        public ServiceCensusBlock DefaultData { get; set; }
    }
    public class PreviewMappingModel
    {
        public int SuccessCount { get; set; }
        public IDictionary<int, object> ErrorList { get; set; }
        public IList<ServiceCensusBlock> Data { get; set; }
        public int SubmissionID { get; set; }
    }
}