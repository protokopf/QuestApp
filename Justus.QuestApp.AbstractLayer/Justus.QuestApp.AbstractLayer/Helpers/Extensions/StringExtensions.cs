using System;

namespace Justus.QuestApp.AbstractLayer.Helpers.Extentions
{
    /// <summary>
    /// Extension methods for string type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks, whether argument null of whitespace.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="name"></param>
        public static void ThrowIfNullOrWhiteSpace(this string str, string name)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException($"{name} argument mustn't be null or whitespace!", name);
            }
        }
    }
}
