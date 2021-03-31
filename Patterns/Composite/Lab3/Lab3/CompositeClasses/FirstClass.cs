using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class FirstClass : ComfortСlass, Component
    {
        public FirstClass()
        {
            passengers = new List<Passenger>();
            maxPassengersCount = 10;
            maxWeightOfFreeBaggage = 60;
        }

        public bool Add(Component component)
        {
            if (component.GetType() == typeof(Passenger) && maxPassengersCount > passengers.Count)
            {
                passengers.Add((Passenger)component);
                return true;
            }
            return false;
        }

        public int GetBaggageWeight()
        {
            int total = 0;
            foreach (Passenger passenger in passengers)
            {
                total += passenger.GetBaggageWeight();
            }
            return total;
        }

        public string GetInfo()
        {
            return "First class:: passengers: " + passengers.Count + " of " + maxPassengersCount +
                "; total baggage weight: " + GetBaggageWeight() + " kg";
        }

        public int GetPassengersCount()
        {
            int count = 0;
            foreach (Passenger passenger in passengers)
            {
                count += passenger.GetPassengersCount();
            }
            return count;
        }

        public int RemoveBaggageFromFlight(int needToRemove)
        {
            return 0;
        }
    }
}
