using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class IntersityBusBuilder : Builder
    {
        public IntersityBusBuilder()
        {
            transports = new List<TransportVehicle>();
        }

        public override void AddBelt()
        {
            ((IntersityBus)transports.Last<TransportVehicle>()).CountOfBelt++;
        }
        

        public override void AddChildSeat()
        {
            ((IntersityBus)transports.Last<TransportVehicle>()).CountOfChildPillow++;
        }

        public override List<TransportVehicle> GetResult()
        {
            return transports;
        }


        public override void PutAdult(Adult adult)
        {
            Random rnd = new Random();
            adult.tripCost = rnd.Next(1, 3) * 300;
            if (transports.Count() == 0 || transports.Last<TransportVehicle>().Passengers.Count() == transports.Last<TransportVehicle>().Capacity)
            {
                PutDriver();
            }
            transports.Last<TransportVehicle>().Passengers.Add(adult);
        }

        public override void PutBenefitRecipient(BenefitRecipient benefitRecipient)
        {
            Random rnd = new Random();
            benefitRecipient.tripCost = rnd.Next(1, 3) * 100;
            if (transports.Count() == 0 || transports.Last<TransportVehicle>().Passengers.Count() == transports.Last<TransportVehicle>().Capacity)
            {
                PutDriver();
            }
            transports.Last<TransportVehicle>().Passengers.Add(benefitRecipient);
        }

        public override void PutChild(Child child)
        {
            Random rnd = new Random();
            child.tripCost = rnd.Next(1, 3) * 200;
            if (transports.Count() == 0 || transports.Last<TransportVehicle>().Passengers.Count() == transports.Last<TransportVehicle>().Capacity)
            {
                PutDriver();
            }
            transports.Last<TransportVehicle>().Passengers.Add(child);
        }

        public override void PutDriver()
        {
            Driver driver = new BusDriver() { FIO = "BusDriver #" + transports.Count() * 2 };
            Driver driver2 = new BusDriver() { FIO = "BusDriver #" + (transports.Count() * 2 + 1) };
            IntersityBus bus = new IntersityBus() { CurrentDriver = driver, SecondDriver = driver2 };
            transports.Add(bus);
        }
    }
}
