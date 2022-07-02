using PharmacyApp.Database;
using System;
using System.Data.SqlClient;

namespace PharmacyApp.Strategies
{
    /// <inheritdoc cref="DbStrategy">
    public sealed class ProductStrategy : DbStrategy
    {
        /// <inheritdoc/>
        public override TableName DbTableName => TableName.Products;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="db">Контекст базы данных</param>
        public ProductStrategy(DbContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public override void CreateElement()
        {
            string name = ConsoleHelper.ReadStringValue("наименование", 100);

            var command = new SqlCommand();
            command.CommandText = $"INSERT INTO {DbTableName} (Name) VALUES (@name)";
            command.Parameters.AddWithValue("@name", name);

            _db.ExecuteSqlCommand(command);
        }

        /// <inheritdoc/>
        public override void ShowElements()
        {
            var command = new SqlCommand($"SELECT * FROM {DbTableName}");

            var result = _db.ExecuteSelectSqlCommand(command, "Id\tНазвание", 2);
            Console.WriteLine(Environment.NewLine + result);
        }
    }
}
