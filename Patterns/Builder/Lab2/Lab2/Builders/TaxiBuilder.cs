using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class TaxiBuilder : Builder
    {
        public TaxiBuilder()
        {
            transports = new List<TransportVehicle>();
        }

        public override List<TransportVehicle> GetResult()
        {
            return transports;
        }
        
        public override void PutAdult(Adult adult)
        {
            Random rnd = new Random();
            adult.tripCost = rnd.Next(50, 1000);
            if (transports.Count() == 0 || transports.Last<TransportVehicle>().Passengers.Count() == transports.Last<TransportVehicle>().Capacity)
            {
                PutDriver();
            }
            transports.Last<TransportVehicle>().Passengers.Add(adult);
        }

        public override void PutBenefitRecipient(BenefitRecipient benefitRecipient)
        {
            Random rnd = new Random();
            benefitRecipient.tripCost = rnd.Next(50, 1000);
            if (transports.Count() == 0 || transports.Last<TransportVehicle>().Passengers.Count() == transports.Last<TransportVehicle>().Capacity)
            {
                PutDriver();
            }
            int value = rnd.Next(0, 1);
            if (value == 0)
            {
                PutAdult(new Adult() { FIO = benefitRecipient.FIO });
            }
            else
            {
                PutChild(new Child() { FIO = benefitRecipient.FIO });
                AddChildSeat();
            }
        }

        public override void PutChild(Child child)
        {
            Random rnd = new Random();
            child.tripCost = rnd.Next(50, 1000);
            if (transports.Count() == 0 || transports.Last<TransportVehicle>().Passengers.Count() == transports.Last<TransportVehicle>().Capacity)
            {
                PutDriver();
            }
            AddChildSeat();
            transports.Last<TransportVehicle>().Passengers.Add(child);
        }
        
        public override void PutDriver()
        {
            Driver driver = new TaxiDriver() { FIO = "TaxiDriver #" + transports.Count() };
            Taxi taxi = new Taxi() { CurrentDriver = driver };
            transports.Add(taxi);
        }

        public override void AddChildSeat()
        {
            ((Taxi)transports.Last<TransportVehicle>()).CountOfChildSafetySeat++;
        }
        

        public override void AddBelt()
        {
            //throw new NotImplementedException();
        }
    }
}
