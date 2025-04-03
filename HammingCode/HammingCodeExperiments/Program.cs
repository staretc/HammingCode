using HammingCodeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HammingCodeExperiments
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var numOfTests = 10;
            Console.WriteLine($"Number of tests: {numOfTests}");

            string path = "../../../inputBytes.txt";
            var inputText = File.ReadAllText(path);

            while (numOfTests-- > 0)
            {
                Console.WriteLine("\n=============\n");
                TestHammingCode(inputText);
                Console.WriteLine("\n=============\n");
                Thread.Sleep(100);
            }
            
        }
        static void TestHammingCode(string inputText)
        {
            Console.WriteLine($"Input Text:   \t\t {inputText}");

            var encodedText = HammingCode.Encode(inputText);
            Console.WriteLine($"Encoded Text: \t\t {encodedText}");

            encodedText = ErrorMaker.SetRandomError(encodedText);
            Console.WriteLine($"Encoded Text with error: {encodedText}");

            var decodedText = HammingCode.Decode(encodedText);
            Console.WriteLine($"Decoded Text: \t\t {decodedText}");
        }
    }
}
