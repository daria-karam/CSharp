using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class BusDriver : Driver
    {
        public override void Call()
        {
            Console.WriteLine("Bus driver: " + FIO);
        }
    }
}
