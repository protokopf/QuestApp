using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Validators
{
    /// <summary>
    /// Interface for quest validators.
    /// </summary>
    public interface IQuestValidator
    {
        /// <summary>
        /// Validates quest and returns response.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        Response Validate(Quest quest);
    }
}
