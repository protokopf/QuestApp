namespace Justus.QuestApp.AbstractLayer.Services
{
    /// <summary>
    /// Interface to services.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Starts service.
        /// </summary>
        void Start();
        /// <summary>
        /// Stops service.
        /// </summary>
        void Stop();
    }
}