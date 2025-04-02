using HammingCodeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HammingCodeExperiments
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "../../../inputBytes.txt";

            Console.WriteLine(HammingCode.Encode(File.ReadAllText(path)));
        }
    }
}
