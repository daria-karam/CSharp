using Lab5Core.Archivation;
using System;
using System.IO;

namespace Lab5Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Archiver archiver = new Archiver();

            string choice, path;
            while (true)
            {
                Console.WriteLine("\nWhat do you want to do?");
                Console.WriteLine("\t1 - pack");
                Console.WriteLine("\t2 - unpack");
                Console.WriteLine("\t3 - exit");
                choice = Console.ReadLine();

                if (choice == "3")
                {
                    break;
                }

                while (true)
                {
                    Console.Write("Enter filename or path: ");
                    path = Console.ReadLine();
                    if (FileOrDirectoreExists(path))
                    {
                        break;
                    }
                }

                switch (choice)
                {
                    case "1":
                        archiver.Pack(path);
                        break;
                    case "2":
                        archiver.Unpack(path);
                        break;
                }
            }
        }

        private static bool FileOrDirectoreExists(string path)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(path);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
