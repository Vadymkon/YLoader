using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YLoader
{
    static class MyExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static DateTime toDateTime(this String line)
        {
            List<int> datePartList = line.Trim().Split('.').ToList().Select(y => Convert.ToInt32(y)).ToList();
            DateTime PublishedDate = new DateTime(datePartList[2], datePartList[1], datePartList[0]);
            return PublishedDate;
        }

        public static String formatOff(this String line)
        {
            List<String> Symbols = new List<string> { ".", "_", "-" }; //to space
            Symbols.ForEach(x => { line = line.Replace(x, " "); });
            List<String> SymbolsOff = new List<string> { "(", ")" }; //symbols which replacing to NOTHING
            SymbolsOff.ForEach(x => { line = line.Replace(x, ""); });
            return line.ToLower().Trim();
        }
    }
}
