using System;

namespace PharmacyApp
{
    /// <summary>
    /// Класс вспомогательных методов для работы с консолью
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Считать строковое значение из консоли
        /// </summary>
        /// <param name="name">Название значения</param>
        /// <param name="maxLength">Максимальная длина строки</param>
        /// <returns>Строковое значение</returns>
        public static string ReadStringValue(string name, int maxLength)
        {
            string value;

            Console.Write($"Введите {name} (до {maxLength} символов): ");
            while (true)
            {
                value = Console.ReadLine();
                if (name.Length <= maxLength)
                {
                    break;
                }

                Console.Write("Значение некорректно. Попробуйте снова: ");
            }

            return value;
        }

        /// <summary>
        /// Считать числовое значение из консоли
        /// </summary>
        /// <param name="name">Название значения</param>
        /// <returns>Натуральное число</returns>
        public static int ReadUIntValue(string name)
        {
            string value;

            Console.Write($"Введите {name} (натуральное число): ");
            while (true)
            {
                value = Console.ReadLine();
                if (int.TryParse(value, out int number) && number > 0)
                {
                    return number;
                }

                Console.Write("Значение некорректно. Попробуйте снова: ");
            }
        }
    }
}
