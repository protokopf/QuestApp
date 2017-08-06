namespace Justus.QuestApp.AbstractLayer.Helpers.Behaviours
{
    /// <summary>
    /// Interface for types, that can be refreshed.
    /// </summary>
    public interface IRefreshable
    {
        /// <summary>
        /// Refreshes current implementation.
        /// </summary>
        void Refresh();
    }
}
