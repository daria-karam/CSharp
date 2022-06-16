using System;

namespace Lab5Core.Compressing.WithContext.LZ78
{
    [Serializable]
    public class LZ78Node
    {
        public int Pos { get; set; }
        public ushort Next { get; set; }

        public override string ToString()
        {
            return $"<{Pos}, {Next}>";
        }
    }
}
