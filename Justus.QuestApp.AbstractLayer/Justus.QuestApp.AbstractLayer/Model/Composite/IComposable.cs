namespace Justus.QuestApp.AbstractLayer.Model.Composite
{
    /// <summary>
    /// Interface for composable types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IComposable<in T>
    {
        /// <summary>
        /// Composes current implementation with other implementation.
        /// </summary>
        /// <param name="other"></param>
        void Compose(T other);
    }
}
