using System;

namespace LexicalAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            One();
        }

        static void One()
        {
            var textParser = new TextParser();

            var script = "ab = 10";
            var tokenize = textParser.tokenize(script);

            Console.WriteLine("token \t value");
            foreach (var result in tokenize)
            {
                Console.WriteLine(result.tokenType + " \t " + result.text.ToString());
            }
        }
    }
}
