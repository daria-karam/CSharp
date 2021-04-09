using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{
    class Program
    {
        static char[] numericalEquivalents = {'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
            'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ',
            'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
        static Key PublicKey { get; set; }
        static Key PrivateKey { get; set; }

        static bool IsPrimeNumber(int number)
        {
            var result = true;
            if (number > 1)
            {
                for (var i = 2; i < number; i++)
                {
                    if (number % i == 0)
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        static int GetNOD(int first, int second)
        {
            while ((first != 0) && (second != 0))
            {
                if (first > second)
                    first -= second;
                else
                    second -= first;
            }
            return Math.Max(first, second);
        }

        static bool GenerateKeys(int p, int q)
        {
            if (IsPrimeNumber(p) && IsPrimeNumber(q))
            {
                int n = p * q;
                int phi = (p - 1) * (q - 1);

                int e;
                Console.Write("Enter e (between 1 and " + (n - 1) + "): ");
                e = Convert.ToInt32(Console.ReadLine());
                while (e < 1 || e > n || GetNOD(e, n) != 1)
                {
                    Console.Write("Try again: ");
                    e = Convert.ToInt32(Console.ReadLine());
                }

                int k = 0;
                int tmp = k * phi + 1;
                while (tmp % e != 0)
                {
                    k++;
                    tmp = k * phi + 1;
                }

                int d = tmp / e;
                PublicKey = new Key(e, n);
                PrivateKey = new Key(d, n);
                return true;
            }
            return false;
        }

        static int GetNumericalEquivalentOfSymbol(char symbol)
        {
            for (int i = 0; i < numericalEquivalents.Count(); i++)
            {
                if (numericalEquivalents[i] == symbol)
                {
                    return i + 1;
                }
            }
            return -1;
        }

        static int GetPowAndMode(int degreeBase, int degree, int mode)
        {
            int result = 1;
            while (degree != 0)
            {
                result = result * degreeBase % mode;
                degree--;
            }
            return result;
        }

        static int GetSymbolEncoding(char symbol)
        {
            return GetPowAndMode(GetNumericalEquivalentOfSymbol(symbol), PublicKey.First, PublicKey.Second);
        }

        static char GetSymbolDecoding(int code)
        {
            int newNumericalEquivalent = GetPowAndMode(code, PrivateKey.First, PrivateKey.Second);
            return numericalEquivalents[newNumericalEquivalent - 1];
        }

        static string Encryption(string text)
        {
            string newText = "";
            for (int i = 0; i < text.Length; i ++)
            {
                newText += GetSymbolEncoding(text[i]);
                if (i != text.Length - 1)
                {
                    newText += " ";
                }
            }
            return newText;
        }

        static string Decryption(string text)
        {
            string[] codes = text.Split(' ');
            string newText = "";
            for (int i = 0; i < codes.Count(); i++)
            {
                newText += GetSymbolDecoding(Convert.ToInt32(codes[i]));
            }
            return newText;
        }

        static void Main(string[] args)
        {
            int p, q;
            Console.WriteLine("Enter p and q: ");
            p = Convert.ToInt32(Console.ReadLine());
            q = Convert.ToInt32(Console.ReadLine());
            while (p * q < 45 || !GenerateKeys(p, q))
            {
                Console.WriteLine("Not prime numbers. Try again: ");
                p = Convert.ToInt32(Console.ReadLine());
                q = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("\nEnter text to encode: ");
            string text = Console.ReadLine();
            string textEncoding = Encryption(text);
            Console.WriteLine("Text encoding: " + textEncoding);
            string textDecoding = Decryption(textEncoding);
            Console.WriteLine("Text decoding: " + textDecoding);
            Console.WriteLine("Are these two identical texts? " + text.Equals(textDecoding));
            Console.ReadKey();
        }
    }
}
