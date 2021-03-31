using System.Collections.Generic;

namespace Lab1
{
    abstract class BoardAnyTransportVehicle
    {
        protected List<TransportVehicle> Transport { get; set; }
        public abstract void BoardDriver(Driver driver);
        public abstract List<TransportVehicle> CreateTransport(List<Passenger> passengers);
    }
}
