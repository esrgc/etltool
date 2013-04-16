using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LINQtoCSV;

namespace ESRGC.Broadband.ETL.CensusBlock.Models
{
    internal class CsvDataRow: List<DataRowItem>, IDataRow
    {
    }
}