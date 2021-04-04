using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    class Driver
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public void SayHello()
        {
            lock(Program.threadLock)
            {
                Console.WriteLine($"Driver:: fio: {FIO}");
            }
        }
    }
}
