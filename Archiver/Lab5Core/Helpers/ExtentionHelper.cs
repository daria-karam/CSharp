using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab5Core.Helpers
{
    static class ExtentionHelper
    {
        public static string ToMyString(this List<bool> bools)
        {
            string line = "";
            foreach (var b in bools)
            {
                line += (b ? 1 : 0);
            }
            return line;
        }

        public static List<bool> LongTake(this List<bool> bools, long count)
        {
            if (count > int.MaxValue)
            {
                List<bool> taked = new List<bool>();

                foreach (var b in bools)
                {
                    if (count == 0)
                    {
                        break;
                    }

                    taked.Add(b);
                    count--;
                }

                return taked;
            }
            else
            {
                return bools.Take((int)count).ToList();
            }
        }

        public static List<bool> RemoveLongRange(this List<bool> bools, long index, long count)
        {
            if (count > int.MaxValue)
            {
                List<bool> taked = bools.LongTake(index);

                long needToSkip = index + count;
                foreach (var b in bools)
                {
                    if (needToSkip <= 0)
                    {
                        taked.Add(b);
                    }
                    else
                    {
                        needToSkip--;
                    }
                }

                return taked;
            }
            else
            {
                bools.RemoveRange((int)index, (int)count);
                return bools;
            }
        }

        public static byte[] ToBytes(this ushort symbol) => BitConverter.GetBytes(symbol);

        public static byte[] ToBytes(this int symbol) => BitConverter.GetBytes(symbol);
    }
}
