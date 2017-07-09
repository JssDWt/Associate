namespace Societatis.Misc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class TypeExtensions
    {
        public static bool IsInstanceOfGenericType(this object instance, Type genericType)
        {
            instance.ThrowIfNull(nameof(instance));
            genericType.ThrowIfNull(nameof(genericType));

            if (!genericType.GetTypeInfo().IsGenericType)
            {
                throw new ArgumentException("Generic type to test against is not a generic type.", nameof(genericType));
            }

            TypeInfo type = instance.GetType().GetTypeInfo();
            while (type != null)
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
                type = type.BaseType?.GetTypeInfo();
            }
            
            return false;
        }
    }
}