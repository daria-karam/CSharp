using PharmacyApp.Database;
using System;
using System.Data.SqlClient;

namespace PharmacyApp.Strategies
{
    /// <inheritdoc cref="DbStrategy">
    public sealed class WarehouseStrategy : DbStrategy
    {
        /// <inheritdoc/>
        public override TableName DbTableName => TableName.Warehouses;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="db">Контекст базы данных</param>
        public WarehouseStrategy(DbContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public override void CreateElement()
        {
            string name = ConsoleHelper.ReadStringValue("наименование", 100);
            int pharmacyId = ReadForeignKey("идентификатор аптеки",TableName.Pharmacies);

            var command = new SqlCommand(
                $"INSERT INTO {DbTableName} "
                + "(Name, PharmacyId) "
                + "VALUES (@name, @pharmacyId)");
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@pharmacyId", pharmacyId);

            _db.ExecuteSqlCommand(command);
        }

        /// <inheritdoc/>
        public override void ShowElements()
        {
            var command = new SqlCommand($"SELECT * FROM {DbTableName}");

            var result = _db.ExecuteSelectSqlCommand(command, "Id\tId аптеки\tНазвание", 3);
            Console.WriteLine(Environment.NewLine + result);
        }
    }
}
