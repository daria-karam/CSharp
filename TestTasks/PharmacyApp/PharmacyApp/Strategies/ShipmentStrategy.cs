using PharmacyApp.Database;
using System;
using System.Data.SqlClient;

namespace PharmacyApp.Strategies
{
    /// <inheritdoc cref="DbStrategy">
    public sealed class ShipmentStrategy : DbStrategy
    {
        /// <inheritdoc/>
        public override TableName DbTableName => TableName.Shipments;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="db">Контекст базы данных</param>
        public ShipmentStrategy(DbContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public override void CreateElement()
        {
            int warehouseId = ReadForeignKey("идентификатор склада", TableName.Warehouses);
            int productId = ReadForeignKey("идентификатор товара", TableName.Products);
            int count = ConsoleHelper.ReadUIntValue("количество в партии");

            var command = new SqlCommand(
                $"INSERT INTO {DbTableName} "
                + "(WarehouseId, ProductId, Count) "
                + "VALUES (@warehouseId, @productId, @count)");
            command.Parameters.AddWithValue("@warehouseId", warehouseId);
            command.Parameters.AddWithValue("@productId", productId);
            command.Parameters.AddWithValue("@count", count);

            _db.ExecuteSqlCommand(command);
        }

        /// <inheritdoc/>
        public override void ShowElements()
        {
            var command = new SqlCommand($"SELECT * FROM {DbTableName}");

            var result = _db.ExecuteSelectSqlCommand(command, "Id\tId продукта\tId склада\tКоличество", 4);
            Console.WriteLine(Environment.NewLine + result);
        }
    }
}
