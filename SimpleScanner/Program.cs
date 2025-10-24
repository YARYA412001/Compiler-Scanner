using System;
using System.Collections.Generic;

namespace SimpleScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Simple Scanner for Compiler ===");
            Console.WriteLine("Complete lexical analyzer implementation\n");

            TestScanner("Simple Assignment", "x := 5");
            TestScanner("If Statement", "if count = 1 then result := 31");
            TestScanner("For Loop", "for i := 1 to 10 do x := x + i");
            TestScanner("While Loop", "while count < 100 do count := count + 1");
            TestScanner("Repeat Until", "repeat x := x * 2 until x > 100");
            TestScanner("Variable Declaration", "var x, y: integer;");
            TestScanner("Begin End Block", "begin x := 5; y := 10 end");
            TestScanner("Procedure Example", "procedure test; begin result := a + b end");
            TestScanner("Decimal Numbers", "price := 3.14 + 2.5");
            TestScanner("Complex Example",
                "var sum, i: integer;\n" +
                "begin\n" +
                "  sum := 0;\n" +
                "  for i := 1 to 10 do\n" +
                "    sum := sum + i;\n" +
                "end");

            Console.WriteLine("\n=== Scanner Implementation Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void TestScanner(string testName, string input)
        {
            Console.WriteLine($"\n{testName}:");
            Console.WriteLine($"Input: {input}");
            Console.WriteLine("Tokens:");
            Console.WriteLine(new string('-', 50));

            var scanner = new Scanner(input);
            var tokens = scanner.ScanAll();

            foreach (var token in tokens)
            {
                if (token.Type != TokenType.EOF)
                    Console.WriteLine($"  {token}");
            }
        }
    }
}