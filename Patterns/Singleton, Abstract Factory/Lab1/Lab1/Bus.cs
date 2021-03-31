using System;
using System.Collections.Generic;

namespace Lab1
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
