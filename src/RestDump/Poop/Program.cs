using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poop
{
    class Program
    {
        static void Main(string[] args)
        {
            Parallel.ForEach(idChunks, new ParallelOptions { MaxDegreeOfParallelism = 20 }, (chunk) =>
            {

            };
        }
    }
}
