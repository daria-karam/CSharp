using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class BusBuilder : Builder
    {
        public BusBuilder()
        {
            transports = new List<TransportVehicle>();
        }

        public override void AddBelt()
        {
            //throw new NotImplementedException();
        }


        public override void AddChildSeat()
        {
            //throw new NotImplementedException();
        }

        public override List<TransportVehicle> GetResult()
        {
            return transports;
        }

        public override void PutAdult(Adult adult)
        {
            if (transports.Count() == 0 || transports.Last<TransportVehicle>().Passengers.Count() == transports.Last<TransportVehicle>().Capacity)
            {
                PutDriver();
            }
            adult.tripCost = 100;
            transports.Last<TransportVehicle>().Passengers.Add(adult);
        }

        public override void PutBenefitRecipient(BenefitRecipient benefitRecipient)
        {
            if (transports.Count() == 0 || transports.Last<TransportVehicle>().Passengers.Count() == transports.Last<TransportVehicle>().Capacity)
            {
                PutDriver();
            }
            benefitRecipient.tripCost = 40;
            transports.Last<TransportVehicle>().Passengers.Add(benefitRecipient);
        }

        public override void PutChild(Child child)
        {
            if (transports.Count() == 0 || transports.Last<TransportVehicle>().Passengers.Count() == transports.Last<TransportVehicle>().Capacity)
            {
                PutDriver();
            }
            child.tripCost = 50;
            transports.Last<TransportVehicle>().Passengers.Add(child);
        }
        
        public override void PutDriver()
        {
            Driver driver = new BusDriver() { FIO = "BusDriver #" + transports.Count() };
            Bus bus = new Bus() { CurrentDriver = driver };
            transports.Add(bus);
        }
    }
}
