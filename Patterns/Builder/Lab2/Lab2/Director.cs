using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Director
    {
        public void ConstructTransport(Builder builder, List<Passenger> passengers)
        {
            while (passengers.Count() != 0)
            {
                if (passengers.First<Passenger>().GetType() == typeof(Adult))
                {
                    builder.PutAdult((Adult)passengers.First<Passenger>());
                    builder.AddBelt();
                }
                if (passengers.First<Passenger>().GetType() == typeof(Child))
                {
                    builder.PutChild((Child)passengers.First<Passenger>());
                    builder.AddBelt();
                    builder.AddChildSeat();
                }
                if (passengers.First<Passenger>().GetType() == typeof(BenefitRecipient))
                {
                    builder.PutBenefitRecipient((BenefitRecipient)passengers.First<Passenger>());
                    builder.AddBelt();
                }
                passengers.RemoveRange(0, 1);
            }
        }
    }
}
