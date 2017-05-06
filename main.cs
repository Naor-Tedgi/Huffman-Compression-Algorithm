using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hoffman
{
    class main
    {

        static void Main(string[] args)
        {
            ATPproject.MyModel mm = new ATPproject.MyModel();
            try
            {
                Console.WriteLine(args[0]);
                if (args[0] == "-c")
                {
                    Console.WriteLine("File {0} Size Before Compress:{1}", args[1], mm.DoSize(args));
                    Console.WriteLine("Starting Compress..");
                    Console.WriteLine(mm.DoHuf(args));
                    args[1] = args[1] + ".huf";
                    Console.WriteLine("File {0} Size After Compress:{1}", args[1], mm.DoSize(args));
                    Console.Read();
                }
                if (args[0] == "-d")
                {
                    Console.WriteLine("Starting DeCompress..");
                    Console.WriteLine(mm.DoUnhuf(args));
                    Console.Read();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
