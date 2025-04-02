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
            var inputText = File.ReadAllText(path);
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
