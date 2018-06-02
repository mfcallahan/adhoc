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

        static async Task Main(string[] args)
        {
            Console.WriteLine("App start.");
            Console.WriteLine();

            // start long running data fethcing operations
            Task<string> dataB = SampleDataLayer.GetDelayedResponse(8);
            Task<string> dataA = SampleDataLayer.SimulateLongProcess(4);            

            // do synchronous work which doesn't need the result of dataA or dataB
            int dataC = SampleDataLayer.Foo();

            // now call await on the tasks
            Console.WriteLine("Awaiting...");
            SampleData data = new SampleData()
            {
                A = await dataA,
                B = await dataB,
                C = dataC
            };

            Console.WriteLine("SampleData obj created:");
            Console.WriteLine("response.A = {0}", data.A);
            Console.WriteLine("response.B = {0}", data.B);
            Console.WriteLine("response.C = {0}", data.C);

            Console.WriteLine();
            Console.WriteLine("Complete.");
            Console.ReadLine();
        }

        void RunAsync()
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            s.Stop();
        }

        void RunSync()
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            s.Stop();
        }        
    }
}
