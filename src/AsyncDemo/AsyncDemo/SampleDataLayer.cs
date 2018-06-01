using System;
using System.Threading.Tasks;

namespace AsyncDemo
{
    //class to simulate both long and short running data operations
    public static class SampleDataLayer
    {
        public async static Task<String> MethodA()
        {
            Console.WriteLine("Fetching data obj A...");
            await Task.Delay(4000);
            string data = "'Hello, world!'";
            Console.WriteLine("Recevied data obj A.");

            return data;
        }

        public async static Task<int> MethodB()
        {
            Console.WriteLine("Fetching data obj B...");
            await Task.Delay(8000);
            int data = 5130843;
            Console.WriteLine("Recevied data obj B.");

            return data;
        }

        public static int MethodC()
        {
            Console.WriteLine("Calculating data obj C...");
            int c = MethodD(100);
            Console.WriteLine("data obj C complete.");
            return c;
        }

        static int MethodD(int c)
        {
            return 100 * c;
        }
    }

    public class SampleDataResponse
    {
        public string A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
    }
}
