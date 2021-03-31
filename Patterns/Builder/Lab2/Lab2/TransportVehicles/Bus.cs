using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Bus : TransportVehicle
    {
        public Bus()
        {
            Capacity = 30;
            Passengers = new List<Passenger>();
            CurrentDriver = null;
        }

        public override void Ride()
        {
            Console.WriteLine("Bus::    driver: " + CurrentDriver.FIO + "; passengers: " + Passengers.Count);
        }
    }
}
