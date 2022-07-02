using PharmacyApp.Database;
using System;
using System.Data.SqlClient;

namespace PharmacyApp.Strategies
{
    /// <summary>
    /// Стратегия работы с базой данных
    /// </summary>
    public abstract class DbStrategy
    {
        protected DbContext _db;

        /// <summary>
        /// Имя таблицы из базы данных
        /// </summary>
        public abstract TableName DbTableName { get; }

        /// <summary>
        /// Создать элемент
        /// </summary>
        public abstract void CreateElement();

        /// <summary>
        /// Вывести все элементы таблицы
        /// </summary>
        public abstract void ShowElements();

        /// <summary>
        /// Удалить элемент
        /// </summary>
        public virtual void DeleteElement()
        {
            int id = ReadForeignKey("Id удаляемого элемента", DbTableName);

            var command = new SqlCommand($"DELETE FROM {DbTableName} WHERE Id = @id");
            command.Parameters.AddWithValue("@id", id);

            _db.ExecuteSqlCommand(command);
        }

        /// <summary>
        /// Считать внешний ключ из консоли
        /// </summary>
        /// <param name="name">Название значения</param>
        /// <returns>Внешний ключ</returns>
        public virtual int ReadForeignKey(string name, TableName tableName)
        {
            int id = ConsoleHelper.ReadUIntValue(name);
            while (!_db.CheckForeignKey(tableName, id))
            {
                Console.WriteLine("Идентификатор не найден в главной таблице. Попробуйте снова");
                id = ConsoleHelper.ReadUIntValue(name);
            }

            return id;
        }
    }
}
