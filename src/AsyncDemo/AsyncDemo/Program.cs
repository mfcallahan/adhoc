using System;
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
            Task<string> taskDataA = SampleDataLayer.MethodA();
            Task<int> taskDataB = SampleDataLayer.MethodB();

            // do synchronous stuff which doesn't need the result of MethodA() or MethodB()
            int dataC = SampleDataLayer.MethodC();

            // now call await on the tasks
            Console.WriteLine("Awaiting...");
            SampleDataResponse response = new SampleDataResponse()
            {
                A = await taskDataA,
                B = await taskDataB,
                C = dataC
            };

            Console.WriteLine("SampleDataResponse obj created:");
            Console.WriteLine("response.A = {0}", response.A);
            Console.WriteLine("response.B = {0}", response.B);
            Console.WriteLine("response.C = {0}", response.C);

            Console.WriteLine();
            Console.WriteLine("Complete.");
            Console.ReadLine();
        }
    }
}
