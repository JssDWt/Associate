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
        /// <param name="dinges">The type that might have the generic type as base type or interface.</param>
        /// <param name="genericType">The generic type to test against.</param>
        /// <returns>A value indicating whether the specified type is of the specified generic type.</returns>
        public static bool IsOfGenericType (this TypeInfo type, Type genericType) 
        {
            // If any is null, this won't be useful.
            if (type == null || genericType == null) 
            {
                return false;
            }

            while (type != null) 
            {
                // If this type is generic and the generic type definition matches the generic type, Found it!
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == genericType) 
                {
                    return true;
                }

                // Apparently this type does not match the generic type. Maybe one of its interfaces?
                foreach (var interfees in type.ImplementedInterfaces) 
                {
                    if (interfees.GetTypeInfo().IsOfGenericType(genericType)) 
                    {
                        return true;
                    }
                }

                // Also the interfaces don't match the generic type. The base type maybe? (if it exists).
                type = type.BaseType?.GetTypeInfo();
            }

            // True would have been returned if this was a match. Return false now.
            return false;
        }

        public static Type[] GetGenericParameterTypes(this TypeInfo type, Type genericType)
        {
            if (type == null) return null;

            while (type != null)
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == genericType) 
                {
                    return type.GenericTypeArguments;
                }

                // Apparently this type does not match the generic type. Maybe one of its interfaces?
                foreach (var interfees in type.ImplementedInterfaces) 
                {
                    var interfaceTypes = interfees.GetTypeInfo().GetGenericParameterTypes(genericType);
                    if (interfaceTypes != null)
                    {
                        return interfaceTypes;
                    }
                }

                // Also the interfaces don't match the generic type. The base type maybe? (if it exists).
                type = type.BaseType?.GetTypeInfo();
            }

            return null;
        }

        public static bool IsConcreteType(this TypeInfo type)
        {
            bool isConcrete = false;
            
            if (type != null)
            {
                isConcrete = !type.IsAbstract && !type.IsInterface;
            }

            return  isConcrete;
        }

        public static ConstructorInfo GetConstructor(this TypeInfo type, params Type[] parameters)
        {
            ConstructorInfo result = null;

            if (type != null)
            {
                result = type.DeclaredConstructors.FirstOrDefault(
                    c => c.GetParameters().Select(
                        p => p.ParameterType).SequenceEqual(parameters));
            }

            return result;
        }
    }
}