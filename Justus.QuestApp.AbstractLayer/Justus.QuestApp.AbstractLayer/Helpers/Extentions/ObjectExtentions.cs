using System;

namespace Justus.QuestApp.AbstractLayer.Helpers.Extentions
{
    /// <summary>
    /// Extention methods for object and types derived from object.
    /// </summary>
    public static class ObjectExtentions
    {
        /// <summary>
        /// Throws exception, if current argument is null.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="name"></param>
        public static void ThrowIfNull(this object argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
