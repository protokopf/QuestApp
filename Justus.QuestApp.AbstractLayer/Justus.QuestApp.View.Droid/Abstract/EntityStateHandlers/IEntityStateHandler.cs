using Android.OS;

namespace Justus.QuestApp.View.Droid.Abstract.EntityStateHandlers
{
    /// <summary>
    /// Used for saving/extraction entity from bundles.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityStateHandler<TEntity>
    {
        /// <summary>
        /// Saves entity in bundle.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <param name="bundle"></param>
        bool Save(string key, TEntity entity, Bundle bundle);

        /// <summary>
        /// Extracts entity from bundle.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bundle"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Extract(string key, Bundle bundle, ref TEntity entity);
    }
}