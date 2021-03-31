using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Pilot : Component
    {
        public string FIO { get; set; }

        public bool Add(Component component)
        {
            return false;
        }

        public int GetBaggageWeight()
        {
            return 0;
        }

        public string GetInfo()
        {
            return "Pilot:: FIO: " + FIO;
        }

        public int GetPassengersCount()
        {
            return 0;
        }

        public int RemoveBaggageFromFlight(int needToRemove)
        {
            return 0;
        }
    }
}
