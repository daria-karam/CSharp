using CBASLanguageInterpreter.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBASLanguageInterpreter.Helpers
{
    class StringHelper
    {
        public static string GetStringFromRules(List<Rule> rules)
        {
            var result = new StringBuilder();

            result.AppendLine("---------------------------------------------------\nRules:");

            foreach (var rule in rules)
            {
                result.AppendLine($"\t{rule}");
            }

            result.AppendLine("---------------------------------------------------");

            return result.ToString();
        }

        public static string GetStringFromDictionaryWithListValue(Dictionary<int, List<int>> dictionary, string title)
        {
            var result = new StringBuilder();

            result.AppendLine($"---------------------------------------------------\n{title}:");

            foreach (var element in dictionary)
            {
                if (!(element.Key == element.Value.First() && element.Value.Count == 1))
                {
                    result.AppendLine($"\t\t{element.Key.ToStringFromHash()}: " +
                        string.Join(" | ", element.Value.Select(_ => _.ToStringFromHash() ?? "$")));
                }
            }

            result.AppendLine("---------------------------------------------------");

            return result.ToString();
        }

        public static string GetStringFromAnalysisTable(Dictionary<Record, Rule> table)
        {
            var result = new StringBuilder();

            result.AppendLine("---------------------------------------------------\nAnalysis Table:");

            foreach (var row in table)
            {
                result.AppendLine($"\t{row.Key}: {row.Value}");
            }

            result.AppendLine("---------------------------------------------------");

            return result.ToString();
        }

        public static string GetStringFromAnalysisState(Stack<int> stack, string input, int current)
        {
            var result = new StringBuilder();

            string line = "";

            foreach (var element in stack)
            {
                line = element.ToStringFromHash() + line;
            }

            result.AppendLine($"Stack: '{line}'\t Input: '{input}'\t Current: '{current.ToStringFromHash()}'");

            return result.ToString();
        }

        public static string GetStringFromHashDictionary(Dictionary<int, string> dictionary)
        {
            var result = new StringBuilder();

            result.AppendLine("---------------------------------------------------\nHash Dictionary:");

            foreach (var element in dictionary)
            {
                result.AppendLine($"\t{element.Key}: {element.Value}");
            }

            result.AppendLine("---------------------------------------------------");

            return result.ToString();
        }

        public static string GetStringFromErrors(List<string> errors)
        {
            var result = new StringBuilder();

            result.AppendLine("---------------------------------------------------\nErrors:");

            foreach (var error in errors)
            {
                result.AppendLine($"\t{error}");
            }

            result.AppendLine($"Total errors count: {errors.Count}");
            result.AppendLine("---------------------------------------------------");

            return result.ToString();
        }

        public static string GetStringFromIntStack(Stack<int> output)
        {
            var result = new StringBuilder();

            result.AppendLine($"---------------------------------------------------\nProgram code:");

            result.AppendLine(string.Join("\n", output.Select(_ => _.ToStringFromHash())).Replace(' ', '~'));

            return result.ToString();
        }
    }
}
