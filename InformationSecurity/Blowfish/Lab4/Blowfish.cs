using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    static class Blowfish
    {
        private static uint[] RoundKeys { get; set; }
        private static uint[,] SubstitutionBox { get; set; }
        private static string SecretKey { get; set; }

        public static void SetPIValuesFromFile()
        {
            string line, hexWord;
            int counter = 0, column = 0;
            RoundKeys = new uint[18];
            SubstitutionBox = new uint[4, 256];
            using (StreamReader file = new StreamReader("first_8366_Hex_Digits_of_PI.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    for (int i = 0; i < 64; i += 8)
                    {
                        if (line.Length >= i + 8)
                        {
                            hexWord = line.Substring(i, 8);
                            uint.TryParse(hexWord, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out uint number);
                            if (counter < 18)
                            {
                                RoundKeys[counter] = number;
                            }
                            else
                            {
                                if ((counter - 18) % 256 == 0 && counter != 18)
                                {
                                    column++;
                                }
                                SubstitutionBox[column, (counter - 18) % 256] = number;
                            }
                        }
                        counter++;
                    }
                }
            }
        }

        public static bool SetKeyValue(string filename)
        {
            if (File.Exists(filename))
            {
                SecretKey = "";
                string line, partOfLine;
                using (StreamReader file = new StreamReader(filename))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        while (line.Length != 0)
                        {
                            partOfLine = line.Substring(0, line.Length > 16 ? 16 : line.Length);
                            line = line.Substring(line.Length > 16 ? 16 : line.Length);
                            if (!ulong.TryParse(partOfLine, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out ulong number))
                            {
                                return false;
                            }
                            else
                            {
                                SecretKey += partOfLine;
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void RoundKeysXORKey()
        {
            string key = SecretKey;
            while (key.Length < RoundKeys.Length * 8)
            {
                key += key;
            }
            for (int i = 0; i < RoundKeys.Length; i++)
            {
                uint.TryParse(key.Substring(8 * i, 8), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out uint keyPart);
                RoundKeys[i] ^= keyPart;
            }
        }

        public static void RoundKeysAndSubstitutionBoxEncryption()
        {
            BetweenFunction(0, 0, out RoundKeys[0], out RoundKeys[1]);
            for (int i = 2; i < 18; i += 2)
            {
                BetweenFunction(RoundKeys[i - 2], RoundKeys[i - 1], out RoundKeys[i], out RoundKeys[i + 1]);
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 256; j += 2)
                {
                    if (!(j == 0 && i == 0))
                    {
                        if (j == 0)
                        {
                            BetweenFunction(SubstitutionBox[i - 1, 254], SubstitutionBox[i - 1, 255],
                                out SubstitutionBox[i, j], out SubstitutionBox[i, j + 1]);
                        }
                        else
                        {
                            BetweenFunction(SubstitutionBox[i, j - 2], SubstitutionBox[i, j - 1],
                                out SubstitutionBox[i, j], out SubstitutionBox[i, j + 1]);
                        }
                    }
                    else
                    {
                        BetweenFunction(RoundKeys[16], RoundKeys[17], out SubstitutionBox[0, 0], out SubstitutionBox[0, 1]);
                    }
                }
            }
        }

        public static void BetweenFunction(UInt32 left, UInt32 right, out UInt32 newLeft, out UInt32 newRight)
        {
            Union32bitTo64bit(out UInt64 union, in left, in right);
            UInt64 result = Function64Bit(union);
            Split64bitTo32bit(in result, out newLeft, out newRight);
        }

        public static UInt32 Function32Bit(UInt32 block)
        {
            byte[] bytes = BitConverter.GetBytes(block);
            return ((SubstitutionBox[0, bytes[0]] + SubstitutionBox[1, bytes[1]])
                ^ SubstitutionBox[2, bytes[2]]) + SubstitutionBox[3, bytes[3]];
        }

        public static UInt64 Function64Bit(UInt64 block)
        {
            Split64bitTo32bit(in block, out UInt32 L, out UInt32 R);

            for (int i = 0; i < 16; i++)
            {
                L ^= RoundKeys[i];
                R ^= Function32Bit(L);
                Swap(ref L, ref R);
            }

            Swap(ref L, ref R);
            L ^= RoundKeys[17];
            R ^= RoundKeys[16];

            Union32bitTo64bit(out UInt64 result, in L, in R);
            return result;
        }

        public static UInt64 ReverseFunction64Bit(in UInt64 block)
        {
            Split64bitTo32bit(in block, out UInt32 L, out UInt32 R);

            R ^= RoundKeys[16];
            L ^= RoundKeys[17];
            Swap(ref L, ref R);

            for (int i = 15; i >= 0; i--)
            {
                Swap(ref L, ref R);
                R ^= Function32Bit(L);
                L ^= RoundKeys[i];
            }

            Union32bitTo64bit(out UInt64 result, in L, in R);
            return result;
        }

        public static void Split64bitTo32bit(in UInt64 from, out UInt32 first, out UInt32 second)
        {
            byte[] bytes = BitConverter.GetBytes(from);
            first = BitConverter.ToUInt32(bytes, 0);
            second = BitConverter.ToUInt32(bytes, 4);
        }

        public static void Union32bitTo64bit(out UInt64 to, in UInt32 first, in UInt32 second)
        {
            byte[] bytes1 = BitConverter.GetBytes(first);
            byte[] bytes2 = BitConverter.GetBytes(second);
            byte[] bytes = { bytes1[0], bytes1[1], bytes1[2], bytes1[3],
                bytes2[0], bytes2[1], bytes2[2], bytes2[3] };
            to = BitConverter.ToUInt64(bytes, 0);
        }

        public static void Swap(ref UInt32 n, ref UInt32 m)
        {
            UInt32 temp = n;
            n = m;
            m = temp;
        }
    }
}
