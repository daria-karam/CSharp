using CBASLanguageInterpreter.Entities;
using CBASLanguageInterpreter.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CBASLanguageInterpreter.Interpretation
{
    public class SyntaxAnalyzer
    {
        private readonly Dictionary<Record, Rule> _analysisTable;
        private readonly Dictionary<int, List<int>> _FIRST;
        private readonly Dictionary<int, List<int>> _FOLLOW;
        private readonly List<Rule> _rules;

        public SyntaxAnalyzer()
        {
            (_analysisTable, _rules) = GrammarHelper.ProcessGrammar(out _FIRST, out _FOLLOW);

            HtmlHelper.SaveAnalysisTable(_analysisTable);
            HtmlHelper.SaveFIRSTorFOLLOW(_FIRST, "FIRST");
            HtmlHelper.SaveFIRSTorFOLLOW(_FOLLOW, "FOLLOW");
        }

        /// <summary>
        /// Check the input string for errors
        /// </summary>
        /// <param name="output">Output stack with grammar lexemes</param>
        /// <param name="line">Input string</param>
        /// <returns>(Information for print, is input string correct)</returns>
        public (string, bool) Analyse(out Stack<int> output, string line)
        {
            var information = new StringBuilder();

            information.AppendLine(StringHelper.GetStringFromRules(_rules));
            information.AppendLine(StringHelper.GetStringFromDictionaryWithListValue(_FIRST, "FIRST"));
            information.AppendLine(StringHelper.GetStringFromDictionaryWithListValue(_FOLLOW, "FOLLOW"));

            output = new Stack<int>();
            int index = 0;
            List<string> errors = new List<string>();

            string htmlHistoryFilename = "process_table_" + line.GetHashCode() + ".html";
            HtmlHelper.InitHtmlHistory(out string htmlHistory);

            try
            {
                Stack<int> stack = new Stack<int>();
                stack.Push(0);
                stack.Push("<program>".GetHashCode());

                string input = line + "$";

                string current;
                string onStackTop;
                Record record = new Record();

                do
                {
                    onStackTop = stack.Peek().ToStringFromHash();
                    current = GetNextElementCBAS(input, onStackTop);

                    HtmlHelper.AddRowToHtmlHistory(ref htmlHistory, stack, input, current);
                    information.AppendLine(StringHelper.GetStringFromAnalysisState(stack, input, current.GetHashCode()));

                    if(GrammarHelper.IsTerminal(onStackTop) || stack.Peek() == 0)
                    {
                        if(IsItTheSameTerminal(stack.Peek(), current))
                        {
                            stack.Pop();
                            input = input.Remove(0, current.Length);
                            index += current.Length;

                            output.Push(GetTerminalAsInGrammarFile(current).GetHashCode());
                        }
                        else
                        {
                            ProcessError(ref errors, ref information, ref htmlHistory, ref stack, ref input, index);
                        }
                    }
                    else
                    {
                        record.Nonterminal = stack.Peek();
                        record.Terminal = GetTerminalAsInGrammarFile(current).GetHashCode();

                        if(_analysisTable.ContainsKey(record))
                        {
                            stack.Pop();

                            var collection = _analysisTable[record].NewStates.First().Lexemes;

                            for(int i = collection.Count - 1; i >= 0; i--)
                            {
                                if(collection[i] != 0)
                                {
                                    stack.Push(collection[i]);
                                }
                            }
                        }
                        else
                        {
                            record.Terminal = 0;

                            if(_analysisTable.ContainsKey(record))
                            {
                                stack.Pop();
                                var collection = _analysisTable[record].NewStates.First().Lexemes;

                                for(int i = collection.Count - 1; i >= 0; i--)
                                {
                                    if(collection[i] != 0)
                                    {
                                        stack.Push(collection[i]);
                                    }
                                }
                            }
                            else
                            {
                                ProcessError(ref errors, ref information, ref htmlHistory, ref stack, ref input, index);
                            }
                        }
                    }
                } while(stack.Count != 0 && stack.Peek() != 0);

                HtmlHelper.AddRowToHtmlHistory(ref htmlHistory, stack, input, string.Empty);
                HtmlHelper.CloseHtmlHistory(ref htmlHistory);
                HtmlHelper.AddErrorsToHtmlHistory(ref htmlHistory, errors);
                HtmlHelper.SaveHtmlHistory(htmlHistory, htmlHistoryFilename);

                information.AppendLine(StringHelper.GetStringFromAnalysisState(stack, input, 0));
                information.AppendLine(StringHelper.GetStringFromErrors(errors));

                if(input != "$" || errors.Count > 0)
                {
                    information.AppendLine("Fail! :(");

                    return (information.ToString(), false);
                }
            }
            catch(Exception e)
            {
                information.AppendLine(StringHelper.GetStringFromErrors(errors));
                information.AppendLine($"ERROR! {e.Message}");
                information.AppendLine("Fail! :(");

                return (information.ToString(), false);
            }

            information.AppendLine("Success! :)");

            return (information.ToString(), true);
        }

        #region process error

        private void ProcessError(
            ref List<string> errors,
            ref StringBuilder information,
            ref string htmlHistory,
            ref Stack<int> stack,
            ref string line, int index)
        {
            AddErrorToErrors(ref errors, stack, line, index);
            information.AppendLine($"The symbol '{stack.Peek().ToStringFromHash()}' was not received");

            while (GrammarHelper.IsTerminal(stack.Peek().ToStringFromHash()) && stack.Peek() != 0)
            {
                HtmlHelper.AddRowToHtmlHistory(ref htmlHistory, stack, line, string.Empty,
                    $"Removing the terminal {stack.Peek().ToStringFromHash()} from the top of the stack");
                stack.Pop();
            }

            try
            {
                while (true)
                {
                    HtmlHelper.AddRowToHtmlHistory(ref htmlHistory, stack, line, string.Empty,
                        $"Deleting character '{line[0]}' from the input string");

                    line = line.Remove(0, 1); //убирать не посимвольно, а по лексемам

                    foreach(var element in _FIRST[stack.Peek()])
                    {
                        if(line.IndexOf(element.ToStringFromHash()) == 0)
                        {
                            var record = new Record()
                            {
                                Nonterminal = stack.Peek(),
                                Terminal = element
                            };

                            if(_analysisTable.ContainsKey(record))
                            {
                                stack.Pop();
                                var collection = _analysisTable[record].NewStates.First().Lexemes;
                                string str = "";

                                for(int i = collection.Count - 1; i >= 0; i--)
                                {
                                    if(collection[i] != 0)
                                    {
                                        str += collection[i].ToStringFromHash();
                                        stack.Push(collection[i]);
                                    }
                                }

                                HtmlHelper.AddRowToHtmlHistory(
                                    ref htmlHistory, stack, line, string.Empty,
                                    $"Adding '{str}' to the stack");

                                return;
                            }
                        }
                    }

                    foreach(var element in _FOLLOW[stack.Peek()])
                    {
                        if(element != 0 && line.IndexOf(element.ToStringFromHash()) == 0)
                        {
                            HtmlHelper.AddRowToHtmlHistory(ref htmlHistory, stack, line, string.Empty,
                                $"Removing the nonterminal {stack.Peek().ToStringFromHash()} from the top of the stack");

                            stack.Pop();

                            return;
                        }
                    }
                }
            }
            catch(Exception) { }
        }

        private void AddErrorToErrors(ref List<string> errors, Stack<int> stack, string input, int index)
        {
            string first = input.Length > 10 ? input.Remove(10) + "..." : input;

            errors.Add($"{GetErrorType(input)} at position {index}."
                + $" '{stack.Peek().ToStringFromHash()}' was expected, '{first}' was received.");
        }

        private string GetErrorType(string input)
        {
            string lexeme = GetNextElementCBAS(input);

            if(lexeme.Length != 1 && _FIRST.Any(_ => _.Value.Contains(lexeme.GetHashCode())))
            {
                return "Semantic error";
            }

            return "Lexical error";
        }

        #endregion

        #region work with terminals

        private bool IsItTheSameTerminal(int onStackTop, string current)
        {
            string expected = onStackTop.ToStringFromHash();

            return GetTerminalAsInGrammarFile(current) == expected;
        }

        private string GetTerminalAsInGrammarFile(string terminal)
        {
            if(Regex.IsMatch(terminal, @"^\s*(scan|print)\s+$"))
            {
                return Regex.Replace(Regex.Replace(terminal, @"\s+$", " "), @"^\s+", string.Empty);
            }

            if(Regex.IsMatch(terminal, @"^\s+(to\s+)?$"))
            {
                return Regex.Replace(terminal, @"\s+$", " ");
            }

            return Regex.Replace(terminal, @"\s+", string.Empty);
        }

        private string GetNextElementCBAS(string line, string onStackTop = null)
        {
            if(!string.IsNullOrEmpty(onStackTop) && (onStackTop == " " || onStackTop == "<statement_end>"))
            {
                return Regex.Match(line, @"^\s+").Value;
            }

            foreach(var expression in CBASTerminalRegularExpressions)
            {
                if(Regex.IsMatch(line, expression))
                {
                    return Regex.Match(line, expression).Value;
                }
            }

            return line[0].ToString();
        }

        private readonly List<string> CBASTerminalRegularExpressions = new List<string>()
        {
            @"^\s*scan\s+",
            @"^\s*print\s+",
            @"^\s*for\s*\(\s*",
            @"^\s*if\s*\(\s*",
            @"^\s*else\s*{\s*",
            @"^\s+to\s+",
            @"^\s*(<|>|==|\!=)\s*",
            @"^\s*(-|/|=|,|{)\s*",
            @"^\s*(\*|\+)\s*",
            @"^\s*(;|}|\))",
            @"^[a-zA-Z0-9_-{}[~%\(\)\#\$*+=@<>!&^№\\/\]?\.\,\;\:-]",
            @"^\s+",
            @"^""",
        };

        #endregion
    }
}
