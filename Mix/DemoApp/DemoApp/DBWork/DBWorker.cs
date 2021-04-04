using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    class DBWorker
    {
        public delegate void DeleteHandler(string message);

        public event DeleteHandler Deleted;

        public void DBFirstInit()
        {
            Random random = new Random();
            List<Driver> drivers = new List<Driver>();
            for (int i = 0; i < 5; i++)
            {
                drivers.Add(new Driver() { FIO = $"driver {i}" });
            }

            List<Bus> buses = new List<Bus>();
            for (int i = 0; i < 7; i++)
            {
                buses.Add(new Bus() { Number = $"{i}{i}{i}", Mark = $"MB{i}", CurrentDriver = drivers[random.Next(0, 5)] });
            }

            List<Car> cars = new List<Car>();
            for (int i = 0; i < 11; i++)
            {
                cars.Add(new Car() { Number = $"{i}{i}{i}", Mark = $"MC{i}", CurrentDriver = drivers[random.Next(0, 5)] });
            }

            using (var db = new MyDataContext())
            {
                foreach (Driver driver in drivers)
                {
                    db.Drivers.Add(driver);
                }
                foreach (Bus bus in buses)
                {
                    db.Buses.Add(bus);
                }
                foreach (Car car in cars)
                {
                    db.Cars.Add(car);
                }

                db.SaveChanges();
            }
        }

        public void ShowData()
        {
            using (var db = new MyDataContext())
            {
                var drivers = db.Drivers.ToList();
                var buses = db.Buses.ToList();
                var cars = db.Cars.ToList();

                Parallel.ForEach(drivers, driver => driver.SayHello());
                Parallel.ForEach(buses, bus => bus.Ride());
                Parallel.ForEach(cars, car => car.Ride());

                /*
                foreach (Driver driver in drivers)
                {
                    driver.SayHello();
                }
                foreach (Bus bus in buses)
                {
                    bus.Ride();
                }
                foreach (Car car in cars)
                {
                    car.Ride();
                }
                */
            }
        }

        public void DeleteBusByNumber(string number)
        {
            Console.WriteLine("Deleting started");
            using (var db = new MyDataContext())
            {
                var buses = db.Buses.AsParallel().Where(bus => bus.Number == number);
                foreach (Bus bus in buses)
                {
                    //Deleted($"Bus #{number} was deleted");
                    IAsyncResult ar = Deleted.BeginInvoke($"Bus #{number} was deleted", null, null);
                    Deleted.EndInvoke(ar);
                    db.Buses.Remove(bus);
                }
                db.SaveChanges();
            }
            Console.WriteLine("Deleting completed");
        }
    }
}
