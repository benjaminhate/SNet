using System;
using System.Collections;
using System.Linq;

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
            return CheckClass(a, b, typeof(T));
        }

        /// <summary>
        /// Check if two class objects are equal
        /// </summary>
        /// <param name="a">The first class object</param>
        /// <param name="b">The second class object</param>
        /// <param name="type">The type of the class</param>
        /// <returns>True if the class objects are equal; false if not</returns>
        /// <exception cref="ArgumentException">The type is not a class type</exception>
        public static bool CheckClass(object a, object b, Type type)
        {
            if(!type.IsClass)
                throw new ArgumentException("The type is not a class type", nameof(type));
            
            var infos = GetInfos(type);

            return !(from info in infos let fieldType = info.FieldType let aVal = info.GetValue(a) let bVal = info.GetValue(b) where !TypeCheck(aVal, bVal) select aVal).Any();
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
            return CheckList(a, b, typeof(T));
        }
        
        /// <summary>
        /// Check if two list objects are equal
        /// </summary>
        /// <param name="a">The first list object</param>
        /// <param name="b">The second list object</param>
        /// <param name="type">The type of the list</param>
        /// <returns>True if the list objects are equal; false if not</returns>
        /// <exception cref="ArgumentException">The type is not derived from IList</exception>
        public static bool CheckList(object a, object b, Type type)
        {
            if (!typeof(IList).IsAssignableFrom(type))
            {
                throw new ArgumentException("Type should be derived from IList to use CheckList function", nameof(type));
            }
            
            var aList = (IList) a;
            var bList = (IList) b;
            
            var aCount = aList.Count;
            var bCount = bList.Count;
            if (aCount != bCount)
                return false;

            for (var i = 0; i < aCount; i++)
            {
                var aSubVal = aList[i];
                var bSubVal = bList[i];
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
                var success = CheckList(a, b, type);
                if (!success)
                    return false;
            }
            else
            {
                var success = CheckClass(a, b, type);
                if (!success)
                    return false;
            }
            return true;
        }
    }
}
