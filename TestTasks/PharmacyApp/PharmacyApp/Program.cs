using PharmacyApp.Database;
using PharmacyApp.Strategies;
using System;
using System.Text;

namespace PharmacyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var db = new DbContext();
            var strategiesStorage = new StrategiesStorage(db);
            var menuText = GetMenuText();

            DbStrategy dbStrategy;
            string choice;

            Console.WriteLine("Добро пожаловать в приложение PharmacyApp!");

            while (true)
            {
                Console.WriteLine(menuText);

                choice = Console.ReadLine();

                if (Enum.TryParse(choice, out TableName tableName)
                    && Enum.IsDefined(typeof(TableName), tableName))
                {
                    dbStrategy = strategiesStorage.GetDbStrategy(tableName);

                    if (dbStrategy != null)
                    {
                        DoSomethingWithTable(dbStrategy);
                    }
                }
                else if (choice == "-1")
                {
                    int id = ConsoleHelper.ReadUIntValue("идентификатор аптеки");
                    while (!db.CheckForeignKey(TableName.Pharmacies, id))
                    {
                        Console.WriteLine("Идентификатор не найден в главной таблице. Попробуйте снова");
                        id = ConsoleHelper.ReadUIntValue("идентификатор аптеки");
                    }

                    var result = db.ShowPharmacyEntry(id);
                    Console.WriteLine(result);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Выполнить действие с таблицей из БД
        /// </summary>
        /// <param name="dbStrategy">Стратегия для работы с таблицей</param>
        private static void DoSomethingWithTable(DbStrategy dbStrategy)
        {
            Console.WriteLine("\nВыберите действие:\n"
                + "\t0 => Добавить элемент\n"
                + "\t1 => Удалить элемент\n"
                + "\t2 => Вывести таблицу\n"
                + "\t_ => Выйти\n");

            switch (Console.ReadLine())
            {
                case "0":
                    dbStrategy.CreateElement();
                    break;

                case "1":
                    dbStrategy.DeleteElement();
                    break;

                case "2":
                    dbStrategy.ShowElements();
                    break;
            }
        }

        /// <summary>
        /// Получить текст для главного меню
        /// </summary>
        /// <returns>Текст для главного меню</returns>
        private static string GetMenuText()
        {
            var stringBuilder = new StringBuilder("\nВыберите действие:\n");

            stringBuilder.AppendLine("\t-1 => Вывести все товары аптеки");

            foreach (TableName value in Enum.GetValues(typeof(TableName)))
            {
                stringBuilder.AppendLine($"\t{(int)value} => Работать с таблицей '{value.GetRussianDescription()}'");
            }

            stringBuilder.AppendLine("\t_ => Выйти");

            return stringBuilder.ToString();
        }
    }
}
