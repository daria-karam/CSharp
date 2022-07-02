using System;

namespace PharmacyApp.Database
{
    /// <summary>
    /// Класс расширений для названий таблиц
    /// </summary>
    public static class TableNameExtensions
    {
        /// <summary>
        /// Получить русскоязычное описание
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <returns>Русское название таблицы</returns>
        public static string GetRussianDescription(this TableName tableName) => tableName switch
        {
            TableName.Pharmacies => "Список аптек",
            TableName.Products => "Список товарных наименований",
            TableName.Shipments => "Список партий",
            TableName.Warehouses => "Список складов",
            _ => throw new NotImplementedException("Данная таблица не поддерживается"),
        };
    }
}
