using System;
using System.IO;

namespace RestDump
{
    class Program
    {
        static void Main(string[] args)
        {
            // test feature service: NOAA Weather Stations 
            string url = "http://maps1.arcgisonline.com/ArcGIS/rest/services/NWS_Weather_Stations/MapServer/2";
            string oidField = "OBJECTID_1";

            Console.WriteLine("Dumping layer: " + url);

            EsriFfeatureLayer table = new EsriFfeatureLayer();
            table.DumpFeatureTable(url, oidField);
            table.WriteFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "feature_dump.csv"));


            Console.WriteLine("Complete!");
            Console.ReadLine();

        }
    }
}
