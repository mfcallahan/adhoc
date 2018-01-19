using System;
using System.Configuration;

namespace RestDump
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = ConfigurationManager.AppSettings["featureServiceUrl"];
            string oidField = ConfigurationManager.AppSettings["objectIdField"];
            int queryLimit = Math.Abs(int.Parse(ConfigurationManager.AppSettings["queryLimit"]));

            Console.WriteLine("Dumping layer: " + url);

            EsriFfeatureLayer table = new EsriFfeatureLayer();
            
            table.DumpFeatureTable(url, oidField, queryLimit);
            table.WriteFile(ConfigurationManager.AppSettings["outputFile"]);


            Console.WriteLine("Complete!");
            Console.ReadLine();

        }
    }
}
