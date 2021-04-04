using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    class Car : ITransport
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Mark { get; set; }
        public Driver CurrentDriver { get; set; }

        public void Ride()
        {
            lock (Program.threadLock)
            {
                Console.WriteLine($"Car:: number: {Number} + mark: {Mark} with driver {CurrentDriver.FIO}");
            }
        }
    }
}
