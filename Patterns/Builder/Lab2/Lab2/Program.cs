using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            Director director = new Director();
            BusBuilder busBuilder = new BusBuilder();
            TaxiBuilder taxiBuilder = new TaxiBuilder();
            IntersityBusBuilder intersityBusBuilder = new IntersityBusBuilder();
            List<Passenger> passengers = new List<Passenger>();

            /***************** First test *****************/

            Console.WriteLine("First test");

            for (int i = 0; i < 3; i ++)
            {
                passengers.Add(new Adult() { FIO = "a" + i });
            }
            passengers.Add(new Child() { FIO = "c0" });
            for (int i = 0; i < 5; i++)
            {
                passengers.Add(new BenefitRecipient() { FIO = "b" + i });
            }

            director.ConstructTransport(taxiBuilder, passengers);
            List<TransportVehicle> transports1 = taxiBuilder.GetResult();
            for (int i = 0; i < transports1.Count(); i++)
            {
                transports1[i].Ride();
            }

            /***************** Second test *****************/

            Console.WriteLine("\nSecond test");
            for (int i = 0; i < 10; i++)
            {
                passengers.Add(new Adult() { FIO = "a" + i });
            }
            for (int i = 0; i < 7; i++)
            {
                passengers.Add(new Child() { FIO = "c" + i });
            }
            for (int i = 0; i < 15; i++)
            {
                passengers.Add(new BenefitRecipient() { FIO = "b" + i });
            }

            director.ConstructTransport(busBuilder, passengers);
            List<TransportVehicle> transports2 = busBuilder.GetResult();
            for (int i = 0; i < transports2.Count(); i++)
            {
                transports2[i].Ride();
            }

            /***************** Third test *****************/

            Console.WriteLine("\nThird test");
            for (int i = 0; i < 10; i++)
            {
                passengers.Add(new Adult() { FIO = "a" + i });
            }
            for (int i = 0; i < 7; i++)
            {
                passengers.Add(new Child() { FIO = "c" + i });
            }
            for (int i = 0; i < 15; i++)
            {
                passengers.Add(new BenefitRecipient() { FIO = "b" + i });
            }

            director.ConstructTransport(intersityBusBuilder, passengers);
            List<TransportVehicle> transports3 = intersityBusBuilder.GetResult();
            for (int i = 0; i < transports3.Count(); i++)
            {
                transports3[i].Ride();
            }

            Console.ReadKey();
        }
    }
}
