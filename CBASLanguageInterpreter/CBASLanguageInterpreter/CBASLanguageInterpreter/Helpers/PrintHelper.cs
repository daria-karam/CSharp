using CBASLanguageInterpreter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CBASLanguageInterpreter.Helpers
{
    static class PrintHelper
    {
        public static void PrintRules(List<Rule> rules)
        {
            Console.WriteLine("---------------------------------------------------\nRules:");

            foreach (var rule in rules)
            {
                Console.WriteLine($"\t{rule}");
            }

            Console.WriteLine("---------------------------------------------------");
        }

        public static void PrintDictionaryWithListValue(Dictionary<int, List<int>> dictionary, string title)
        {
            Console.WriteLine($"---------------------------------------------------\n{title}:");
            string line;

            foreach (var element in dictionary)
            {
                if (!(element.Key == element.Value.First() && element.Value.Count == 1))
                {
                    line = $"\t{element.Key.ToStringFromHash()}: ";

                    foreach (var item in element.Value)
                    {
                        line += $"{item.ToStringFromHash() ?? "$"} | ";
                    }

                    line = line.Remove(line.Length - 3);

                    Console.WriteLine($"\t{line}");
                }
            }

            Console.WriteLine("---------------------------------------------------");
        }

        public static void PrintAnalysisTable(Dictionary<Record, Rule> table)
        {
            Console.WriteLine("---------------------------------------------------\nAnalysis Table:");

            foreach (var row in table)
            {
                Console.WriteLine($"\t{row.Key}: {row.Value}");
            }

            Console.WriteLine("---------------------------------------------------");
        }

        public static void PrintAnalysisState(Stack<int> stack, string input, int current)
        {
            string line = "";

            foreach (var element in stack)
            {
                line = element.ToStringFromHash() + line;
            }

            line = $"Stack: '{line}'\t Input: '{input}'\t Current: '{current.ToStringFromHash()}'";

            Console.WriteLine(line);
        }

        public static void PrintHashDictionary(Dictionary<int, string> dictionary)
        {
            Console.WriteLine("---------------------------------------------------\nHash Dictionary:");

            foreach (var element in dictionary)
            {
                Console.WriteLine($"\t{element.Key}: {element.Value}");
            }

            Console.WriteLine("---------------------------------------------------");
        }

        public static void PrintErrors(List<string> errors)
        {
            Console.WriteLine("---------------------------------------------------\nErrors:");

            foreach (var error in errors)
            {
                Console.WriteLine($"\t{error}");
            }

            Console.WriteLine($"Total errors count: {errors.Count}");
            Console.WriteLine("---------------------------------------------------");
        }

        public static void PrintIntStack(Stack<int> output)
        {
            Console.WriteLine($"---------------------------------------------------\nProgram code:");

            var line = string.Join("\n", output.Select(_ => _.ToStringFromHash())).Replace(' ', '~');

            Console.WriteLine(line);
        }
    }
}
