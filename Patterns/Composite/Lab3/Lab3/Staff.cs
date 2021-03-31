using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Staff : Component
    {
        public List<Component> components;

        public Staff()
        {
            components = new List<Component>();
        }

        public bool Add(Component component)
        {
            bool flag = true;
            if (component.GetType() == typeof(Pilot) && GetPilotsCount() < 2)
            {
                components.Add(component);
            }
            else if (component.GetType() == typeof(Stewardess) && GetStewardessesCount() < 6)
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
            return 0;
        }

        public string GetInfo()
        {
            string result = "staff:: ";
            foreach (Component component in components)
            {
                result += "\n\t\t" + component.GetInfo();
            }
            return result;
        }

        public bool IsReady()
        {
            return GetPilotsCount() == 2 && GetStewardessesCount() == 6;
        }

        public int GetPassengersCount()
        {
            return 0;
        }

        public int RemoveBaggageFromFlight(int needToRemove)
        {
            return 0;
        }

        private int GetPilotsCount()
        {
            int count = 0;
            foreach (Component component in components)
            {
                if (component.GetType() == typeof(Pilot))
                {
                    count++;
                }
            }
            return count;
        }

        private int GetStewardessesCount()
        {
            int count = 0;
            foreach (Component component in components)
            {
                if (component.GetType() == typeof(Stewardess))
                {
                    count++;
                }
            }
            return count;
        }

    }
}
