using CBASLanguageInterpreter.Helpers;
using System.Collections.Generic;

namespace CBASLanguageInterpreter.Entities
{
    public class State
    {
        public List<int> Lexemes { get; set; } = new List<int>();

        public override string ToString()
        {
            string line = "";

            foreach (var lexeme in Lexemes)
            {
                line += lexeme.ToStringFromHash();
            }

            return line;
        }

        public bool HasEmptyWord() => Lexemes.Count == 0 || Lexemes.Contains(0);
    }
}
