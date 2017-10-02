using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestDump
{
    class Program
    {
        static void Main(string[] args)
        {
            //string url = "https://apps.fs.usda.gov/arcx/rest/services/EDW_FEATURE/EDW_RecreationAreaActivities_01/FeatureServer/0";
            string url = "https://www.arcgis.com/sharing/rest/content/items/b5868511b7d64936bff99d7336b8f217/data"; 
            string oidField = "_OBJECTID";

            Console.WriteLine("Dumping layer: " + url);

            EsriFfeatureLayer table = new EsriFfeatureLayer();
            table.DumpFeatureTable(url, oidField);
            table.WriteFile(@"C:\Temp\feature_dump.csv");

            Console.WriteLine("Complete!");
            Console.ReadLine();

        }
    }
}
