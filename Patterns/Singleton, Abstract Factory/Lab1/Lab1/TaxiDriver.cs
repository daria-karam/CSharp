using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class TaxiDriver : Driver
    {
        public override void Call()
        {
            Console.WriteLine("Taxi driver: " + FIO);
        }
    }
}
