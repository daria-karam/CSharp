using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1
{
    class BoardBus : BoardAnyTransportVehicle
    {
        public string BusFabricName { get; private set; }

        private static BoardBus instance;

        protected BoardBus(string BusFabricName)
        {
            this.BusFabricName = BusFabricName;
            Transport = new List<TransportVehicle>();
        }

        public static BoardBus getInstance(string BusFabricName)
        {
            if (instance == null)
                instance = new BoardBus(BusFabricName);
            return instance;
        }

        public override List<TransportVehicle> CreateTransport(List<Passenger> passengers)
        {
            List<TransportVehicle> transports = new List<TransportVehicle>();
            if (passengers != null && passengers.Count() != 0)
            {
                while (passengers.Count() != 0 && Transport.Count() != 0)
                {
                    while (passengers.Count() != 0 && Transport.First<TransportVehicle>().Passengers.Count() != Transport.First<TransportVehicle>().Capacity)
                    {
                        Transport.First<TransportVehicle>().Passengers.Add(passengers.First<Passenger>());
                        passengers.RemoveRange(0, 1);
                    }
                    transports.Add(Transport.First<TransportVehicle>());
                    Transport.RemoveRange(0, 1);
                }
                return transports;
            }
            return null;
        }

        public override void BoardDriver(Driver driver)
        {
            if (driver != null && driver.GetType() == typeof(BusDriver))
            {
                Bus bus = new Bus();
                bus.CurrentDriver = driver;
                Transport.Add(bus);
            }
        }

        public void WhatIsTheFabric()
        {
            Console.WriteLine("BusFabric::     " + BusFabricName);
        }
    }
}
