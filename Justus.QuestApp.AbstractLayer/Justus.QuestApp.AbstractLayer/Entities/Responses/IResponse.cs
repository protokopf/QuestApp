namespace Justus.QuestApp.AbstractLayer.Entities.Responses
{
    /// <summary>
    /// Interface for responces.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Points, whether response is successfull or not.
        /// </summary>
        bool IsSuccessful { get; }
    }
}
