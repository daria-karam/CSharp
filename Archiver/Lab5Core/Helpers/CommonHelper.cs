using System.Text.Json;

namespace Lab5Core.Helpers
{
    public class CommonHelper
    {
        public static bool AreEqualObjects<T>(T expected, T actual)
        {
            var expectedString = JsonSerializer.Serialize(expected);
            var actualString = JsonSerializer.Serialize(actual);

            return expectedString == actualString;
        }
    }
}
