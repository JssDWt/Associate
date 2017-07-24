namespace Societatis.Misc
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Class containing extension methods for objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Throws if the specified object is null, using the specified name as parameter name.
        /// </summary>
        /// <param name="obj">The object to test to null.</param>
        /// <param name="paramName">The name of the parameter that represents the object. 
        /// It is assumed to be provided, since it is usually a compile time constant.</param>
        /// <exception cref="ArgumentNullException">Thrown when obj is null.</exception>
        public static void ThrowIfNull<T>(this T obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Throws if the specified object is null, using the specified name as parameter name.
        /// </summary>
        /// <param name="obj">The object to test to null.</param>
        /// <param name="paramName">The name of the parameter that represents the object. 
        /// It is assumed to be provided, since it is usually a compile time constant.</param>
        /// <param name="message">The message to supply to the exception.</param>
        /// <exception cref="ArgumentNullException">Thrown when obj is null.</exception>
        public static void ThrowIfNull<T>(this T obj, string paramName, string message)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName, message);
            }
        }

        public static bool IsInstanceOfGenericType(this object obj, Type genericType)
        {
            return obj.GetType().GetTypeInfo().IsOfGenericType(genericType);
        }
    }
}