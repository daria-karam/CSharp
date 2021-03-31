using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    abstract class TransportVehicle
    {
        public int Capacity { get; set; }
        public List<Passenger> Passengers { get; set; }
        public Driver CurrentDriver { get; set; }
        public abstract void Ride();
    }
}
