using Lab5Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab5Core.Compressing.WithoutContext.Arithmetic
{
    class ArithmeticCompressor : ICompressor
    {
        public Dictionary<byte, long> Compress(ref List<bool> bools)
        {
            List<byte> bytes = ConvertHelper.ConvertBoolsToBytes(bools);

            Dictionary<byte, long> frequences = bytes
                .GroupBy(_ => _)
                .OrderByDescending(_ => _.LongCount())
                .ThenByDescending(_ => _.Key)
                .ToDictionary(_ => _.Key, _ => _.LongCount());

            var intervals = GetFrequenceIntervals(frequences);

            //some logic here

            return frequences;
        }

        public void Decompress(Dictionary<byte, long> frequences, ref List<bool> bools)
        {
            throw new NotImplementedException();
        }

        //private bool EnsureNewBit((long, long) borders, )

        private (long, long) GetNewBorders((long, long) freq, (long, long) oldBorders, long D)
        {
            var left = oldBorders.Item1 + (freq.Item1 * (oldBorders.Item2 - oldBorders.Item1)) / D;
            var right = oldBorders.Item1 + (freq.Item2 * (oldBorders.Item2 - oldBorders.Item1)) / D;

            return (left, right);
        }

        private (long, long) ScaleBorders((long, long) borders, long N)
        {
            if (borders.Item1 > N / 2)
            {
                borders.Item1 -= N / 2;
                borders.Item2 -= N / 2;
            }
            else if (borders.Item1 > N / 4)
            {
                borders.Item1 -= N / 4;
                borders.Item2 -= N / 4;
            }

            borders.Item1 *= 2;
            borders.Item2 *= 2;

            return borders;
        }

        private Dictionary<byte, (long, long)> GetFrequenceIntervals(Dictionary<byte, long> frequences)
        {
            Dictionary<byte, (long, long)> intervals = new Dictionary<byte, (long, long)>();

            long total = 0;

            foreach (var frequence in frequences)
            {
                intervals.Add(frequence.Key, (total, frequence.Value));
                total += frequence.Value;
            }

            return intervals;
        }
    }
}
