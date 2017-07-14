namespace Societatis.Misc {
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System;


    /// <summary>
    /// Class containing extension methods for <see cref="Type" /> objects.
    /// </summary>
    public static class TypeExtensions 
    {
        /// <summary>
        /// Gets a value indicating whether the current type is of the specified generic type 
        /// or has a base class or interface that is of the specified generic type.
        /// </summary>
        /// <param name="type">The type that might have the generic type as base type or interface.</param>
        /// <param name="genericType">The generic type to test against.</param>
        /// <returns>A value indicating whether the specified type is of the specified generic type.</returns>
        public static bool IsOfGenericType (this Type type, Type genericType) 
        {
            // If any is null, this won't be useful.
            if (type == null || genericType == null) 
            {
                return false;
            }

            TypeInfo typeInfo = type.GetTypeInfo();
            while (typeInfo != null) 
            {
                // If this type is generic and the generic type definition matches the generic type, Found it!
                if (typeInfo.IsGenericType &&
                    typeInfo.GetGenericTypeDefinition() == genericType) 
                {
                    return true;
                }

                // Apparently this type does not match the generic type. Maybe one of its interfaces?
                foreach (var interfees in typeInfo.ImplementedInterfaces) 
                {
                    if (interfees.IsOfGenericType(genericType)) 
                    {
                        return true;
                    }
                }

                // Also the interfaces don't match the generic type. The base type maybe? (if it exists).
                typeInfo = typeInfo.BaseType?.GetTypeInfo();
            }

            // True would have been returned if this was a match. Return false now.
            return false;
        }

        public static Type[] GetGenericParameterTypes(this Type type, Type genericType)
        {
            if (type == null) return null;

            var typeInfo = type.GetTypeInfo();

            while (typeInfo != null)
            {
                if (typeInfo.IsGenericType &&
                    typeInfo.GetGenericTypeDefinition() == genericType) 
                {
                    return typeInfo.GenericTypeArguments;
                }

                // Apparently this type does not match the generic type. Maybe one of its interfaces?
                foreach (var interfees in typeInfo.ImplementedInterfaces) 
                {
                    var interfaceTypes = interfees.GetGenericParameterTypes(genericType);
                    if (interfaceTypes != null)
                    {
                        return interfaceTypes;
                    }
                }

                // Also the interfaces don't match the generic type. The base type maybe? (if it exists).
                typeInfo = typeInfo.BaseType?.GetTypeInfo();
            }

            return null;
        }
    }
}