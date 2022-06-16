using Lab5Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Lab5Core.Compressing.WithoutContext.Huffman
{
    class HuffmanCompressor : ICompressor
    {
        public Dictionary<byte, long> Compress(ref List<bool> bools)
        {
            List<byte> bytes = ConvertHelper.ConvertBoolsToBytes(bools);

            List<Symbol> symbols = bytes
                .GroupBy(_ => _)
                .Select(_ => new Symbol(_.Key, _.LongCount()))
                .OrderByDescending(_ => _.Count)
                .ThenByDescending(_ => _.Letters.First())
                .ToList();

            Dictionary<byte, long> frequences = symbols.ToDictionary(_ => _.Letters.First(), _ => _.Count);

            Dictionary<byte, List<bool>> dictionary = GetCompressionTable(symbols);

            bools = new List<bool>();

            foreach (var letter in bytes)
            {
                bools.AddRange(dictionary[letter]);
            }

            while (bools.Count % 8 != 0)
            {
                bools.Add(false);
            }

            return frequences;
        }

        public void Decompress(Dictionary<byte, long> frequences, ref List<bool> bools)
        {
            List<Symbol> symbols = frequences
                .AsParallel()
                .Select(_ => new Symbol(_.Key, _.Value))
                .OrderByDescending(_ => _.Count)
                .ThenByDescending(_ => _.Letters.First())
                .ToList();

            Dictionary<byte, List<bool>> dictionary = GetCompressionTable(symbols);

            Dictionary<string, byte> terribleDictionary = GetTerribleDictionary(dictionary);

            List<byte> bytes = new List<byte>();
            List<bool> current = new List<bool>();

            foreach (var b in bools)
            {
                current.Add(b);

                if (terribleDictionary.ContainsKey(current.ToMyString()))
                {
                    bytes.Add(terribleDictionary[current.ToMyString()]);
                    current.Clear();
                }
            }

            bools = ConvertHelper.ConvertBytesToBools(bytes);
        }

        private Dictionary<string, byte> GetTerribleDictionary(Dictionary<byte, List<bool>> dictionary)
        {
            Dictionary<string, byte> terribleDictionary = new Dictionary<string, byte>();

            foreach (var item in dictionary)
            {
                terribleDictionary.Add(item.Value.ToMyString(), item.Key);
            }

            return terribleDictionary;
        }

        private Dictionary<byte, List<bool>> GetCompressionTable(List<Symbol> symbols)
        {
            Dictionary<byte, List<bool>> dictionary = new Dictionary<byte, List<bool>>();

            dictionary = symbols.
                ToDictionary(_ => _.Letters.First(), _ => new List<bool>());

            Symbol first, second;
            while (symbols.Count > 1)
            {
                first = symbols.Last();
                symbols.Remove(first);

                second = symbols.Last();
                symbols.Remove(second);

                symbols.Add(new Symbol(first, second));

                foreach (var letter in first.Letters)
                {
                    dictionary[letter].Add(false);
                }

                foreach (var letter in second.Letters)
                {
                    dictionary[letter].Add(true);
                }

                symbols = symbols
                    .OrderByDescending(_ => _.Count)
                    .ThenByDescending(_ => _.Letters.First())
                    .ToList();
            }

            foreach (var item in dictionary)
            {
                //Console.Write($"\n{item.Key}:\t");
                item.Value.Reverse();
                //foreach (var b in item.Value)
                //{
                //    Console.Write(b ? 1 : 0);
                //}
            }

            return dictionary;
        }
    }
}
