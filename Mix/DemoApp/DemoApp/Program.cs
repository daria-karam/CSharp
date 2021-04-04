using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    class Program
    {
        static DBWorker worker = new DBWorker();

        public static object threadLock = new object();

        static void Main(string[] args)
        {
            //worker.DBFirstInit();

            worker.Deleted += new DBWorker.DeleteHandler(message => Console.WriteLine(message));            

            //worker.DeleteBusByNumber("555");

            worker.ShowData();

            Console.ReadKey();
        }
    }
}
