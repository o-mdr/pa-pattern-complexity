using System;
using System.Collections.Generic;
using System.Linq;

namespace PatternPA.Utils
{
    public static class Shuffle
    {
        public static IEnumerable<T> GetFisherYatesShuffle<T>(this IEnumerable<T> data)
        {
            var dictionary = new Dictionary<int, T>();
            var random = new Random();

            foreach (var item in data)
            {
                int id = random.Next();

                while (dictionary.ContainsKey(id))
                {
                    id = random.Next();
                }

                dictionary.Add(id, item);
            }

            // Sort by the random number
            return from item in dictionary
                   orderby item.Key
                   select item.Value;
        }
    }
}
