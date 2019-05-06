using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunnerAppCon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            int argCount = args.Length;

            int i = 1;
            foreach (string s in args)
            {
                Console.WriteLine($"arg {i}: [{s}]");
                i++;
            }

            Console.ReadKey();

        }
    }
}



