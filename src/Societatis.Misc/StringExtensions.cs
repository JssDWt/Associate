namespace Societatis.Misc
{
    using System;
    using System.Runtime.CompilerServices;

    public static class StringExtensions
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException" /> when the string is null or whitespace.
        /// </summary>
        /// <param name="subject">The string to perform check on.</param>
        /// <param name="name">The name of the parameter. This defaults to the name of the caller.</param>
        /// <exception cref="ArgumentException">Thrown when the subject is null or whitespace.</exception>
        public static void ThrowIfNullOrWhiteSpace(this string subject, string name)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", name);
            }
        }
    }
}