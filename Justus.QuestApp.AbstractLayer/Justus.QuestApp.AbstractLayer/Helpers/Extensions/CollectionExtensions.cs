using System.Collections.Generic;

namespace Justus.QuestApp.AbstractLayer.Helpers.Extensions
{
    /// <summary>
    /// Extensions for collection implementations
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Moves element within collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="oldPosition"></param>
        /// <param name="newPosition"></param>
        /// <returns></returns>
        public static bool Move<T>(this IList<T> collection, int oldPosition, int newPosition)
        {
            if (oldPosition == newPosition)
            {
                return true;
            }
            if (!collection.InRange(oldPosition) || !collection.InRange(newPosition))
            {
                return false;
            }
            T item = collection[oldPosition];
            collection.RemoveAt(oldPosition);
            collection.Insert(newPosition, item);
            return true;

        }

        /// <summary>
        /// Points, whether index is in range of collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool InRange<T>(this ICollection<T> collection, int index)
        {
            return index >= 0 && index < collection.Count;
        }

    }
}
