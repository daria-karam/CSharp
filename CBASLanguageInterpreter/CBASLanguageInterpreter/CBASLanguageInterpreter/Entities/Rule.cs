using CBASLanguageInterpreter.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CBASLanguageInterpreter.Entities
{
    public class Rule
    {
        public int PrevState { get; set; }
        public List<State> NewStates { get; set; } = new List<State>();

        public void AddState(string state)
        {
            State newState = new State() { Lexemes = new List<int>() };

            if (string.IsNullOrEmpty(state))
            {
                newState.Lexemes.Add(0);
            }

            string word;

            while (state.Length > 0)
            {
                if (Regex.IsMatch(state, @"^<[^<>]{1,}>"))
                {
                    word = Regex.Match(state, @"^<[^<>]{1,}>").Value;
                }
                else
                {
                    word = Regex.Match(state, @"^[^<>]{0,}<?>?").Value;

                    if (word.Length > 1)
                    {
                        word = word.TrimEnd('<').TrimEnd('>');
                    }
                }

                state = state.Remove(0, word.Length);

                GrammarHelper.GrammarHashDictionary.TryAddLexeme(word);

                newState.Lexemes.Add(word.GetHashCode());
            }

            NewStates.Add(newState);
        }

        public List<int> GetFirstLexemesInNewStates()
        {
            List<int> first = new List<int>();

            foreach (var state in NewStates)
            {
                first.Add(state.Lexemes.FirstOrDefault());
            }

            return first;
        }

        public bool HasEmptyStateInNewStates()
        {
            foreach (var state in NewStates)
            {
                if (state.HasEmptyWord())
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            string line = $"{PrevState.ToStringFromHash()}: ";

            foreach (var state in NewStates)
            {
                line += $"{state} | ";
            }

            line = line.Remove(line.Length - 3);

            return line;
        }
    }
}
