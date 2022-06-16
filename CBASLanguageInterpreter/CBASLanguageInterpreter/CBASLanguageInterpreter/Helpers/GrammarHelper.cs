using CBASLanguageInterpreter.Entities;
using CBASLanguageInterpreter.Interpretation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CBASLanguageInterpreter.Helpers
{
    static class GrammarHelper
    {
        public static HashDictionary GrammarHashDictionary = new HashDictionary();

        public static (Dictionary<Record, Rule>, List<Rule>) ProcessGrammar(
            out Dictionary<int, List<int>> FIRST,
            out Dictionary<int, List<int>> FOLLOW)
        {
            ReadFile(out List<Rule> rules);

            return (GetAnalysisTable(rules, out FIRST, out FOLLOW), rules);
        }

        public static bool ReadFile(out List<Rule> rules)
        {
            string filename = "Grammar_3.0.txt";
            rules = new List<Rule>();
            string line;
            int counter = 0;

            try
            {
                using (StreamReader file = new StreamReader(filename))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        line = Regex.Replace(line, "'", string.Empty);

                        if (Regex.IsMatch(line, @"^<[^<>]{1,}>:.+( \| .{1,}){0,}$"))
                        {
                            string left = Regex.Match(line, @"^<[^<>]{1,}>").Value;
                            line = line.Remove(0, left.Length + 2);
                            string state = Regex.Match(line, @"^[^|]+").Value;
                            state = state.TrimEnd(' ');
                            line = line.Remove(0, state.Length);

                            GrammarHashDictionary.TryAddLexeme(left);

                            rules.Add(
                                new Rule()
                                {
                                    PrevState = left.GetHashCode(),
                                    NewStates = new List<State>()
                                });

                            rules.Last().AddState(state);

                            if (left == "symbol>")
                            {
                                GrammarHashDictionary.TryAddLexeme(" ");
                                rules.Last().AddState(" ");
                            }

                            while (Regex.IsMatch(line, @"^ \| [^|]{1,}"))
                            {
                                line = line.Remove(0, 3);
                                state = Regex.Match(line, @"^[^|]{1,}").Value;
                                state = state.TrimEnd(' ');
                                line = line.Remove(0, state.Length);
                                rules.Last().AddState(state);
                            }
                        }
                        else
                        {
                            //Console.WriteLine($"Syntax error in file (line {counter}: '{line}' does not match the pattern");
                        }
                        counter++;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);

                return false;
            }
        }

        public static Dictionary<Record, Rule> GetAnalysisTable(
            List<Rule> rules,
            out Dictionary<int, List<int>> FIRST,
            out Dictionary<int, List<int>> FOLLOW)
        {
            var analysisTable = new Dictionary<Record, Rule>();

            GetTerminalsAndNonterminals(rules, out List<int> terminals, out List<int> nonterminals);

            FIRST = GetFIRST(rules, terminals);
            FOLLOW = GetFOLLOW(FIRST, rules, nonterminals);

            foreach (var rule in rules)
            {
                foreach (var state in rule.NewStates)
                {
                    if (FIRST.ContainsKey(state.Lexemes.FirstOrDefault()))
                    {
                        foreach (var terminal in FIRST[state.Lexemes.FirstOrDefault()])
                        {
                            try
                            {
                                analysisTable.Add(
                                new Record()
                                {
                                    Nonterminal = rule.PrevState,
                                    Terminal = terminal
                                },
                                new Rule()
                                {
                                    PrevState = rule.PrevState,
                                    NewStates = new List<State>() { state }
                                });
                            }
                            catch (Exception) { }
                        }

                        if (FIRST[state.Lexemes.FirstOrDefault()].Contains(0))
                        {
                            foreach (var terminal in FOLLOW[rule.PrevState])
                            {
                                try
                                {
                                    analysisTable.Add(
                                        new Record()
                                        {
                                            Nonterminal = rule.PrevState,
                                            Terminal = terminal
                                        },
                                        new Rule()
                                        {
                                            PrevState = rule.PrevState,
                                            NewStates = new List<State>() { state }
                                        });
                                }
                                catch (Exception) { }
                            }

                            if (FOLLOW[rule.PrevState].Contains(0))
                            {
                                try
                                {
                                    analysisTable.Add(
                                    new Record()
                                    {
                                        Nonterminal = rule.PrevState,
                                        Terminal = 0
                                    },
                                    new Rule()
                                    {
                                        PrevState = rule.PrevState,
                                        NewStates = new List<State>() { state }
                                    });
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                }
            }

            return analysisTable;
        }

        public static void GetTerminalsAndNonterminals(
            List<Rule> rules,
            out List<int> terminals,
            out List<int> nonterminals)
        {
            terminals = new List<int>();
            nonterminals = new List<int>();

            foreach (var rule in rules)
            {
                AddToList(ref nonterminals, rule.PrevState);

                foreach (var state in rule.NewStates)
                {
                    foreach (var word in state.Lexemes)
                    {
                        if (!IsTerminal(word.ToStringFromHash()))
                        {
                            AddToList(ref nonterminals, word);
                        }
                        else
                        {
                            AddToList(ref terminals, word);
                        }
                    }
                }
            }
        }

        public static void AddToList(ref List<int> list, int element)
        {
            if (!list.Contains(element))
            {
                list.Add(element);
            }
        }

        public static Dictionary<int, List<int>> GetFOLLOW(
            Dictionary<int, List<int>> FIRST,
            List<Rule> rules,
            List<int> nonterminals)
        {
            Dictionary<int, List<int>> FOLLOW = new Dictionary<int, List<int>>();

            foreach (var nonterminal in nonterminals)
            {
                AddElementToDictionary(ref FOLLOW, nonterminal, 0);
            }

            int index;
            int temp;
            bool wasChanged = true;

            while (wasChanged)
            {
                wasChanged = false;

                foreach (var rule in rules)
                {
                    foreach (var state in rule.NewStates)
                    {
                        if (state.Lexemes.LastOrDefault() != 0
                            && !IsTerminal(state.Lexemes.LastOrDefault().ToStringFromHash()))
                        {
                            wasChanged |= AddElementsToDictionary(
                                ref FOLLOW,
                                state.Lexemes.Last(),
                                FOLLOW[rule.PrevState].Except(nonterminals).ToList());
                        }

                        for (int i = state.Lexemes.Count - 2; i >= 0; i--)
                        {
                            if (!IsTerminal(state.Lexemes[i].ToStringFromHash()))
                            {
                                wasChanged |= AddElementsToDictionary(
                                    ref FOLLOW,
                                    state.Lexemes[i],
                                    FIRST[state.Lexemes[i + 1]].Except(new List<int>() { 0 }).ToList());
                            }
                        }

                        temp = state.Lexemes.LastOrDefault(_ => FIRST[_].Contains(0));

                        if (temp != 0)
                        {
                            index = state.Lexemes.IndexOf(temp);

                            if (index > 0 && !IsTerminal(state.Lexemes[index - 1].ToStringFromHash()))
                            {
                                wasChanged |= AddElementsToDictionary(
                                    ref FOLLOW,
                                    state.Lexemes[index - 1],
                                    FOLLOW[state.Lexemes[index]].Except(nonterminals).ToList());
                            }
                        }
                    }
                }
            }

            return FOLLOW;
        }

        public static Dictionary<int, List<int>> GetFIRST(
            List<Rule> rules,
            List<int> terminals)
        {
            Dictionary<int, List<int>> FIRST = new Dictionary<int, List<int>>();

            foreach (var terminal in terminals)
            {
                AddElementToDictionary(ref FIRST, terminal, terminal);
            }

            foreach (var rule in rules)
            {
                if (rule.HasEmptyStateInNewStates())
                {
                    AddElementToDictionary(ref FIRST, rule.PrevState, 0);
                }

                var Lexemes = rule.GetFirstLexemesInNewStates();
                foreach (var word in Lexemes)
                {
                    AddElementToDictionary(ref FIRST, rule.PrevState, word);
                }
            }

            List<int> hasOnlyTerminals = new List<int>();

            while (hasOnlyTerminals.Count != FIRST.Count)
            {
                foreach (var element in FIRST)
                {
                    if (IsTerminalsOnly(element.Value))
                    {
                        if (!hasOnlyTerminals.Contains(element.Key))
                        {
                            hasOnlyTerminals.Add(element.Key);
                        }

                        foreach (var another in FIRST)
                        {
                            if (!IsTerminalsOnly(another.Value) && FIRST[another.Key].Contains(element.Key))
                            {
                                FIRST[another.Key].Remove(element.Key);
                                AddElementsToDictionary(ref FIRST, another.Key, element.Value);
                            }
                        }
                    }
                }
            }

            //PrintHelper.PrintDictionaryWithListValue(FIRST, "FIRST");

            return FIRST;
        }

        public static bool IsTerminalsOnly(List<int> list) => list.All(_ => IsTerminal(_.ToStringFromHash()));

        public static bool AddElementsToDictionary(
            ref Dictionary<int, List<int>> dictionary,
            int key,
            List<int> values)
        {
            if (dictionary.ContainsKey(key))
            {
                int count = dictionary[key].Count;
                dictionary[key].AddRange(values.Except(dictionary[key]));

                return count != dictionary[key].Count;
            }
            else
            {
                dictionary.Add(key, new List<int>(values));

                return true;
            }
        }

        public static void AddElementToDictionary(
            ref Dictionary<int, List<int>> dictionary,
            int key,
            int value)
        {
            if (dictionary.ContainsKey(key))
            {
                if (!dictionary[key].Contains(value))
                {
                    dictionary[key].Add(value);
                }
            }
            else
            {
                dictionary.Add(key, new List<int>() { value });
            }
        }

        public static bool IsTerminal(string element) => !Regex.IsMatch(element, @"^<[^<>]{1,}>$");
    }
}
