using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace PharmacyApp.Database
{
    /// <summary>
    /// Класс работы с БД
    /// </summary>
    public class DbContext
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        private readonly SqlConnection _sqlConnection;

        /// <summary>
        /// .ctor
        /// </summary>
        public DbContext()
        {
            _sqlConnection = new SqlConnection(@"Server=(localdb)\mssqllocaldb;Database=PharmacyDb;Trusted_Connection=True");
            EnsureDatabaseExists();
            EnsureTablesExist();
        }

        /// <summary>
        /// Выполнить SQL-команду, не возвращающую значения
        /// </summary>
        /// <param name="command">SQL-команда</param>
        public void ExecuteSqlCommand(SqlCommand command)
        {
            try
            {
                _sqlConnection.Open();
                command.Connection = _sqlConnection;

                if (command.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine(Environment.NewLine + "Операция выполнена успешно");
                }
                else
                {
                    Console.WriteLine(Environment.NewLine + "Операция не выполнена");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(Environment.NewLine + $"Что-то пошло не так! Текст ошибки: {e.Message}");
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        /// <summary>
        /// Проверить значение внешнего ключа
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="key">Значение внешнего ключа</param>
        /// <returns>Существует ли строка в главной таблице</returns>
        public bool CheckForeignKey(TableName tableName, int key)
        {
            var exists = false;
            try
            {
                _sqlConnection.Open();

                var command = new SqlCommand($"SELECT COUNT(*) FROM {tableName} WHERE Id = @id");
                command.Parameters.AddWithValue("@id", key);
                command.Connection = _sqlConnection;

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    exists = reader.GetInt32(0) > 0;
                }

                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Что-то пошло не так! Текст ошибки: {e.Message}");
            }
            finally
            {
                _sqlConnection.Close();
            }

            return exists;
        }

        /// <summary>
        /// Выполнить SQL-команду, не возвращающую значения
        /// </summary>
        /// <param name="command">SQL-команда</param>
        public string ExecuteSelectSqlCommand(SqlCommand command, string title, int fieldsCount)
        {
            var stringBuilder = new StringBuilder();
            try
            {
                _sqlConnection.Open();
                command.Connection = _sqlConnection;
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    stringBuilder.AppendLine(title);

                    while (reader.Read())
                    {
                        for (int i = 0; i < fieldsCount; i++)
                        {
                            stringBuilder.Append($"{reader.GetValue(i)}\t");
                        }

                        stringBuilder.Append("\n");
                    }
                }
                else
                {
                    stringBuilder.AppendLine("Подходящие строки не найдены");
                }

                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Что-то пошло не так! Текст ошибки: {e.Message}");
            }
            finally
            {
                _sqlConnection.Close();
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Получить все товары аптеки
        /// </summary>
        /// <param name="pharmacyId">Идентификатор аптеки</param>
        public string ShowPharmacyEntry(int pharmacyId)
        {
            var command = new SqlCommand("SELECT Products.Name, SUM(Shipments.Count) "
                + "FROM Products JOIN Shipments ON Shipments.ProductId = Products.id "
                + "JOIN Warehouses ON Shipments.WarehouseId = Warehouses.id "
                + "WHERE Warehouses.PharmacyId = 1 "
                + "GROUP BY Products.Name");

            var result = ExecuteSelectSqlCommand(command, "Название\tКоличество", 2);
            return result;
        }

        /// <summary>
        /// Убедиться в существовании БД
        /// Если отсутствует - создать
        /// </summary>
        private void EnsureDatabaseExists()
        {
            var connection = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;");
            try
            {
                connection.Open();

                var path = @"Database\CreateDbScript.txt";
                if (File.Exists(path))
                {
                    var sql = File.ReadAllText(path);
                    var command = new SqlCommand(sql);
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Что-то пошло не так! Текст ошибки: {e.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Убедиться в существовании нужных таблиц в БД
        /// Если отсутствуют - создать
        /// </summary>
        private void EnsureTablesExist()
        {
            try
            {
                _sqlConnection.Open();

                var path = @"Database\CreateTablesScript.txt";
                if (File.Exists(path))
                {
                    var sql = File.ReadAllText(path);
                    var command = new SqlCommand(sql);
                    command.Connection = _sqlConnection;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Что-то пошло не так! Текст ошибки: {e.Message}");
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }
}
