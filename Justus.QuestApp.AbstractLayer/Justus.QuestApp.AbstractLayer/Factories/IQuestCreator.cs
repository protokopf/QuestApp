using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Factories
{
    /// <summary>
    /// Interface for quest factories.
    /// </summary>
    public interface IQuestCreator
    {
        /// <summary>
        /// Creates quest.
        /// </summary>
        /// <returns></returns>
        Quest Create();
    }
}
