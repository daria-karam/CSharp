using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Taxi : TransportVehicle
    {
        public int CountOfChildSafetySeat { get; set; }

        public Taxi()
        {
            Capacity = 4;
            Passengers = new List<Passenger>();
            CurrentDriver = null;
            CountOfChildSafetySeat = 0;
        }

        public override void Ride()
        {
            if (CurrentDriver != null && Passengers.Count != 0)
            {
                Console.WriteLine("Taxi::   driver: " + CurrentDriver.FIO + "; passengers: " + Passengers.Count
                    + "; child seats: " + CountOfChildSafetySeat);
            }
        }
    }
}
