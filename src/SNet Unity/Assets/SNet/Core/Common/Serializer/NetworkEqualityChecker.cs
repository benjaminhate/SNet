using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SNet.Core.Common.Serializer
{
    public class NetworkEqualityChecker : NetworkSerializer
    {
        /// <summary>
        /// Check if two object are equal
        /// </summary>
        /// <param name="a">The first object</param>
        /// <param name="b">The second object</param>
        /// <typeparam name="T">The type of the objects</typeparam>
        /// <returns>True if the objects are equal; false if not</returns>
        public static bool Check<T>(T a, T b)
        {
            return TypeCheck(a, b);
        }

        /// <summary>
        /// Check if two class objects are equal
        /// </summary>
        /// <param name="a">The first class object</param>
        /// <param name="b">The second class object</param>
        /// <typeparam name="T">The type of the class</typeparam>
        /// <returns>True if the class objects are equal; false if not</returns>
        public static bool CheckClass<T>(T a, T b)
        {
            IEnumerable<FieldInfo> infos = GetInfos<T>();

            return !(from info in infos let type = info.FieldType let aVal = info.GetValue(a) let bVal = info.GetValue(b) where !TypeCheck(aVal, bVal) select aVal).Any();
        }

        /// <summary>
        /// Check if two list objects are equal
        /// </summary>
        /// <param name="a">The first list object</param>
        /// <param name="b">The second list object</param>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <returns>True if the list objects are equal; false if not</returns>
        public static bool CheckList<T>(T a, T b) where T : IList
        {
            var aCount = a.Count;
            var bCount = b.Count;
            if (aCount != bCount)
                return false;

            for (var i = 0; i < aCount; i++)
            {
                var aSubVal = a[i];
                var bSubVal = b[i];
                if (!TypeCheck(aSubVal, bSubVal))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check if two objects are equal using the Equals method
        /// </summary>
        /// <param name="a">The first object</param>
        /// <param name="b">The second object</param>
        /// <typeparam name="T">The type of the objects</typeparam>
        /// <returns>True if the objects are equal; false if not</returns>
        public static bool CheckEquals<T>(T a, T b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Check if two objects are equal or not
        /// The type of the objects are evaluated to determine which comparison method is best to compare them
        /// </summary>
        /// <param name="a">The first object</param>
        /// <param name="b">The second object</param>
        /// <returns>True if the objects are equal; false if not</returns>
        private static bool TypeCheck(object a, object b)
        {
            var aType = a.GetType();
            var bType = b.GetType();
            if (aType != bType)
                return false;

            var type = aType;
            if (type.IsPrimitive || type.IsEnum || type == typeof(string))
            {
                if (!CheckEquals(a,b))
                    return false;
            }
            else if (typeof(IList).IsAssignableFrom(type))
            {
                var success = (bool)typeof(NetworkEqualityChecker)
                    ?.GetMethod("CheckList")
                    ?.MakeGenericMethod(type)
                    ?.Invoke(null, new[] { a, b });
                if (!success)
                    return false;
            }
            else
            {
                var success = (bool)typeof(NetworkEqualityChecker)
                    ?.GetMethod("CheckClass")
                    ?.MakeGenericMethod(type)
                    ?.Invoke(null, new[] { a, b });
                if (!success)
                    return false;
            }
            return true;
        }
    }
}
