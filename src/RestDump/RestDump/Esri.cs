using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RestDump
{
    public class EsriFfeatureLayer
    {
        public DataTable Table { get; set; }

        public EsriFfeatureLayer()
        {

        }

        public EsriFfeatureLayer(string json)
        {
            Table = DeserializeSelection(json);
        }

        public void WriteFile(string outputFilePath)
        {
            if (File.Exists(outputFilePath))
                File.Delete(outputFilePath);

            Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));

            WriteCsv(outputFilePath);
        }

        public void DumpFeatureTable(string url, string oidField)
        {
            TableIds ids = TableIds.GetAllIds(url);
            List<DataTable> tableList = new List<DataTable>();

            // single thread dump

            //foreach(int id in ids.objectIds)
            //{
            //    Console.WriteLine("OBJECTID: " + id);

            //    string queryUrl = url + "/query?where=" + oidField + "=" + id + "&text=&objectIds=&time=&geometry=&geometryType=esriGeometryPoint&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=*&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&resultOffset=&resultRecordCount=&f=pjson";
            //    string json;

            //    using (WebClient w = new WebClient())
            //    {
            //        StreamReader sr = new StreamReader(w.OpenRead(queryUrl));
            //        json = sr.ReadToEnd();
            //        sr.Close();
            //    }

            //    DataTable t = DeserializeTable(json);
            //    tableList.Add(t);
            //}


            // multi-thread dump

            ids.objectIds.Sort();
            IEnumerable<List<int>> idChunks = ids.objectIds.SplitList(500);

            
            Parallel.ForEach(idChunks, new ParallelOptions { MaxDegreeOfParallelism = 20 }, (chunk) =>
            {
                int idLow = chunk.First();
                Debug.WriteLine("idLow = " + idLow);
                int idHigh = chunk.Last();
                Debug.WriteLine("idHigh = " + idHigh);

                string queryUrl = url + "/query?where=" + oidField + ">=" + idLow + "+AND+" + oidField + "<=" + idHigh + "&text=&objectIds=&time=&geometry=&geometryType=esriGeometryPoint&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=*&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&resultOffset=&resultRecordCount=&f=pjson";
                string json;
                Debug.WriteLine("queryUrl = " + queryUrl);
                using (WebClient w = new WebClient())
                {
                    StreamReader sr = new StreamReader(w.OpenRead(queryUrl));
                    json = sr.ReadToEnd();
                    sr.Close();
                }

                DataTable t = DeserializeTable(json);
                tableList.Add(t);
            });

            //merge all tables
            DataTable mergedTable = new DataTable();
            tableList.ForEach(mergedTable.Merge);

            Table = mergedTable;
        }

        DataTable DeserializeTable(string json)
        {
            //Json string to dynamic object
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic tableObj = serializer.Deserialize<object>(json);

            //get features
            List<Dictionary<string, object>> features = new List<Dictionary<string, object>>();
            foreach (var feature in tableObj["features"])
                features.Add(feature);

            //get just the attributes form the features
            List<Dictionary<string, object>> attributes = new List<Dictionary<string, object>>();
            foreach (var feature in features)
                attributes.Add((Dictionary<string, object>)feature["attributes"]);

            //get column names from first row, all rows will have identical schema
            IEnumerable<string> colNames = new List<string>(attributes[0].Keys);

            return MakeTable(colNames, attributes);
        }

        DataTable DeserializeSelection(string json)
        {
            //Json string to dynamic object
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic tableObj = serializer.Deserialize<object>(json);

            //dynamic object to List<Dictionary<string, object> containing column, value for each row
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            foreach (var row in tableObj)
                data.Add(row);

            //get column names from first row, all rows will have identical schema
            IEnumerable<string> colNames = new List<string>(data[0].Keys);

            return MakeTable(colNames, data);
        }

        DataTable MakeTable(IEnumerable colNames, IList<Dictionary<string, object>> data)
        {
            DataTable table = new DataTable();

            //add DataTable columns
            foreach (string col in colNames)
                table.Columns.Add(col, typeof(string));

            foreach (var row in data)
            {
                List<string> insertRow = new List<string>();
                foreach (var value in row.Values)
                {
                    if (value == null)
                        insertRow.Add("null");
                    else
                        insertRow.Add(value.ToString());
                }

                table.Rows.Add(insertRow.ToArray());
            }

            return table;
        }

        void WriteCsv(string outputPath)
        {
            StringBuilder sb = new StringBuilder();

            //write column names
            IEnumerable<string> colNames = Table.Columns.Cast<DataColumn>().Select(columns => columns.ColumnName);
            sb.AppendLine(string.Join(",", colNames.Select(s => string.Format("\"{0}\"", s))));

            //write rows, all vlaues enclosed in quotes
            foreach (DataRow row in Table.Rows)
            {
                IEnumerable<string> rowValues = row.ItemArray.Select(rows => rows.ToString()).ToList();
                sb.AppendLine(string.Join(",", rowValues.Select(s => string.Format("\"{0}\"", s))));
            }

            File.WriteAllText(outputPath, sb.ToString());
        }        
    }

    class TableIds
    {
        public string objectIdFieldName { get; set; }
        public List<int> objectIds { get; set; }

        public static TableIds GetAllIds(string url)
        {
            //select all, return only the IDs
            url += "/query?where=1%3D1&text=&objectIds=&time=&geometry=&geometryType=esriGeometryPoint&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=*&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=true&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&resultOffset=&resultRecordCount=&f=pjson";
            string json;

            using (WebClient w = new WebClient())
            {
                StreamReader sr = new StreamReader(w.OpenRead(url));
                json = sr.ReadToEnd();
                sr.Close();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var obj = serializer.Deserialize<TableIds>(json);

            return obj;
        }
    }

}
