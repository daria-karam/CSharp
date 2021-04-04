using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    interface ITransport
    {
        int Id { get; set; }
        string Number { get; set; }
        string Mark { get; set; }
        Driver CurrentDriver { get; set; }
        void Ride();
    }
}
