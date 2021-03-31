using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Airplane : Component
    {
        public int maxBaggageWeight;

        public int baggageRemoved;

        public List<Component> components;

        public Airplane(int maxBaggageWeight)
        {
            components = new List<Component>();
            baggageRemoved = 0;
            this.maxBaggageWeight = maxBaggageWeight > (10 + 20 + 150) * 60 ? (10 + 20 + 150) * 60 :
                (maxBaggageWeight < (10 + 20 + 150) * 5 ? (10 + 20 + 150) * 5 : maxBaggageWeight);
        }

        public bool Add(Component component)
        {
            bool flag = true;
            if (component.GetType() == typeof(Staff) && GetFirstClass() == null)
            {
                components.Add(component);
            }
            else if (component.GetType() == typeof(FirstClass) && GetFirstClass() == null)
            {
                components.Add(component);
            }
            else if (component.GetType() == typeof(BusinessClass) && GetBusinessClass() == null)
            {
                components.Add(component);
            }
            else if (component.GetType() == typeof(EconomyClass) && GetEconomyClass() == null)
            {
                components.Add(component);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public int GetBaggageWeight()
        {
            int total = 0;
            foreach (Component component in components)
            {
                total += component.GetBaggageWeight();
            }
            return total;
        }

        public string GetInfo()
        {
            string result = "Airplane:: ";
            foreach (Component component in components)
            {
                result += "\n\t" + component.GetInfo();
            }

            result += "\n\tTotal passengers count: " + GetPassengersCount();
            result += "\n\tTotal baggage weight: " + GetBaggageWeight() + " of " + maxBaggageWeight;
            result += "\n\tBaggage removed: " + baggageRemoved + " kg";
            result += "\n Ready for take-off: " + IsReadyForeTakeoff();

            return result;
        }

        public int RemoveBaggageFromFlight(int needToRemove)
        {
            foreach (Component component in components)
            {
                baggageRemoved += component.RemoveBaggageFromFlight(needToRemove);
            }
            return baggageRemoved;
        }

        public bool IsReadyForeTakeoff()
        {
            if (GetStaff().IsReady() && GetPassengersCount() > 0 && GetBaggageWeight() <= maxBaggageWeight)
            {
                return true;
            }
            return false;
        }

        public int GetPassengersCount()
        {
            int count = 0;
            foreach (Component component in components)
            {
                count += component.GetPassengersCount();
            }
            return count;
        }

        private Staff GetStaff()
        {
            foreach (Component component in components)
            {
                if (component.GetType() == typeof(Staff))
                {
                    return (Staff)component;
                }
            }
            return null;
        }

        private FirstClass GetFirstClass()
        {
            foreach (Component component in components)
            {
                if (component.GetType() == typeof(FirstClass))
                {
                    return (FirstClass)component;
                }
            }
            return null;
        }

        private BusinessClass GetBusinessClass()
        {
            foreach (Component component in components)
            {
                if (component.GetType() == typeof(BusinessClass))
                {
                    return (BusinessClass)component;
                }
            }
            return null;
        }

        private EconomyClass GetEconomyClass()
        {
            foreach (Component component in components)
            {
                if (component.GetType() == typeof(EconomyClass))
                {
                    return (EconomyClass)component;
                }
            }
            return null;
        }
    }
}
