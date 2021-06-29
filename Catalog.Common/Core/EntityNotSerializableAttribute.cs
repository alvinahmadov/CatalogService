using System;

namespace Catalog.Common
{
    /// <summary>
    /// The following attribute is used when a property should not be serialized by a service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EntityNotSerializableAttribute : Attribute
    {
    }
}
