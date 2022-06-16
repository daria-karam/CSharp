using CBASLanguageInterpreter.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CBASLanguageInterpreter.Helpers
{
    public static class HtmlHelper
    {
        public static void AddRowToHtmlHistory(
            ref string htmlHistory,
            Stack<int> stack,
            string input,
            string current,
            string note = null)
        {
            htmlHistory += "\n\t<tr>";

            string line = "";

            foreach (var element in stack)
            {
                line = element.ToStringFromHash() + line;
            }

            htmlHistory += $"\n\t\t<td>{line.Replace("<", "&lt").Replace(">", "&gt")}</td>";
            htmlHistory += $"\n\t\t<td>{input.Replace("<", "&lt").Replace(">", "&gt")}</td>";
            htmlHistory += $"\n\t\t<td>{(!string.IsNullOrEmpty(current) ? current : "$")}</td>";
            htmlHistory += $"\n\t\t<td>{note?.Replace("<", "&lt").Replace(">", "&gt")}</td>";

            htmlHistory += "\n\t</tr>";
        }

        public static void InitHtmlHistory(out string htmlHistory)
        {

            htmlHistory = "<pre><table border=1 cellpadding=10px bgcolor=#aacff2>";
            htmlHistory += "\n\t<tr>";
            htmlHistory += "\n\t\t<td width=400px>Stack</td>";
            htmlHistory += "\n\t\t<td width=400px>Input</td>";
            htmlHistory += "\n\t\t<td width=100px>Current</td>";
            htmlHistory += "\n\t\t<td width=400px>Note</td>";
            htmlHistory += "\n\t</tr>";
        }

        public static void AddErrorsToHtmlHistory(ref string htmlHistory, List<string> errors)
        {
            htmlHistory += "</br>Errors:<ul>";
            foreach (var error in errors)
            {
                htmlHistory += $"<li>{error.Replace("<", "&lt").Replace(">", "&gt")}</li>";
            }
            htmlHistory += $"</ul>Total errors count: {errors.Count}";
        }

        public static void CloseHtmlHistory(ref string htmlHistory)
        {
            htmlHistory += "</table></pre>\n";
        }

        public static void SaveHtmlHistory(string htmlHistory, string htmlHistoryFilename)
        {
            using (StreamWriter sw = new StreamWriter(
                Path.Combine("Results", htmlHistoryFilename), false, Encoding.Default))
            {
                sw.WriteLine(htmlHistory);
            }
        }

        public static void SaveAnalysisTable(Dictionary<Record, Rule> analysisTable)
        {
            string table = "<table border=1 cellpadding=10px bgcolor=#aacff2>";
            table += "\n\t<tr>";
            table += "\n\t\t<td width=150px>Nonterminal</td>";
            table += "\n\t\t<td width=150px>Terminal</td>";
            table += "\n\t\t<td width=150px>Rule (previous state)</td>";
            table += "\n\t\t<td width=300px>Rule (next state)</td>";
            table += "\n\t</tr>";

            string nonterminal, terminal, prevState, newState;
            foreach (var element in analysisTable)
            {
                nonterminal = element.Key.Nonterminal.ToStringFromHash().Replace("<", "&lt").Replace(">", "&gt");
                terminal = element.Key.Terminal.ToStringFromHash();
                prevState = element.Value.PrevState.ToStringFromHash().Replace("<", "&lt").Replace(">", "&gt");
                newState = element.Value.NewStates.First().ToString().Replace("<", "&lt").Replace(">", "&gt");
                table += "\n\t<tr>";
                table += $"\n\t\t<td>{nonterminal}</td>";
                table += $"\n\t\t<td>{(!string.IsNullOrEmpty(terminal) ? terminal : "$")}</td>";
                table += $"\n\t\t<td>{prevState}</td>";
                table += $"\n\t\t<td>{(!string.IsNullOrEmpty(newState) ? newState : "$")}</td>";
                table += "\n\t</tr>";
            }

            table += "</table>\n";

            using (StreamWriter sw = new StreamWriter(
                Path.Combine("Results", "analysis_table.html"), false, Encoding.Default))
            {
                sw.WriteLine(table);
            }
        }

        public static void SaveFIRSTorFOLLOW(Dictionary<int, List<int>> dictionary, string title)
        {
            var table = new StringBuilder("<table border=1 cellpadding=10px bgcolor=#aacff2>");
            table.Append("\n\t<tr>");
            table.Append("\n\t\t<td width=150px>Lexeme</td>");
            table.Append($"\n\t\t<td width=1500px>{title}</td>");
            table.Append("\n\t</tr>");

            string line;

            foreach (var element in dictionary)
            {
                if (!(element.Key == element.Value.First() && element.Value.Count == 1))
                {
                    var items = element.Value
                        .Select(_ => _.ToStringFromHash()?.Replace("<", "&lt").Replace(">", "&gt"))
                        .Select(_ => string.IsNullOrEmpty(_) ? "$" : _);

                    line = string.Join(" | ", items);

                    table.Append("\n\t<tr>");
                    table.Append($"\n\t\t<td>{element.Key.ToStringFromHash().Replace("<", "&lt").Replace(">", "&gt")}</td>");
                    table.Append($"\n\t\t<td>{line}</td>");
                    table.Append("\n\t</tr>");
                }
            }

            table.Append("</table>\n");

            using (StreamWriter sw = new StreamWriter(
                Path.Combine("Results", $"{title}.html"), false, Encoding.Default))
            {
                sw.WriteLine(table.ToString());
            }
        }
    }
}
