using System;
using System.Net;
using System.Threading.Tasks;

namespace AsyncDemo
{
    public static class SampleDataLayer
    {
        static readonly string _apiUrl = "http://mfcallahan.com/api/GetDelayedResponse?s=";

        // method to simulate a long running HTTP request
        public async static Task<string> GetDelayedResponse(int seconds)
        {           

            using (WebClient client = new WebClient())
            {
                Console.WriteLine("Contacting server...");
                var message =  await client.DownloadStringTaskAsync(string.Concat(_apiUrl, seconds));
                Console.WriteLine("Server response received.");

                return message;
            }            
        }

        // method to simualte a long running process in app code
        public async static Task<String> SimulateLongProcess(int s)
        {
            Console.WriteLine("Simulating long process...");
            await Task.Delay(s * 1000);
            string data = "'Hello, world!'";
            Console.WriteLine("Long process complete.");

            return data;
        }

        // methods to simulate synchronous short running process in app code
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
