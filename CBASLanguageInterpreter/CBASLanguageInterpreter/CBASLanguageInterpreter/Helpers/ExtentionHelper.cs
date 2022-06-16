using System.Collections.Generic;

namespace CBASLanguageInterpreter.Helpers
{
    public static class ExtentionHelper
    {
        public static string ToStringFromHash(this int hash)
            => GrammarHelper.GrammarHashDictionary.TryGetLexeme(hash);

        public static Stack<int> Reverse(this Stack<int> oldStack)
        {
            oldStack = new Stack<int>(oldStack);

            return oldStack;
        }

        public static Stack<int> Copy(this Stack<int> oldStack)
        {
            var newStack = new Stack<int>(oldStack);

            return newStack.Reverse();
        }
    }
}
