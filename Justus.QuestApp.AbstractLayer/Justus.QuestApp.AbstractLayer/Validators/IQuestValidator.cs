using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;

namespace Justus.QuestApp.AbstractLayer.Validators
{
    /// <summary>
    /// Interface for quest validators.
    /// </summary>
    public interface IQuestValidator<out TResponse>
        where TResponse : IResponse
    {
        /// <summary>
        /// Validates quest and returns response.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        TResponse Validate(Quest quest);
    }
}
