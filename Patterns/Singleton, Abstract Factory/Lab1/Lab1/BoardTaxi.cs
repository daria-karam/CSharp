using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1
{
    class BoardTaxi : BoardAnyTransportVehicle
    {
        public string TaxiFabricName { get; private set; }

        private static BoardTaxi instance;

        protected BoardTaxi(string TaxiFabricName)
        {
            this.TaxiFabricName = TaxiFabricName;
            Transport = new List<TransportVehicle>();
        }

        public static BoardTaxi getInstance(string TaxiFabricName)
        {
            if (instance == null)
                instance = new BoardTaxi(TaxiFabricName);
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

        public void WhatIsTheFabric()
        {
            Console.WriteLine("TaxiFabric::    " + TaxiFabricName);
        }

        public override void BoardDriver(Driver driver)
        {
            if (driver != null && driver.GetType() == typeof(TaxiDriver))
            {
                Taxi taxi = new Taxi();
                taxi.CurrentDriver = driver;
                Transport.Add(taxi);
            }
        }
    }
}
