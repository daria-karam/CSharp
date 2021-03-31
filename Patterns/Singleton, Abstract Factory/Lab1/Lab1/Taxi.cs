using System;
using System.Collections.Generic;

namespace Lab1
{
    class Taxi : TransportVehicle
    {
        public Taxi()
        {
            Capacity = 4;
            Passengers = new List<Passenger>();
            CurrentDriver = null;
        }

        public override void Ride()
        {
            if (CurrentDriver != null && Passengers.Count != 0)
            {
                Console.WriteLine("Taxi::   driver: " + CurrentDriver.FIO + "; passengers: " + Passengers.Count);
            }
        }
    }
}
