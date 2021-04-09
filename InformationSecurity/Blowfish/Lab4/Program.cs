using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    class Program
    {
        static void GetKey()
        {
            Console.Write("Enter name of file with key: ");
            while (!Blowfish.SetKeyValue(Console.ReadLine()))
            {
                Console.Write("Wrong key or file not exists. Choose another file: ");
            }
        }

        static List<byte> ReadBytesFromFile(string filename)
        {
            string text = "";
            using (StreamReader file = new StreamReader(filename, true))
            {
                while (!file.EndOfStream)
                {
                    text = file.ReadToEnd();
                }
            }
            //Console.WriteLine(text);
            return Encoding.Default.GetBytes(text).ToList<byte>();
        }

        static void WriteBytesToFile(string filename, List<byte> bytes)
        {
            string text = Encoding.Default.GetString(bytes.ToArray());
            //Console.WriteLine(text);
            using (StreamWriter file = new StreamWriter(filename))
            {
                file.Write(text);
            }
        }

        static void Encryption()
        {
            List<byte> bytes = ReadBytesFromFile("text.txt");
            List<byte> newBytes = new List<byte>();

            while (bytes.Count % 8 != 0)
            {
                bytes.Add(0);
            }

            for (int i = 0; i < bytes.Count; i += 8)
            {
                byte[] tempBytes = { bytes[i + 0], bytes[i + 1], bytes[i + 2], bytes[i + 3],
                bytes[i + 4], bytes[i + 5], bytes[i + 6], bytes[i + 7]};
                UInt64 result = Blowfish.Function64Bit(BitConverter.ToUInt64(tempBytes, 0));
                newBytes.AddRange(BitConverter.GetBytes(result).ToList<byte>());
            }
            WriteBytesToFile("newtext.txt", newBytes);
        }

        static void Decryption()
        {
            List<byte> bytes = ReadBytesFromFile("newtext.txt");
            List<byte> newBytes = new List<byte>();

            while (bytes.Count % 8 != 0)
            {
                bytes.Add(0);
            }

            for (int i = 0; i < bytes.Count; i += 8)
            {
                byte[] tempBytes = { bytes[i + 0], bytes[i + 1], bytes[i + 2], bytes[i + 3],
                bytes[i + 4], bytes[i + 5], bytes[i + 6], bytes[i + 7]};
                UInt64 result = Blowfish.ReverseFunction64Bit(BitConverter.ToUInt64(tempBytes, 0));
                newBytes.AddRange(BitConverter.GetBytes(result).ToList<byte>());
            }

            WriteBytesToFile("newnewtext.txt", newBytes);
        }

        static void Run()
        {
            Blowfish.SetPIValuesFromFile();
            GetKey();
            //Blowfish.SetKeyValue("key.txt");
            Blowfish.RoundKeysXORKey();
            Blowfish.RoundKeysAndSubstitutionBoxEncryption();

            Encryption();
            Console.WriteLine("Encryption completed");
            Decryption();
            Console.WriteLine("Decryption completed");
        }

        static void Main(string[] args)
        {
            Run();
            KeyboardRun();
            Console.ReadKey();
        }

        static void KeyboardRun()
        {
            Blowfish.SetPIValuesFromFile();
            Blowfish.SetKeyValue("key.txt");
            Blowfish.RoundKeysXORKey();
            Blowfish.RoundKeysAndSubstitutionBoxEncryption();

            //String str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Console.Write("Write your string: ");
            String str = Console.ReadLine();

            List<byte> bytes = new List<byte>();
            bytes.AddRange(Encoding.UTF8.GetBytes(str).ToList<byte>());
            while (bytes.Count % 8 != 0)
            {
                bytes.Add(0);
            }

            List<byte> newBytes = new List<byte>();
            for (int i = 0; i < bytes.Count; i += 8)
            {
                byte[] tempBytes = { bytes[i + 0], bytes[i + 1], bytes[i + 2], bytes[i + 3],
                bytes[i + 4], bytes[i + 5], bytes[i + 6], bytes[i + 7]};
                UInt64 result = Blowfish.Function64Bit(BitConverter.ToUInt64(tempBytes, 0));
                newBytes.AddRange(BitConverter.GetBytes(result).ToList<byte>());
            }

            List<byte> newNewBytes = new List<byte>();


            string text = Encoding.Default.GetString(newBytes.ToArray());

            Console.WriteLine($"Encrypted string: {text}");

            newBytes = Encoding.Default.GetBytes(text).ToList<byte>();

            for (int i = 0; i < newBytes.Count; i += 8)
            {
                byte[] tempBytes = { newBytes[i + 0], newBytes[i + 1], newBytes[i + 2], newBytes[i + 3],
                newBytes[i + 4], newBytes[i + 5], newBytes[i + 6], newBytes[i + 7]};
                UInt64 result = Blowfish.ReverseFunction64Bit(BitConverter.ToUInt64(tempBytes, 0));
                newNewBytes.AddRange(BitConverter.GetBytes(result).ToList<byte>());
            }

            text = Encoding.UTF8.GetString(newNewBytes.ToArray());
            Console.WriteLine($"Decrypted string: {text}");
        }
    }
}
