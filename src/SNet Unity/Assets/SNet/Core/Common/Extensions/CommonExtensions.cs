using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace SNet.Core.Common.Extensions
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Replace the oldKey with the newKey in a dictionary
        /// </summary>
        /// <param name="dict">The dictionary to modify</param>
        /// <param name="oldKey">The key to replace</param>
        /// <param name="newKey">The key of replacement</param>
        /// <typeparam name="TK">The type of the keys in the dictionary</typeparam>
        /// <typeparam name="T">The type of the values in the dictionary</typeparam>
        public static void Replace<TK, T>(this Dictionary<TK, T> dict, TK oldKey, TK newKey)
        {
            var success = dict.TryGetValue(oldKey, out var value);
            if (!success) return;
            
            dict.Remove(oldKey);
            dict.Add(newKey, value);
        }
        
        /// <summary>
        /// Transform the enumValue into an array of names
        /// Get all the possible names of the Enum and if they are in the enumValue, add them to the array
        /// </summary>
        /// <param name="enumValue">The value to check</param>
        /// <returns>The array of names</returns>
        public static string[] ToFlagArray(this Enum enumValue)
        {
            var flagNames = new List<string>();

            var values = Enum.GetValues(enumValue.GetType());
            var names = Enum.GetNames(enumValue.GetType());

            for (var i = 0; i < values.Length; i++)
            {
                if(enumValue.HasFlag((Enum)values.GetValue(i)))
                    flagNames.Add(names[i]);
            }
            
            return flagNames.ToArray();
        }
        
        /// <summary>
        /// Transform the dictionary into an array of names
        /// Simulates an enum flag from a dictionary
        /// </summary>
        /// <param name="enumValue">The dictionary to check</param>
        /// <param name="value">The value to check from</param>
        /// <returns>The array of names</returns>
        public static string[] ToFlagArray(this Dictionary<int, string> enumValue, int value)
        {
            var flagNames = new List<string>();

            var values = enumValue.Keys.ToArray();
            var names = enumValue.Values.ToArray();

            for (var i = 0; i < values.Length; i++)
            {
                if((values[i] & value) == values[i])
                    flagNames.Add(names[i]);
            }
            
            return flagNames.ToArray();
        }

        /// <summary>
        /// Transform a list of values to a dictionary
        /// The dictionary simulates an enum flag value
        /// </summary>
        /// <param name="values">The list of values to use</param>
        /// <returns>The dictionary enum value</returns>
        public static Dictionary<int, string> ToFlagEnum(this List<string> values)
        {
            var dict = new Dictionary<int, string>();
            for (var i = 0; i < values.Count; i++)
            {
                var key = 1 << i;
                var value = values[i];
                dict.Add(key, value);
            }

            return dict;
        }

        /// <summary>
        /// Get a random item from a list
        /// </summary>
        /// <param name="list">The list to use</param>
        /// <typeparam name="T">The type of the values in the list</typeparam>
        /// <returns>A random item of the list</returns>
        public static T RandomItem<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count())];
        }
    }
}
