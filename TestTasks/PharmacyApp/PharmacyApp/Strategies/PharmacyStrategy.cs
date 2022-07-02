using PharmacyApp.Database;
using System;
using System.Data.SqlClient;

namespace PharmacyApp.Strategies
{
    /// <inheritdoc cref="DbStrategy">
    public sealed class PharmacyStrategy : DbStrategy
    {
        /// <inheritdoc/>
        public override TableName DbTableName => TableName.Pharmacies;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="db">Контекст базы данных</param>
        public PharmacyStrategy(DbContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public override void CreateElement()
        {
            string name = ConsoleHelper.ReadStringValue("наименование", 100);
            string address = ConsoleHelper.ReadStringValue("адрес", 255);
            string phone = ConsoleHelper.ReadStringValue("телефон", 16);

            var command = new SqlCommand(
                $"INSERT INTO {DbTableName} "
                + "(Name, Address, Phone) "
                + "VALUES (@name, @address, @phone)");
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@phone", phone);

            _db.ExecuteSqlCommand(command);
        }

        /// <inheritdoc/>
        public override void ShowElements()
        {
            var command = new SqlCommand($"SELECT * FROM {DbTableName}");

            var result = _db.ExecuteSelectSqlCommand(command, "Id\tНазвание\tАдрес\tТелефон", 4);
            Console.WriteLine(Environment.NewLine + result);
        }
    }
}
