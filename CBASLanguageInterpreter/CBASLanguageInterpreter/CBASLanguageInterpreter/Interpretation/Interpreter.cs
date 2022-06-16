using CBASLanguageInterpreter.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CBASLanguageInterpreter.Interpretation
{
    public static class Interpreter
    {
        private static HashDictionary _programHashDictionary = new HashDictionary();

        private static Dictionary<int, int> _variables = new Dictionary<int, int>();

        private static Dictionary<int, string> _lexemeRegularExpressions = new Dictionary<int, string>
        {
            { "<character>".GetHashCode(), @"^[a-zA-Z_]$"},
            { "<digit>".GetHashCode(), @"^[0-9]$"},
            { "<quote>".GetHashCode(), @"^""$"},
            { "<identifier>".GetHashCode(), @"^[a-zA-Z_]+$"},
            { "<number>".GetHashCode(), @"^[0-9]+$"},
        };

        public static void Interpret(Stack<int> input, bool isStart = false)
        {
            if (isStart)
            {
                _programHashDictionary = new HashDictionary();
            }

            while(input.Count > 0)
            {
                Process(ref input);
            }
        }

        public static int Process(ref Stack<int> input)
        {
            while(input.Count > 0 && input.Peek() == " ".GetHashCode())
            {
                input.Pop(); // removing ' '
            }

            if(input.Count == 0)
            {
                return 0;
            }

            var current = input.Peek().ToStringFromHash();

            return current switch
            {
                "if(" => ProcessIf(ref input),
                "for(" => ProcessFor(ref input),
                "print " => ProcessPrint(ref input),
                "scan " => ProcessScan(ref input),
                var value
                    when Regex.IsMatch(value, _lexemeRegularExpressions["<character>".GetHashCode()])
                        => ProcessAssign(ref input),
                _ => throw new Exception($"PANIC! Lexeme: {current}"),
            };
        }

        #region process: identifier, number, string

        public static int ProcessIdentifierOrNumberOrString(ref Stack<int> input)
        {
            while(input.Peek() == " ".GetHashCode())
            {
                input.Pop(); // removing ' '
            }

            var current = input.Peek().ToStringFromHash();

            return current switch
            {
                var value
                    when Regex.IsMatch(value, _lexemeRegularExpressions["<character>".GetHashCode()])
                        => ProcessIdentifier(ref input),

                var value
                    when Regex.IsMatch(value, _lexemeRegularExpressions["<digit>".GetHashCode()])
                        => ProcessNumber(ref input),

                var value
                    when Regex.IsMatch(value, _lexemeRegularExpressions["<quote>".GetHashCode()])
                        => ProcessString(ref input),

                _ => throw new Exception($"PANIC! Lexeme: {current}"),
            };
        }

        public static int ProcessIdentifier(ref Stack<int> input)
        {
            var regex = _lexemeRegularExpressions["<character>".GetHashCode()];

            var identifier = new StringBuilder("");

            while(Regex.IsMatch(input.Peek().ToStringFromHash(), regex))
            {
                identifier.Append(input.Pop().ToStringFromHash());
            }

            var hash = identifier.ToString().GetHashCode();

            if(_programHashDictionary.TryAddLexeme(identifier.ToString()))
            {
                _variables[hash] = 0;
            }

            return hash;
        }

        public static int ProcessNumber(ref Stack<int> input)
        {
            var regex = _lexemeRegularExpressions["<digit>".GetHashCode()];

            var number = new StringBuilder("");

            while(Regex.IsMatch(input.Peek().ToStringFromHash(), regex))
            {
                number.Append(input.Pop().ToStringFromHash());
            }

            var hash = number.ToString().GetHashCode();

            if(_programHashDictionary.TryAddLexeme(number.ToString()))
            {
                _variables[hash] = int.Parse(number.ToString());
            }

            return hash;
        }

        public static int ProcessString(ref Stack<int> input)
        {
            var lexeme = new StringBuilder(input.Pop().ToStringFromHash());

            while(input.Peek().ToStringFromHash() != @"""")
            {
                lexeme.Append(input.Pop().ToStringFromHash());
            }

            lexeme.Append(input.Pop().ToStringFromHash());

            _programHashDictionary.TryAddLexeme(lexeme.ToString());

            return lexeme.ToString().GetHashCode();
        }

        #endregion

        #region process: expression, term, factor

        public static int ProcessExpression(ref Stack<int> input)
        {
            var term = ProcessTerm(ref input);

            if(input.Peek() == "+".GetHashCode())
            {
                input.Pop(); // removing '+'

                var expressionEnd = ProcessExpression(ref input);

                var result = _variables[term] + _variables[expressionEnd];

                _variables[result.GetHashCode()] = result;

                return result.GetHashCode();
            }

            if(input.Peek() == "-".GetHashCode())
            {
                input.Pop(); // removing '-'

                var expressionEnd = ProcessExpression(ref input);

                var result = _variables[term] - _variables[expressionEnd];

                _variables[result.GetHashCode()] = result;

                return result.GetHashCode();
            }

            return term;
        }

        public static int ProcessTerm(ref Stack<int> input)
        {
            var factor = ProcessFactor(ref input);

            if(input.Peek() == "*".GetHashCode())
            {
                input.Pop(); // removing '*'

                var termEnd = ProcessTerm(ref input);

                var result = _variables[factor] * _variables[termEnd];

                _variables[result.GetHashCode()] = result;

                return result.GetHashCode();
            }

            if(input.Peek() == "/".GetHashCode())
            {
                input.Pop(); // removing '/'

                var termEnd = ProcessTerm(ref input);

                var result = _variables[factor] / _variables[termEnd];

                _variables[result.GetHashCode()] = result;

                return result.GetHashCode();
            }

            return factor;
        }

        public static int ProcessFactor(ref Stack<int> input)
        {
            if(input.Peek() == "(".GetHashCode())
            {
                input.Pop(); // removing '('

                var result = ProcessExpression(ref input);

                input.Pop(); // removing ')'

                return result;
            }

            return ProcessIdentifierOrNumberOrString(ref input);
        }

        #endregion

        #region process: assign

        public static int ProcessAssign(ref Stack<int> input)
        {
            var identifier = ProcessIdentifier(ref input);

            input.Pop(); // removing '='

            //var hash = ProcessNumber(ref input);
            var hash = ProcessExpression(ref input);

            _variables[identifier] = _variables[hash];

            input.Pop(); // removing ';'

            return 0;
        }

        #endregion

        #region process: if, else, bool expression

        public static bool ProcessBoolExpression(ref Stack<int> input)
        {
            var first = _variables[ProcessExpression(ref input)];

            var relop = input.Pop();

            var second = _variables[ProcessExpression(ref input)];

            return relop.ToStringFromHash() switch
            {
                "<" => first < second,
                ">" => first > second,
                "==" => first == second,
                "!=" => first != second,
                _ => throw new Exception($"PANIC! Lexeme: {relop.ToStringFromHash()}"),
            };
        }

        public static int ProcessIf(ref Stack<int> input)
        {
            input.Pop(); // removing 'if('

            var result = ProcessBoolExpression(ref input);

            input.Pop(); // removing ')'

            var ifBody = GetCodeBlock(ref input);
            var elseBody = GetCodeBlock(ref input);

            if(result)
            {
                Interpret(ifBody);
            }
            else
            {
                Interpret(elseBody);
            }

            return 0;
        }

        #endregion

        #region process: for

        public static int ProcessFor(ref Stack<int> input)
        {
            input.Pop(); // removing 'for('

            var identifier = ProcessIdentifier(ref input);

            input.Pop(); // removing '='

            var from = ProcessExpression(ref input);

            input.Pop(); // removing ' to '

            var to = ProcessExpression(ref input);

            input.Pop(); // removing ')'

            var body = GetCodeBlock(ref input);

            for(var i = _variables[from]; i < _variables[to]; i++)
            {
                _variables[identifier] = i;

                Interpret(body.Copy());
            }

            return 0;
        }

        #endregion

        #region process: print, scan

        public static int ProcessPrint(ref Stack<int> input)
        {
            input.Pop(); // removing 'print '

            int hash;

            while(input.Peek() != ";".GetHashCode())
            {
                if(input.Peek() == ",".GetHashCode())
                {
                    input.Pop(); // removing ','
                    Console.Write(", ");
                }

                if(input.Peek() == @"""".GetHashCode())
                {
                    hash = ProcessString(ref input);
                    Console.Write(_programHashDictionary.TryGetLexeme(hash).Replace(@"""", string.Empty));
                }
                else
                {
                    hash = ProcessExpression(ref input);
                    Console.Write(_variables[hash]);
                }
            }

            Console.WriteLine();

            input.Pop(); // removing ';'

            return 0;
        }

        public static int ProcessScan(ref Stack<int> input)
        {
            input.Pop(); // removing 'scan '

            var identifier = ProcessIdentifier(ref input);

            Console.Write($"Enter {_programHashDictionary.TryGetLexeme(identifier)}: ");

            var value = Console.ReadLine(); //checking for numbers only

            while (!int.TryParse(value, out int _))
            {
                Console.Write("Not an integer. Try again: ");
                value = Console.ReadLine();
            }

            _variables[identifier] = int.Parse(value);

            input.Pop(); // removing ';'

            return 0;
        }

        #endregion

        public static Stack<int> GetCodeBlock(ref Stack<int> input)
        {
            var block = new Stack<int>();

            var countOfOpenedBrackets = 1;

            input.Pop(); // removing '{' or 'else{'

            string current;

            while(countOfOpenedBrackets != 0)
            {
                current = input.Peek().ToStringFromHash();

                if(current.Contains("{"))
                {
                    countOfOpenedBrackets++;
                }
                else if(current.Contains("}"))
                {
                    countOfOpenedBrackets--;
                }

                block.Push(input.Pop());
            }

            block.Pop(); // removig '}'

            return block.Reverse();
        }
    }
}
