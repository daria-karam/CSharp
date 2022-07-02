using PharmacyApp.Database;
using System.Linq;

namespace PharmacyApp.Strategies
{
    /// <summary>
    /// Хранилище стратегий для работы с таблицами БД
    /// </summary>
    public class StrategiesStorage
    {
        /// <summary>
        /// Массив стратегий для работы с таблицами БД
        /// </summary>
        private readonly DbStrategy[] _strategies;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="db">Контекст базы данных</param>
        public StrategiesStorage(DbContext db)
        {
            _strategies = new DbStrategy[]
            {
                new ProductStrategy(db),
                new PharmacyStrategy(db),
                new WarehouseStrategy(db),
                new ShipmentStrategy(db)
            };
        }

        /// <summary>
        /// Получить стратегию для работы с таблицей
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DbStrategy GetDbStrategy(TableName tableName)
            => _strategies.FirstOrDefault(_ => _.DbTableName == tableName);
    }
}
