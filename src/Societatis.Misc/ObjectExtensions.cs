namespace Societatis.Misc
{
    using System;

    public static class ObjectExtensions
    {
        public static void ThrowIfNull<T>(this T obj, string paramName) where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}