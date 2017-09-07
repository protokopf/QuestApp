namespace Justus.QuestApp.AbstractLayer.Helpers.Behaviours
{
    /// <summary>
    /// Interface for types, that can be initialize.
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Initialize implementation.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Points, whether implementation has been initialized.
        /// </summary>
        /// <returns></returns>
        bool IsInitialized();
    }
}
