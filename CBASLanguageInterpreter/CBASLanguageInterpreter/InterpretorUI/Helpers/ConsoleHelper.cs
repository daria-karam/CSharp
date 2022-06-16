using System;
using System.Runtime.InteropServices;

namespace InterpretorUI.Helpers
{
    public static class ConsoleHelper
    {
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        public static void WriteToConsole(string line)
        {
            Console.WriteLine(line);
        }
    }
}
