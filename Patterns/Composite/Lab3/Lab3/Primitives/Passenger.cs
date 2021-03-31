using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Passenger : Component
    {
        public string FIO { get; set; }
        private int Baggage { get; set; }

        public Passenger(int baggage)
        {
            Baggage = baggage < 5 ? 5 : (baggage > 60 ? 60 : baggage);
        }

        public bool Add(Component component)
        {
            return false;
        }

        public int GetBaggageWeight()
        {
            return Baggage;
        }

        public string GetInfo()
        {
            return "Passenger:: FIO: " + FIO + "; baggage: " + Baggage + " kg";
        }

        public int GetPassengersCount()
        {
            return 1;
        }

        public int RemoveBaggageFromFlight(int needToRemove)
        {
            int baggage = Baggage;
            Baggage = 0;
            return baggage;
        }
    }
}
