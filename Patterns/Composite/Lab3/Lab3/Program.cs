using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            Airplane airplane = new Airplane(2000);
            Staff staff = new Staff();
            Pilot p1 = new Pilot() { FIO = "p1" };
            Pilot p2 = new Pilot() { FIO = "p2" };

            List<Stewardess> stewardesses = new List<Stewardess>();
            for (int i = 0; i < 7; i++)
            {
                stewardesses.Add(new Stewardess() { FIO = "s" + i });
            }

            List<Passenger> passengers = new List<Passenger>();
            for (int i = 0; i < 180; i++)
            {
                passengers.Add(new Passenger(random.Next(5, 60)) { FIO = "p" + i });
            }

            staff.Add(p1);
            staff.Add(p2);
            
            while (stewardesses.Count != 0 && staff.Add(stewardesses.First<Stewardess>()))
            {
                stewardesses.RemoveRange(0, 1);
            }

            airplane.Add(staff);

            Console.WriteLine(airplane.GetInfo());

            FirstClass firstClass = new FirstClass();
            while (passengers.Count != 0 && firstClass.Add(passengers.First<Passenger>()))
            {
                passengers.RemoveRange(0, 1);
            }

            BusinessClass businessClass = new BusinessClass();
            while (passengers.Count != 0 && businessClass.Add(passengers.First<Passenger>()))
            {
                passengers.RemoveRange(0, 1);
            }

            EconomyClass economyClass = new EconomyClass();
            while (passengers.Count != 0 && economyClass.Add(passengers.First<Passenger>()))
            {
                passengers.RemoveRange(0, 1);
            }

            Console.WriteLine();
            
            airplane.Add(firstClass);
            airplane.Add(businessClass);
            airplane.Add(economyClass);

            Console.WriteLine(airplane.GetInfo());

            airplane.RemoveBaggageFromFlight(airplane.GetBaggageWeight() - airplane.maxBaggageWeight);
            Console.WriteLine();
            Console.WriteLine(airplane.GetInfo());

            Console.ReadKey();
        }
    }
}
