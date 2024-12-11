﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace VkInfrastructure.Extensions
{
    public static class ListExtensions
    {
        // The Random object this method uses.
        private static Random _rand = null;

        // Return num_items random values.
        public static List<T> PickRandom<T>(this List<T> values, int num_values)
        {
            // Create the Random object if it doesn't exist.
            if (_rand == null) _rand = new Random();

            // Don't exceed the array's length.
            if (num_values >= values.Count)
                num_values = values.Count - 1;

            // Make an array of indexes 0 through values.Length - 1.
            int[] indexes =
                Enumerable.Range(0, values.Count).ToArray();

            // Build the return list.
            List<T> results = new List<T>();

            // Randomize the first num_values indexes.
            for (int i = 0; i < num_values; i++)
            {
                // Pick a random entry between i and values.Length - 1.
                int j = _rand.Next(i, values.Count);

                // Swap the values.
                int temp = indexes[i];
                indexes[i] = indexes[j];
                indexes[j] = temp;

                // Save the ith value.
                results.Add(values[indexes[i]]);
            }

            // Return the selected items.
            return results;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
