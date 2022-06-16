using Lab5Core.Compressing;
using Lab5Core.Compressing.WithContext.LZ78;
using Lab5Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lab5Core.Archivation
{
    partial class Archiver
    {
        private readonly ICompressor compressor = new LZ78Compressor();

        private Dictionary<string, (int, int)> _headerStructure = new Dictionary<string, (int, int)>() //target: (offset, size)
        {
            { "Signature", (0 * _byteSize, 8 * _byteSize) },
            { "Version", (8 * _byteSize, 8 * _byteSize) },
            { "Encryption", (16 * _byteSize, 1 * _byteSize) },
            { "CompressionWithoutContext", (17 * _byteSize, 1 * _byteSize) },
            { "CompressionWithContext", (18 * _byteSize, 1 * _byteSize) },
            { "NoiseProtection", (19 * _byteSize, 1 * _byteSize) },
            { "ArchiveSize", (24 * _byteSize, 8 * _byteSize) },
            { "CountOfFiles", (32 * _byteSize, 8 * _byteSize) },
            { "SizeOfCompressionTable", (40 * _byteSize, 8 * _byteSize) },
        };

        private Dictionary<string, (int, int)> _subheaderStructure = new Dictionary<string, (int, int)>()
        {
            { "OffsetOfFilename", (0 * _byteSize, 8 * _byteSize) },
            { "OffsetOfFileBegin", (8 * _byteSize, 8 * _byteSize) },
        };

        private static readonly int _byteSize = 8;
        private static int _headerSize = 48 * _byteSize;
        private static int _subheaderSize = 16 * _byteSize;

        private long _archiveSize { get; set; } = 0;

        public List<bool> Header { get; set; }
        public List<bool> Subheader { get; set; }
        public List<bool> CompressionTable { get; set; }

        public List<bool> ArchiveEntry;

        public List<FileInfo> Files { get; set; }
        private DirectoryInfo _root { get; set; }

        public void Pack(string root)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(root);

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    _root = new DirectoryInfo(root);
                    Files = DirectoryWalker.WalkDirectoryTree(_root);
                }
                else
                {
                    if (IsArchive(root))
                    {
                        throw new Exception("It is ADM archive");
                    }
                    else
                    {
                        _root = new DirectoryInfo("archive");
                        Files = new List<FileInfo>() { new FileInfo(root) };
                    }
                }

                Console.WriteLine("\nProcess started...");
                _archiveSize = GetFilesSize() * 8;

                Console.WriteLine("Creating header...");
                CreateHeader();

                Console.WriteLine("Reading files...");
                FillArchiveEntry();

                Console.WriteLine("Compression started...");
                var table = compressor.Compress(ref ArchiveEntry);

                Console.WriteLine("Creating compression table...");
                CreateCompressionTable(table);

                Console.WriteLine("Creating subheader...");
                CreateSubheader();

                //Console.WriteLine("Encoding...");
                //Encrypter.Encode(ref ArchiveEntry);

                Console.WriteLine("Saving...");
                SaveArchiveToFile();

                Console.WriteLine("Done.");

                PrintHeader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Unpack(string root)
        {
            try
            {
                if (!IsArchive(root))
                {
                    throw new Exception("Not ADM archive");
                }

                Console.WriteLine("\nProcess started...");
                Console.WriteLine("Reading files...");
                byte[] bytes = File.ReadAllBytes(root);

                ArchiveEntry = ConvertHelper.ConvertBytesToBools(bytes.ToList());

                Console.WriteLine("Creating header...");
                Header = new List<bool>();
                Header.AddRange(ArchiveEntry.Take(_headerSize));
                ArchiveEntry.RemoveRange(0, _headerSize);
                _archiveSize = GetArchiveSize();

                Console.WriteLine("Creating subheader...");
                Subheader = new List<bool>();
                Subheader.AddRange(ArchiveEntry.Take(_subheaderSize * GetCountOfFiles()));
                ArchiveEntry.RemoveRange(0, Subheader.Count);

                Console.WriteLine("Creating compression table...");
                CompressionTable = new List<bool>();
                CompressionTable.AddRange(ArchiveEntry.LongTake(GetSizeOfCompressionTable()));
                ArchiveEntry.RemoveRange(0, CompressionTable.Count);
                Dictionary<byte, long> table = GetDictionaryByCompressionTable();

                //Console.WriteLine("Decoding...");
                //Encrypter.Decode(ref ArchiveEntry);

                Console.WriteLine("Decompression started...");
                compressor.Decompress(table, ref ArchiveEntry);

                ArchiveEntry = ArchiveEntry.LongTake(_archiveSize).ToList();

                int countOfFiles = GetCountOfFiles();
                var sizes = GetSizes();
                string filename;

                Console.WriteLine("Saving...");
                for (int i = 0; i < countOfFiles; i++)
                {
                    bytes = ConvertHelper.ConvertBoolsToBytes(ArchiveEntry.LongTake(sizes[i].Item1).ToList()).ToArray();
                    filename = Encoding.UTF8.GetString(bytes);
                    ArchiveEntry = ArchiveEntry.RemoveLongRange(0, sizes[i].Item1);

                    bytes = ConvertHelper.ConvertBoolsToBytes(ArchiveEntry.LongTake(sizes[i].Item2).ToList()).ToArray();
                    ArchiveEntry = ArchiveEntry.RemoveLongRange(0, sizes[i].Item2);

                    (new FileInfo(filename)).Directory.Create();
                    File.WriteAllBytes(filename, bytes);
                }

                Console.WriteLine("Done.");

                PrintHeader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool IsArchive(string root)
        {
            byte[] bytes;
            using (BinaryReader reader = new BinaryReader(File.OpenRead(root)))
            {
                bytes = reader.ReadBytes(8);
            }
            return Encoding.UTF8.GetString(bytes).Replace("\0", string.Empty) == "ADM";
        }

        private Dictionary<int, (long, long)> GetSizes()
        {
            Dictionary<int, (long, long)> dict = new Dictionary<int, (long, long)>();
            long offsetOfFilename, offsetOfFileBegin, offsetOfNextFilename;
            int countOfFiles = GetCountOfFiles();

            for (int i = 0; i < countOfFiles; i++)
            {
                offsetOfFilename = GetOffsetOfFilename(i);
                offsetOfFileBegin = GetOffsetOfFileBegin(i);
                if (i != countOfFiles - 1)
                {
                    offsetOfNextFilename = GetOffsetOfFilename(i + 1);
                    dict.Add(i, (offsetOfFileBegin - offsetOfFilename, offsetOfNextFilename - offsetOfFileBegin));
                }
                else
                {
                    dict.Add(i,
                        (offsetOfFileBegin - offsetOfFilename,
                        _headerSize + _subheaderSize * countOfFiles + CompressionTable.Count + ArchiveEntry.Count - offsetOfFileBegin));
                }
            }

            return dict;
        }

        private void CreateHeader()
        {
            Header = new List<bool>();
            Header.AddRange(ConvertHelper.GetBoolsByString("ADM", 8)); // { "Signature", (0, 8) },
            Header.AddRange(ConvertHelper.GetBoolsByString("1.0", 8)); // { "Version", (8, 8) },
            Header.AddRange(ConvertHelper.GetBoolsByString("1", 1));   // { "Encryption", (16 * _byteSize, 1 * _byteSize) },
            Header.AddRange(ConvertHelper.GetBoolsByString("1", 1));   // { "CompressionWithoutContext", (16, 1) },
            Header.AddRange(ConvertHelper.GetBoolsByString("0", 1));   // { "CompressionWithContext", (17, 1) },
            Header.AddRange(ConvertHelper.GetBoolsByString("0", 1));   // { "NoiseProtection", (18, 1) },
            Header.AddRange(ConvertHelper.GetBoolsByString("0", 4));   // reserved (19, 5)
            Header.AddRange(ConvertHelper.GetBoolsByLong(_archiveSize, 8)); // { "ArchiveSize", (24, 8) },
            Header.AddRange(ConvertHelper.GetBoolsByString(Files.Count.ToString(), 8)); //{ "CountOfFiles", (32, 8) },
        }

        private void CreateSubheader()
        {
            Subheader = new List<bool>();

            long currentOffset = CompressionTable.Count + _headerSize + _subheaderSize * Files.Count;
            foreach (var file in Files)
            {
                string filename = file.FullName.Remove(0, _root.FullName.Length - _root.Name.Length);

                Subheader.AddRange(ConvertHelper.GetBoolsByLong(currentOffset, 8));
                currentOffset += filename.Length * _byteSize;

                Subheader.AddRange(ConvertHelper.GetBoolsByLong(currentOffset, 8));
                currentOffset += file.Length * _byteSize;
            }
        }

        private void CreateCompressionTable(Dictionary<byte, long> table)
        {
            List<byte> bytes = new List<byte>();
            foreach (var item in table)
            {
                bytes.Add(item.Key);
                bytes.AddRange(BitConverter.GetBytes(item.Value));
            }

            CompressionTable = ConvertHelper.ConvertBytesToBools(bytes);

            Header.AddRange(ConvertHelper.GetBoolsByString(CompressionTable.Count.ToString(), 8)); // { "SizeOfCompressionTable", (40 * _byteSize, 8 * _byteSize) },
        }

        private Dictionary<byte, long> GetDictionaryByCompressionTable()
        {
            Dictionary<byte, long> table = new Dictionary<byte, long>();

            List<byte> bytes = ConvertHelper.ConvertBoolsToBytes(CompressionTable);

            byte letter;
            for (int i = 0; i < bytes.Count; i += 9)
            {
                letter = bytes.ElementAt(i);
                byte[] number = bytes.Skip(i+1).Take(8).ToArray();
                table.Add(letter, BitConverter.ToInt64(number));
            }

            return table;
        }

        private void FillArchiveEntry()
        {
            ArchiveEntry ??= new List<bool>();

            foreach (var file in Files)
            {
                ArchiveEntry.AddRange(ConvertHelper.GetBoolsByString(file.FullName
                    .Remove(0, _root.FullName.Length - _root.Name.Length), 0));

                var bytes = File.ReadAllBytes(file.FullName);

                ArchiveEntry.AddRange(ConvertHelper.ConvertBytesToBools(bytes.ToList()));
            }
        }

        public void PrintHeader()
        {
            Console.WriteLine("\n----------------------------------------------------------------------------------");
            Console.WriteLine($"Signature: {GetSignature()}");
            Console.WriteLine($"Version: {GetVersion()}");
            Console.WriteLine($"CompressionWithoutContext: {GetCompressionWithoutContext()}");
            Console.WriteLine($"CompressionWithContext: {GetCompressionWithContext()}");
            Console.WriteLine($"NoiseProtection: {GetNoiseProtection()}");
            Console.WriteLine($"ArchiveSize: {GetArchiveSize()}");
            Console.WriteLine($"CountOfFiles: {GetCountOfFiles()}");
            Console.WriteLine($"SizeOfCompressionTable: {GetSizeOfCompressionTable()}");
            Console.WriteLine($"Encryption: {GetEncryption()}");

            Console.WriteLine("-----------------------------------------");

            for (int i = 0; i < GetCountOfFiles(); i++)
            {
                Console.WriteLine($"\tOffsetOfFilename: {GetOffsetOfFilename(i)}");
                Console.WriteLine($"\tOffsetOfFileBegin: {GetOffsetOfFileBegin(i)}");
                Console.WriteLine("-----------------------------------------");
            }

            Console.WriteLine("----------------------------------------------------------------------------------\n");
        }

        public void SaveArchiveToFile()
        {
            var archive = new List<bool>();
            archive.AddRange(Header);
            archive.AddRange(Subheader);
            archive.AddRange(CompressionTable);
            archive.AddRange(ArchiveEntry);

            byte[] bytes = ConvertHelper.ConvertBoolsToBytes(archive).ToArray();

            File.WriteAllBytes("archive.adm", bytes);
        }
    }

    partial class Archiver
    {
        private long GetFilesSize() => Files
                .Select(file => (file.Length + file.FullName.Length - (_root.FullName.Length - _root.Name.Length)))
                .Sum();

        private string GetStringByTargetFromHeader(string target)
        {
            IEnumerable<bool> array = Header
                .Skip(_headerStructure[target].Item1)
                .Take(_headerStructure[target].Item2);

            byte[] bytes = ConvertHelper.ConvertBoolsToBytes(array.ToList()).ToArray();

            string line;
            if (target == "ArchiveSize")
            {
                line = BitConverter.ToInt64(bytes).ToString();
            }
            else
            {
                line = Encoding.UTF8.GetString(bytes);
            }

            return line;
        }

        private string GetStringByTargetFromSubheader(int fileNumber, string target)
        {
            IEnumerable<bool> array = Subheader
                .Skip(_subheaderStructure[target].Item1 + fileNumber * _subheaderSize)
                .Take(_subheaderStructure[target].Item2);

            byte[] bytes = ConvertHelper.ConvertBoolsToBytes(array.ToList()).ToArray();

            string line = BitConverter.ToInt64(bytes).ToString();

            return line;
        }
    }

    partial class Archiver
    {
        public string GetSignature() =>
            GetStringByTargetFromHeader("Signature");
        public string GetVersion() =>
            GetStringByTargetFromHeader("Version");
        public string GetEncryption() =>
            GetStringByTargetFromHeader("Encryption");
        public string GetCompressionWithoutContext() =>
            GetStringByTargetFromHeader("CompressionWithoutContext");
        public string GetCompressionWithContext() =>
            GetStringByTargetFromHeader("CompressionWithContext");
        public string GetNoiseProtection() =>
            GetStringByTargetFromHeader("NoiseProtection");
        public long GetArchiveSize() =>
            long.Parse(GetStringByTargetFromHeader("ArchiveSize").Replace("\0", string.Empty));
        public int GetCountOfFiles() =>
            int.Parse(GetStringByTargetFromHeader("CountOfFiles").Replace("\0", string.Empty));
        public long GetSizeOfCompressionTable() =>
            long.Parse(GetStringByTargetFromHeader("SizeOfCompressionTable").Replace("\0", string.Empty));
        public long GetOffsetOfFilename(int fileNumber) =>
            long.Parse(GetStringByTargetFromSubheader(fileNumber, "OffsetOfFilename").Replace("\0", string.Empty));
        public long GetOffsetOfFileBegin(int fileNumber) =>
            long.Parse(GetStringByTargetFromSubheader(fileNumber, "OffsetOfFileBegin").Replace("\0", string.Empty));
    }
}
