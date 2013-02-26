using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESRGC.Broadband.ETL.CensusBlock.Extension
{
    public static class ListToJson
    {
        /// <summary>
        /// converts IDictionary typed list to json string
        /// represents tabular data
        /// </summary>
        /// <param name="list">List of dictionary that contains table data</param>
        /// <returns>json string</returns>
        public static string ToJSon(this IEnumerable<IDictionary<string, object>> list) {
            string jsonStr = "[";
            var row = new List<string>();
            foreach (var dict in list) {
                var str = "{";
                var valueList = new List<string>();
                foreach (var pair in dict) {
                    valueList.Add('"' + pair.Key + "\": \"" + pair.Value + '"');
                }
                str += string.Join(",", valueList);
                str += '}';
                row.Add(str);
            }
            jsonStr += string.Join(",", row);
            jsonStr += ']';
            return jsonStr;
        }
    }
}