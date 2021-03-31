using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    abstract class Driver
    {
        public string FIO { get; set; }
        public abstract void Call();
    }
}
