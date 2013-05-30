using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace ESRGC.Broadband.ETL.CensusBlock.Extension
{
    public static class HtmlHelperExt
    {
        /// <summary>
        /// add new search routing filter
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="routeDict">Current routing values</param>
        /// <param name="newFilter">New route value</param>
        /// <returns></returns>
        public static string SearchFilter(
            this UrlHelper helper,
            string actionName,
            IDictionary<string, object> routeDict,
            string newFilterKey,
            string newFilterVal) {

            var routeValues = new RouteValueDictionary(routeDict);
            if (routeValues.Keys.Contains(newFilterKey))
                routeValues[newFilterKey] = newFilterVal;
            else
                routeValues.Add(newFilterKey, newFilterVal);
            //generate url
            return helper.Action(actionName, routeValues);
        }

        public static string ClearSearchFilter(
            this UrlHelper helper,
            string actionName,
            IDictionary<string, object> routeDict,
            string removeKey){
            //create new route dictionary
            var routeValues = new RouteValueDictionary(routeDict);
            //remove key
            routeValues.Remove(removeKey);
            //generate url
            return helper.Action(actionName, routeValues);
        }

        public static string TimeSpan(this HtmlHelper helper, DateTime? timeInPast) {
            if (timeInPast == null)
                return "";
            var timeSpan = DateTime.Now - timeInPast.Value;
            if (timeSpan.Days == 0) {
                if (timeSpan.Hours == 0) {
                    if (timeSpan.Minutes == 0)
                        return "Less than a minute ago";
                    if (timeSpan.Minutes == 1)
                        return timeSpan.Minutes + " minute ago";
                    else
                        return timeSpan.Minutes + " minutes ago";
                }
                if (timeSpan.Hours == 1)
                    return timeSpan.Hours + " hour ago";
                else
                    return timeSpan.Hours + " hours ago";
            }
            if (timeSpan.Days == 1)
                return timeSpan.Days + " day ago";
            else
                return timeSpan.Days + " days ago";
            
        }
    }
}