using System.Web;
using System.Web.Mvc;

namespace ESRGC.Broadband.ETL.CensusBlock
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}