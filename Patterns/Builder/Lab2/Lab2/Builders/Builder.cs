using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    abstract class Builder
    {
        public List<TransportVehicle> transports;
        public abstract void PutDriver();
        public abstract void AddChildSeat();
        public abstract void AddBelt();
        public abstract void PutAdult(Adult adult);
        public abstract void PutChild(Child child);
        public abstract void PutBenefitRecipient(BenefitRecipient benefitRecipient);
        public abstract List<TransportVehicle> GetResult();
    }
}
