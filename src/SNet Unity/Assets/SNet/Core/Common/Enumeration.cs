using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SNet.Core.Common
{
    
    public abstract partial class Enumeration : IConvertible, IComparable, IFormattable
{
    public string Name { get; }
    public int Value { get; }

    protected Enumeration(int id, string name)
    {
        Value = id;
        Name = name;
    }

    #region Equality members

    public override bool Equals(object obj)
    {
        var otherValue = obj as Enumeration;
        if (otherValue == null)
        {
            return false;
        }
        var typeMatches = GetType() == obj.GetType();
        var valueMatches = Value.Equals(otherValue.Value);
        return typeMatches && valueMatches;
    }

    protected bool Equals(Enumeration other)
    {
        return string.Equals(Name, other.Name) && Value == other.Value;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Value;
        }
    }

    #endregion

    #region Implementation of IComparable

    public int CompareTo(object other)
    {
        return Value.CompareTo(((Enumeration)other).Value);
    }

    #endregion

    #region ToString methods

    public string ToString(string format)
    {
        if (string.IsNullOrEmpty(format))
        {
            format = "G";
        }
        if (string.Compare(format, "G", StringComparison.OrdinalIgnoreCase) == 0)
        {
            return Name;
        }
        if (string.Compare(format, "D", StringComparison.OrdinalIgnoreCase) == 0)
        {
            return Value.ToString();
        }
        if (string.Compare(format, "X", StringComparison.OrdinalIgnoreCase) == 0)
        {
            return Value.ToString("X8");
        }
        throw new FormatException("Invalid format");
    }

    public override string ToString() => ToString("G");
    public string ToString(string format, IFormatProvider formatProvider) => ToString(format);

    #endregion

    #region Implementation of IConvertible

    TypeCode IConvertible.GetTypeCode() => TypeCode.Int32;
    bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Value, provider);
    char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(Value, provider);
    sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(Value, provider);
    byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(Value, provider);
    short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(Value, provider);
    ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Value, provider);
    int IConvertible.ToInt32(IFormatProvider provider) => Value;
    uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Value, provider);
    long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(Value, provider);
    ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Value, provider);
    float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(Value, provider);
    double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(Value, provider);
    decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Value, provider);
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException("Invalid cast.");
    string IConvertible.ToString(IFormatProvider provider) => ToString();
    object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        => Convert.ChangeType(this, conversionType, provider);

    #endregion
}

public abstract partial class Enumeration
{
    private static readonly Dictionary<Type, IEnumerable<Enumeration>> AllValuesCache =
        new Dictionary<Type, IEnumerable<Enumeration>>();

    #region Parse overloads

    public static TEnumeration Parse<TEnumeration>(string name)
        where TEnumeration : Enumeration
    {
        return Parse<TEnumeration>(name, false);
    }

    public static TEnumeration Parse<TEnumeration>(string name, bool ignoreCase)
        where TEnumeration : Enumeration
    {
        return ParseImpl<TEnumeration>(name, ignoreCase, true);
    }

    private static TEnumeration ParseImpl<TEnumeration>(string name, bool ignoreCase, bool throwEx)
        where TEnumeration : Enumeration
    {
        var value = GetValues<TEnumeration>()
            .FirstOrDefault(entry => StringComparisonPredicate(entry.Name, name, ignoreCase));
        if (value == null && throwEx)
        {
            throw new InvalidOperationException($"Requested value {name} was not found.");
        }
        return value;
    }

    #endregion

    #region TryParse overloads

    public static bool TryParse<TEnumeration>(string name, out TEnumeration value)
        where TEnumeration : Enumeration
    {
        return TryParse(name, false, out value);
    }

    public static bool TryParse<TEnumeration>(string name, bool ignoreCase, out TEnumeration value)
        where TEnumeration : Enumeration
    {
        value = ParseImpl<TEnumeration>(name, ignoreCase, false);
        return value != null;
    }

    #endregion

    #region Format overloads

    public static string Format<TEnumeration>(TEnumeration value, string format)
        where TEnumeration : Enumeration
    {
        return value.ToString(format);
    }

    #endregion

    #region GetNames

    public static IEnumerable<string> GetNames<TEnumeration>()
        where TEnumeration : Enumeration
    {
        return GetValues<TEnumeration>().Select(e => e.Name);
    }

    #endregion

    #region GetValues

    public static IEnumerable<TEnumeration> GetValues<TEnumeration>()
        where TEnumeration : Enumeration
    {
        var enumerationType = typeof(TEnumeration);
        if (AllValuesCache.TryGetValue(enumerationType, out var value))
        {
            return value.Cast<TEnumeration>();
        }
        return AddValueToCache(enumerationType, enumerationType
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(p => p.GetValue(enumerationType)).Cast<TEnumeration>());
    }

    private static IEnumerable<TEnumeration> AddValueToCache<TEnumeration>(Type key,
        IEnumerable<TEnumeration> value)
        where TEnumeration : Enumeration
    {
        AllValuesCache.Add(key, value);
        return value;
    }

    #endregion

    #region IsDefined overloads

    public static bool IsDefined<TEnumeration>(string name)
        where TEnumeration : Enumeration
    {
        return IsDefined<TEnumeration>(name, false);
    }

    public static bool IsDefined<TEnumeration>(string name, bool ignoreCase)
        where TEnumeration : Enumeration
    {
        return GetValues<TEnumeration>().Any(e => StringComparisonPredicate(e.Name, name, ignoreCase));
    }

    #endregion

    #region Helpers

    private static bool StringComparisonPredicate(string item1, string item2, bool ignoreCase)
    {
        var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return string.Compare(item1, item2, comparison) == 0;
    }

    #endregion
}
    
}