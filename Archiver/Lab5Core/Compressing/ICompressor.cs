using System.Collections.Generic;

namespace Lab5Core.Compressing
{
    interface ICompressor
    {
        Dictionary<byte, long> Compress(ref List<bool> bools);
        void Decompress(Dictionary<byte, long> frequences, ref List<bool> bools);
    }
}
