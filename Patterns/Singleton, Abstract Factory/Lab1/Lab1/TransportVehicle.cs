using System.Collections.Generic;

namespace Lab1
{
    abstract class TransportVehicle
    {
        public int Capacity { get; set; }
        public List<Passenger> Passengers { get; set; }
        public Driver CurrentDriver { get; set; }
        public abstract void Ride();
    }
}
