using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinqToExcel;
using System.IO;
using LINQtoCSV;
using ESRGC.Broadband.ETL.CensusBlock.Models;

namespace ESRGC.Broadband.ETL.CensusBlock.Extension
{
    public class Helpers
    {
        public static List<IDictionary<string, object>> parseToDictionary(string filePath) {
            var excel = new ExcelQueryFactory(filePath);
            var worksheets = excel.GetWorksheetNames();

            var allRows = from c in excel.Worksheet(worksheets.First())
                          select c;

            var columnNames = allRows.First().ColumnNames;
            var dataList = new List<IDictionary<string, object>>();
            foreach (var row in allRows) {
                IDictionary<string, object> dict = new Dictionary<string, object>();
                foreach (var name in columnNames) {
                    var value = row[name];
                    dict.Add(name, value);
                }
                dataList.Add(dict);
            }
            return dataList;
        }
        /*------------parse csv inputstream and return a list of dictionary of data-----------------*/
        public static List<IDictionary<string, object>> parseToDictionary(Stream inputStream) {
            //read header
            string[] headers;
            IEnumerable<CsvDataRow> data;
            using (StreamReader reader = new StreamReader(inputStream)) {
                if (reader.EndOfStream) {
                    reader.BaseStream.Position = 0;
                    reader.DiscardBufferedData();
                }
                string firstLine = reader.ReadLine();
                headers = firstLine.Split(',');

                //return to beginning of the stream
                inputStream.Position = 0;
                reader.DiscardBufferedData();

                //read data
                CsvFileDescription fd = new CsvFileDescription {
                    SeparatorChar = ',',
                    FirstLineHasColumnNames = true
                };
                CsvContext context = new CsvContext();
                data = context.Read<CsvDataRow>(reader, fd);

                var result = new List<IDictionary<string, object>>();
                foreach (CsvDataRow row in data) {
                    int i = 0;
                    var dict = new Dictionary<string, object>();
                    foreach (var rowValue in row) {//read data 
                        dict.Add(headers[i], rowValue.Value);
                        i++;
                    }
                    result.Add(dict);
                }

                return result;
            }
        }

        public static Stream writeToCsv<TEntity>(IEnumerable<TEntity> list) {
            //file descriptor
            CsvFileDescription fd = new CsvFileDescription() { 
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = true
            };
            var stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            CsvContext context = new CsvContext();
            context.Write<TEntity>(list, writer, fd);
            return stream;
        }
    }
}