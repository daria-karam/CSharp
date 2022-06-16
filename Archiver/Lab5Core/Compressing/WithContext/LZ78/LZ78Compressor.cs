using Lab5Core.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab5Core.Compressing.WithContext.LZ78
{
    class LZ78Compressor : ICompressor
    {
        #region fields for debugging

        List<LZ78Node> compression;
        List<bool> compressionBoolsBefore;
        List<bool> compressionBoolsAfter;
        string compressionBoolsAsString;
        string compressionString;
        string compressionString1;

        List<LZ78Node> decompression;
        List<bool> decompressionBoolsBefore;
        List<bool> decompressionBoolsAfter;
        string decompressionBoolsAsString;
        string decompressionString;

        #endregion

        public Dictionary<byte, long> Compress(ref List<bool> bools)
        {
            compressionBoolsBefore = bools; // for debugging

            var str = ConvertHelper.GetStringByBools(bools);

            var bools1 = ConvertHelper.GetBoolsByString(str); // for debugging
            compressionString1 = ConvertHelper.GetStringByBools(bools1); // for debugging

            string buffer = ""; // текущий префикс
            Dictionary<string, int> dict = new Dictionary<string, int>(); // словарь

            dict[""] = 0;

            List<LZ78Node> ans = new List<LZ78Node>(); // ответ

            for (int i = 0; i < str.Length; i++)
            {
                if (dict.ContainsKey(buffer + str[i]))
                {
                    buffer += str[i];
                } // можем ли мы увеличить префикс
                else
                {
                    ans.Add(new LZ78Node() { Pos = dict[buffer], Next = str[i] }); // добавляем пару в ответ

                    dict[buffer + str[i]] = dict.Count; // добавляем слово в словарь

                    buffer = ""; // сбрасываем буфер
                }
            }

            // если буффер не пуст - этот код уже был, нужно его добавить в конец словаря
            if (!string.IsNullOrEmpty(buffer))
            {
                var last_ch = buffer.Last(); // берем последний символ буффера, как "новый" символ

                buffer.Remove(buffer.Length - 1); // удаляем последний символ из буфера

                ans.Add(new LZ78Node { Pos = dict[buffer], Next = last_ch }); // добавляем пару в ответ
            }

            bools = ConvertHelper.GetBoolsByLZ78NodeList(ans);

            // for debugging
            compression = ans;
            compressionBoolsAfter = bools;
            compressionBoolsAsString = string.Join("", bools.Select(_ => _ ? "1" : "0"));
            compressionString = str;

            return new Dictionary<byte, long>();
        }

        public void Decompress(Dictionary<byte, long> frequences, ref List<bool> bools)
        {
            decompressionBoolsBefore = bools; // for debugging

            var encoded = ConvertHelper.GetLZ78NodeListByBools(bools);

            List<string> dict = new List<string>(); // словарь, слово с номером 0 — пустая строка

            dict.Add("");

            string ans = ""; // ответ

            for (int i = 0; i < encoded.Count; i++)
            {
                var word = dict[encoded[i].Pos] + (char)encoded[i].Next; // составляем слово из уже известного из словаря и новой буквы

                ans += word; // приписываем к ответу

                dict.Add(word);
            }

            bools = ConvertHelper.GetBoolsByString(ans);
            if (compressionBoolsBefore.Count != bools.Count)
            {
                bools = bools.LongTake(bools.Count - 8);
            }

            // for debugging
            decompression = encoded;
            decompressionBoolsAfter = bools;
            decompressionBoolsAsString = string.Join("", bools.Select(_ => _ ? "1" : "0"));
            decompressionString = ans;

            var areEqual = CommonHelper.AreEqualObjects(compression, decompression); // always true
            var areEqualbools1 = CommonHelper.AreEqualObjects(compressionBoolsBefore, decompressionBoolsAfter);
            var areEqualbools2 = CommonHelper.AreEqualObjects(compressionBoolsAfter, decompressionBoolsBefore); // always true
        }
    }
}
