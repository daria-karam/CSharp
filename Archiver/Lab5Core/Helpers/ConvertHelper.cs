using Lab5Core.Compressing.WithContext.LZ78;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Lab5Core.Helpers
{
    static class ConvertHelper
    {
        #region bytes <-> bools

        public static List<bool> ConvertBytesToBools(List<byte> bytes)
        {
            List<bool> bools = new List<bool>();

            foreach (var value in bytes)
            {
                bools.AddRange(ConvertByteToBools(value));
            }

            return bools;
        }

        public static List<bool> ConvertByteToBools(byte value)
        {
            List<bool> bools = new List<bool>
            {
                (value & 128) != 0,
                (value & 64) != 0,
                (value & 32) != 0,
                (value & 16) != 0,
                (value & 8) != 0,
                (value & 4) != 0,
                (value & 2) != 0,
                (value & 1) != 0
            };

            return bools;
        }

        public static List<byte> ConvertBoolsToBytes(List<bool> bools)
        {
            List<bool> newBools = new List<bool>();
            List<byte> bytes = new List<byte>();

            foreach (var b in bools)
            {
                newBools.Add(b);
                if (newBools.Count == 8)
                {
                    bytes.Add(ConvertBoolsToByte(newBools));
                    newBools.Clear();
                }
            }

            return bytes;
        }

        public static byte ConvertBoolsToByte(List<bool> bools)
        {
            byte b = 0;

            foreach (var item in bools)
            {
                b <<= 1;
                b += (item ? (byte)1 : (byte)0);
            }

            return b;
        }

        #endregion

        #region string <-> bools

        public static List<bool> GetBoolsByString(string line, int sizeInBytes = 0, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var bytes = encoding.GetBytes(line).ToList();

            if (sizeInBytes != 0)
            {
                bytes.InsertRange(0, new byte[sizeInBytes - bytes.Count]);
            }

            return ConvertHelper.ConvertBytesToBools(bytes);
        }

        public static string GetStringByBools(List<bool> bools, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;

            return encoding.GetString(ConvertBoolsToBytes(bools).ToArray());
        }

        #endregion

        #region long | int | ushort <-> bools

        public static List<bool> GetBoolsByLong(long number, int sizeInBytes = 0)
        {
            var bytes = BitConverter.GetBytes(number).ToList();
            if (sizeInBytes != 0)
            {
                bytes.InsertRange(0, new byte[sizeInBytes - bytes.Count()]);
            }

            return ConvertHelper.ConvertBytesToBools(bytes);
        }

        public static int GetIntByBytes(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes);
        }

        public static ushort GetUshortByBytes(byte[] bytes)
        {
            return BitConverter.ToUInt16(bytes);
        }
        #endregion

        #region object <-> bools

        public static T GetObjectByBools<T>(List<bool> bools)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using MemoryStream stream = new MemoryStream(ConvertBoolsToBytes(bools).ToArray());

            return (T)formatter.Deserialize(stream);
        }

        public static List<bool> GetBoolsByObject<T>(T obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using MemoryStream stream = new MemoryStream();

            formatter.Serialize(stream, obj);

            return ConvertBytesToBools(stream.ToArray().ToList());
        }

        #endregion

        #region List<LZ78Node> <-> bools

        public static List<LZ78Node> GetLZ78NodeListByBools(List<bool> bools)
        {
            List<LZ78Node> nodes = new List<LZ78Node>();

            var bytes = ConvertBoolsToBytes(bools);

            for (int i = 0; i < bytes.Count; i += 6)
            {
                var node = new LZ78Node()
                {
                    Pos = GetIntByBytes(new byte[] { bytes[i], bytes[i + 1], bytes[i + 2], bytes[i + 3] }),
                    Next = GetUshortByBytes(new byte[] { bytes[i + 4], bytes[i + 5] })
                };

                nodes.Add(node);
            }

            return nodes;
        }

        public static List<bool> GetBoolsByLZ78NodeList(List<LZ78Node> nodes)
        {
            List<byte> bytes = new List<byte>();

            foreach (var node in nodes)
            {
                bytes.AddRange(node.Pos.ToBytes());
                bytes.AddRange(node.Next.ToBytes());
            }

            return ConvertBytesToBools(bytes);
        }

        #endregion
    }
}
