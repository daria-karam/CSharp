using System.Collections.Generic;

namespace Lab5Core.Compressing.WithoutContext.Huffman
{
    class Symbol
    {
        public List<byte> Letters { get; set; }

        public long Count { get; set; }

        public Symbol(byte letter, long count)
        {
            Letters = new List<byte>() { letter };
            Count = count;
        }

        public Symbol(Symbol first, Symbol second)
        {
            Letters = new List<byte>();
            Letters.AddRange(first.Letters);
            Letters.AddRange(second.Letters);

            Count = first.Count + second.Count;
        }
    }
}
