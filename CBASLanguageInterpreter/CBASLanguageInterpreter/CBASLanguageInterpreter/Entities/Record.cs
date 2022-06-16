using CBASLanguageInterpreter.Helpers;
using System;

namespace CBASLanguageInterpreter.Entities
{
    public class Record : IEquatable<Record>
    {
        public int Terminal { get; set; }
        public int Nonterminal { get; set; }

        public bool Equals(Record other)
        {
            if (other is null)
            {
                return false;
            }

            return this.Terminal == other.Terminal
                && this.Nonterminal == other.Nonterminal;
        }

        public override bool Equals(object obj) => Equals(obj as Record);
        public override int GetHashCode() => new Tuple<int, int>(Terminal, Nonterminal).GetHashCode();

        public override string ToString()
            => $"M[{Nonterminal.ToStringFromHash()}, {Terminal.ToStringFromHash() ?? "$"}]";
    }
}
