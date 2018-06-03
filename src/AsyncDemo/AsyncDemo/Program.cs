using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsyncDemo
{
    class Program
    {
        // https://stackoverflow.com/questions/47588531/error-message-cs5001-program-does-not-contain-a-static-main-method-suitable-f
        // https://stackoverflow.com/questions/13002507/how-can-i-call-async-go-method-in-for-example-main
        // https://stackoverflow.com/questions/9343594/how-to-call-asynchronous-method-from-synchronous-method-in-c

        // Wait time in seconds for SampleDataLayer methods
        const int _getDelayedResponseWaitTime = 4;
        const int _simulateLongProcessWaitTime = 3;

        static async Task Main()
        {
            Stopwatch s = new Stopwatch();

            // RunDemoSynchronous() will call three methods in SampleDataLayer synchronously
            s.Start();
            RunDemoSynchronous();
            s.Stop();

            Console.WriteLine("RunDemoSynchronous() complete. Elapsed seconds: " + s.Elapsed.TotalSeconds + Environment.NewLine);

            // RunDemoAsynchronous() will call three methods in SampleDataLayer asynchronously
            s.Restart();
            await RunDemoAsynchronous();
            s.Stop();
            Console.WriteLine("RunDemoAsynchronous() complete. Elapsed seconds: " + s.Elapsed.TotalSeconds + Environment.NewLine);

            Console.WriteLine("Demo complete.");
            Console.ReadLine();
        }

        static void RunDemoSynchronous()
        {
            Console.WriteLine("RunDemoSynchronous() start.");

            // execute long and short running methods synchronously
            string dataA = SampleDataLayer.GetDelayedResponse(_getDelayedResponseWaitTime);
            string dataB = SampleDataLayer.SimulateLongProcess(_simulateLongProcessWaitTime);
            int dataC = SampleDataLayer.Foo();

            SampleData sample = new SampleData()
            {
                A = dataA,
                B = dataB,
                C = dataC
            };

            Console.WriteLine("SampleData object 'sample' created:");
            Console.WriteLine("sample.A = {0}", sample.A);
            Console.WriteLine("sample.B = {0}", sample.B);
            Console.WriteLine("sample.C = {0}", sample.C);
        }

        async static Task RunDemoAsynchronous()
        {
            Console.WriteLine("RunDemoAsynchronous() start.");

            Stopwatch s = new Stopwatch();
            s.Start();

            // start long running asynchronous methods
            Task<string> dataA = SampleDataLayer.GetDelayedResponseAsync(_getDelayedResponseWaitTime);
            Task<string> dataB = SampleDataLayer.SimulateLongProcessAsync(_simulateLongProcessWaitTime);

            // start synchronous method which doesn't need to wait for dataA or dataB
            int dataC = SampleDataLayer.Foo();

            // now call await on the tasks
            Console.WriteLine("Awaiting...");
            SampleData sample = new SampleData()
            {
                A = await dataA,
                B = await dataB,
                C = dataC
            };

            Console.WriteLine("SampleData object 'sample' created:");
            Console.WriteLine("sample.A = {0}", sample.A);
            Console.WriteLine("sample.B = {0}", sample.B);
            Console.WriteLine("sample.C = {0}", sample.C);
        }
    }
}