namespace Societatis.Misc
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Class containing extension methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException" /> when the string is null or whitespace.
        /// </summary>
        /// <param name="subject">The string to perform check on.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown when the subject is null or whitespace.</exception>
        public static void ThrowIfNullOrWhiteSpace(this string subject, string name)
        {
            ThrowIfNullOrWhiteSpace(subject, name, "Argument cannot be null or whitespace.");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException" /> when the string is null or whitespace.
        /// </summary>
        /// <param name="subject">The string to perform check on.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="message">The message to supply to the exception.</param>
        /// <exception cref="ArgumentException">Thrown when the subject is null or whitespace.</exception>
        public static void ThrowIfNullOrWhiteSpace(this string subject, string name, string message)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException(message, name);
            }
        }
    }
}