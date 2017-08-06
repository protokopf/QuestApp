using SQLite.Net.Interop;

namespace Justus.QuestApp.DataLayer.Platform
{
    /// <summary>
    /// Interface for providing ISQLitePlatform implementations.
    /// </summary>
    public interface ISqLitePlatformFactory
    {
        /// <summary>
        /// Returns ISQLitePlatform implementations.
        /// </summary>
        /// <returns></returns>
        ISQLitePlatform Create();
    }
}
