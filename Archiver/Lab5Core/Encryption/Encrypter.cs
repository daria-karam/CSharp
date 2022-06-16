using System.Collections.Generic;
using System.Linq;

namespace Lab5Core.Encryption
{
    static class Encrypter
    {
        public static void Encode(ref List<bool> archiveEntry)
        {
            List<bool> bools = new List<bool>();

            long count = 0;
            foreach (var b in archiveEntry)
            {
                count++;
                bools.Add(b);
                if (count % 8 == 0)
                {
                    bools.Add(false);
                    bools.Add(false);
                }
            }

            while (bools.Count % 8 != 0)
            {
                bools.Add(false);
            }

            archiveEntry = bools;
        }

        public static void Decode(ref List<bool> archiveEntry)
        {
            List<bool> bools = new List<bool>();

            long count = 0;
            foreach (var b in archiveEntry)
            {
                if (count % 10 < 8)
                {
                    bools.Add(b);
                }
                count++;
            }

            while (bools.Count % 8 != 0)
            {
                bools.RemoveAt(bools.Count - 1);
            }

            archiveEntry = bools;
        }
    }
}
