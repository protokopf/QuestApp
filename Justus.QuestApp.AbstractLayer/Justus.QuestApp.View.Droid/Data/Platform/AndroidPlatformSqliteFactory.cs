using Justus.QuestApp.DataLayer.Platform;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;

namespace Justus.QuestApp.View.Droid.Data.Platform
{
    /// <summary>
    /// Returns Android implementation for ISQLitePlatform.
    /// </summary>
    public class AndroidPlatformSqliteFactory : ISqLitePlatformFactory
    {
        #region ISqLitePlatformFactory implementation

        ///<inheritdoc cref="ISqLitePlatformFactory"/>
        public ISQLitePlatform Create()
        {
            return new SQLitePlatformAndroid();
        }

        #endregion
    }
}
