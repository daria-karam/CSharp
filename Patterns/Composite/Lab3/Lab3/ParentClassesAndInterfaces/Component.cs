using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    interface Component
    {
        bool Add(Component component);

        int GetBaggageWeight();

        int RemoveBaggageFromFlight(int needToRemove);

        int GetPassengersCount();

        string GetInfo();
    }
}
