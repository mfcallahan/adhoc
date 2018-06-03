using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo
{
    public static class SampleDataLayer
    {
        const string _apiUrl = "http://mfcallahan.com/api/GetDelayedResponse?waitSeconds=";

        // synchronous method to simulate a long running HTTP request
        public static string GetDelayedResponse(int seconds)
        {
            using (WebClient client = new WebClient())
            {
                Console.WriteLine("Starting /api/GetDelayedResponse");
                var message = client.DownloadString(string.Concat(_apiUrl, seconds));
                Console.WriteLine("/api/GetDelayedResponse complete.");

                return message.Trim('"');
            }
        }

        // async method to simulate a long running HTTP request
        public async static Task<string> GetDelayedResponseAsync(int seconds)
        {
            using (WebClient client = new WebClient())
            {
                Console.WriteLine("Starting async /api/GetDelayedResponse");
                var message =  await client.DownloadStringTaskAsync(string.Concat(_apiUrl, seconds));
                Console.WriteLine("async /api/GetDelayedResponse complete.");

                return message.Trim('"');
            }            
        }

        // synchronous method to simualte long running work in application code
        public static string SimulateLongProcess(int seconds)
        {
            Console.WriteLine("Starting SimulateLongProcess()...");
            Thread.Sleep(seconds * 1000);
            string data = "Hello, world!";
            Console.WriteLine("SimulateLongProcess() complete.");

            return data;
        }

        // async method to simualte long running work in application code
        public async static Task<string> SimulateLongProcessAsync(int seconds)
        {
            Console.WriteLine("Starting SimulateLongProcessAsync()...");
            await Task.Delay(seconds * 1000);
            string data = "Hello, world!";
            Console.WriteLine("SimulateLongProcessAsync() complete.");

            return data;
        }

        // synchronous methods to simulate short running work in application code
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
