using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class EconomyClass : ComfortСlass, Component
    {
        public EconomyClass()
        {
            passengers = new List<Passenger>();
            maxPassengersCount = 150;
            maxWeightOfFreeBaggage = 20;
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
            return "Economy class:: passengers: " + passengers.Count + " of " + maxPassengersCount +
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
            int total = 0;
            int maxWeight = -1;
            while (needToRemove > 0 && maxWeight != 0)
            {
                Passenger p = passengers.First<Passenger>();
                foreach (Passenger passenger in passengers)
                {
                    if (p.GetBaggageWeight() < passenger.GetBaggageWeight())
                    {
                        p = passenger;
                    }
                }
                maxWeight = p.RemoveBaggageFromFlight(needToRemove);
                total += maxWeight;
                needToRemove -= maxWeight;
            }
            return total;
        }
    }
}
