using System;

namespace SNet.Core.Common.Serializer
{
    /// <summary>
    /// Attribute to determine if the list variable should have its subtype encoded in the serialization
    /// </summary>
    internal class EncodeSubType : Attribute { }

    /// <summary>
    /// Attribute to determine if the variable should not be included in the serialization
    /// </summary>
    internal class NotIncluded : Attribute { }
}