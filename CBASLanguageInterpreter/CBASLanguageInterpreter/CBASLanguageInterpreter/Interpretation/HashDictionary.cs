using System.Collections.Generic;

namespace CBASLanguageInterpreter.Interpretation
{
    public class HashDictionary
    {
        private Dictionary<int, string> _lexemes
            = new Dictionary<int, string>() { { 0, string.Empty } };

        public bool TryAddLexeme(string lexeme)
        {
            if (!_lexemes.ContainsKey(lexeme.GetHashCode()))
            {
                _lexemes.Add(lexeme.GetHashCode(), lexeme);

                return true;
            }

            return false;
        }

        public void TryDeleteLexeme(string lexeme = null, int hash = 0)
        {
            if (!string.IsNullOrEmpty(lexeme) && _lexemes.ContainsKey(lexeme.GetHashCode()))
            {
                _lexemes.Remove(lexeme.GetHashCode());
            }
            else
            {
                _lexemes.Remove(hash);
            }
        }

        public string TryGetLexeme(int hash)
        {
            if (_lexemes.ContainsKey(hash))
            {
                return _lexemes[hash];
            }

            return null;
        }
    }
}
