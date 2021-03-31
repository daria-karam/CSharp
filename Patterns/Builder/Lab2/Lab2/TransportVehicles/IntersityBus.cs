using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class IntersityBus : TransportVehicle
    {
        public int CountOfChildPillow { get; set; }
        public int CountOfBelt { get; set; }
        public Driver SecondDriver { get; set; }
        public IntersityBus()
        {
            Capacity = 30;
            Passengers = new List<Passenger>();
            CurrentDriver = null;
            SecondDriver = null;
            CountOfChildPillow = 0;
            CountOfBelt = 0;
        }

        public override void Ride()
        {
            Console.WriteLine("IntersityBus::    first driver: " + CurrentDriver.FIO + "; second driver: " + SecondDriver.FIO +
                "; passengers: " + Passengers.Count + "; child pillows: " + CountOfChildPillow + "; belts: " + CountOfBelt);
        }
    }
}
