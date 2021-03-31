using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Singleton test
            Console.WriteLine("Singleton test (waited: BusFabric #1 and TaxiFabric #1)");
            BoardBus myBoardBus = BoardBus.getInstance("BusFabric #1");
            myBoardBus = BoardBus.getInstance("BusFabric #2");
            myBoardBus.WhatIsTheFabric();
            
            BoardTaxi myBoardTaxi = BoardTaxi.getInstance("TaxiFabric #1");
            myBoardTaxi = BoardTaxi.getInstance("TaxiFabric #2");
            myBoardTaxi.WhatIsTheFabric();
            Console.WriteLine();

            List<Passenger> myPassengers1 = new List<Passenger>();
            for (int i=0; i< 14; i++)
                myPassengers1.Add(new Passenger() { FIO = "Pass #" + i });

            List<Passenger> myPassengers2 = new List<Passenger>();
            for (int i = 0; i < 31; i++)
                myPassengers2.Add(new Passenger() { FIO = "Pass #" + i });
            
            for (int i = 0; i < 3; i++)
            {
                myBoardTaxi.BoardDriver(new TaxiDriver() { FIO = "TaxiDriver #" + i });
            }

            for (int i = 0; i < 2; i++)
            {
                myBoardBus.BoardDriver(new BusDriver() { FIO = "BusDriver #" + i });
            }

            //First test
            List<TransportVehicle> myTransport1 = myBoardTaxi.CreateTransport(myPassengers1);
            for (int i = 0; i < myTransport1.Count(); i++)
            {
                myTransport1[i].Ride();
            }

            Console.WriteLine();
            //Second test
            List<TransportVehicle> myTransport2 = myBoardBus.CreateTransport(myPassengers2);
            for (int i = 0; i < myTransport2.Count(); i++)
            {
                myTransport2[i].Ride();
            }
           

            Console.ReadKey();
        }
    }
}
