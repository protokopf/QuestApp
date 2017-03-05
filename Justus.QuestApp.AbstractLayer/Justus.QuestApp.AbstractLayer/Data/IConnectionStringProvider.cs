namespace Justus.QuestApp.AbstractLayer.Data
{
    /// <summary>
    /// Interface for types, which provide connection string to data storage.
    /// </summary>
    public interface IConnectionStringProvider
    {
        /// <summary>
        /// Returns connection string to data storage.
        /// </summary>
        /// <returns></returns>
        string GetConnectionString();
    }
}
