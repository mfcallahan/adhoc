using System;
using System.Net;
using System.Threading.Tasks;

namespace AsyncDemo
{
    public static class SampleDataLayer
    {
        static readonly string _apiUrl = "http://mfcallahan.com/api/GetDelayedResponse?waitSeconds=";

        // async method to simulate long running HTTP request
        public async static Task<string> GetDelayedResponse(int seconds)
        {           

            using (WebClient client = new WebClient())
            {
                Console.WriteLine("Starting /api/GetDelayedResponse()");
                var message =  await client.DownloadStringTaskAsync(string.Concat(_apiUrl, seconds));
                Console.WriteLine("GetDelayedResponse() complete.");

                return message.Trim('"');
            }            
        }

        // async method to simualte long running work in app code
        public async static Task<String> SimulateLongProcess(int s)
        {
            Console.WriteLine("Starting SimulateLongProcess()...");
            await Task.Delay(s * 1000);
            string data = "Hello, world!";
            Console.WriteLine("SimulateLongProcess() complete.");

            return data;
        }

        // synchronous methods to simulate short running work in app code
        public static int Foo()
        {
            Console.WriteLine("Starting Foo()...");
            int n = Bar(10);
            Console.WriteLine("Foo() complete.");
            return n;
        }

        static int Bar(int n)
        {
            return 100 * n;
        }
    }

    public class SampleData
    {
        public string A { get; set; }
        public string B { get; set; }
        public int C { get; set; }
    }
}
