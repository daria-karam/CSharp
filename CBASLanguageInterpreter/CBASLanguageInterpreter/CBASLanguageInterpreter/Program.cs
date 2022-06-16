using CBASLanguageInterpreter.Helpers;
using CBASLanguageInterpreter.Interpretation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CBASLanguageInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            SyntaxAnalyzer SA = new SyntaxAnalyzer();

            //SA.Analyse(out Stack<int> output, "abc   \n   = 4 + 3 * 4 ");
            //SA.Analyse(out Stack<int> output, @"abc = 4 + 3 * 4 if ( a == b ) { print ""c"" ; x = 45  } else { scan b ; y = 999  } ");
            //SA.Analyse(out Stack<int> output, @"abc = 4 + 3* 4     ; if ( a== b ) { 
            //print ""c"" ; x = 45;} else { scan b ; y = 999;     }");
            //SA.Analyse(out Stack<int> output, @"scan abc; x=99; print 4,abc,x,""hahaha"";");
            //SA.Analyse(out Stack<int> output, @"a=20; x=(99)*a*3/(2+1)+10-8; print x;");
            //SA.Analyse(out Stack<int> output, @"a=20; x=4/(2+2); print x;");
            //SA.Analyse(out Stack<int> output, @"for(x=1 to 4){ for(y=8 to 10){ print x,y; } }");
            //SA.Analyse(out Stack<int> output, @"scan z; for(x=1 to 4){ for(y=8 to 10){ print x,y; } }");
            //SA.Analyse(out Stack<int> output, @"x=1; if(x<4-9){print ""true"";}else{print ""false"";}");

            var (information, isCorrect) = SA.Analyse(out Stack<int> output, @"x=1; if(x<4-9){print ""true"";}else{print ""false"";}");

            Console.WriteLine(information);

            if (isCorrect)
            {
                var stack = output.Reverse();

                //PrintHelper.PrintIntStack(stack);

                Console.WriteLine("---------------------------------------------------");

                Interpreter.Interpret(stack, true);
            }

            Console.ReadKey();
        }
    }
}
