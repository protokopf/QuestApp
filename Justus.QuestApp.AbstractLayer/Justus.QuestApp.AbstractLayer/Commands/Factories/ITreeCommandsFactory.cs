using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Commands.Factories
{
    /// <summary>
    /// Interface for repository commands factories.
    /// </summary>
    public interface ITreeCommandsFactory
    {
        /// <summary>
        /// Return command, that add quest.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="quest"></param>
        ICommand AddQuest(Quest parent, Quest quest);

        /// <summary>
        /// Returns command that delete quest.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="quest"></param>
        /// <returns></returns>
        ICommand DeleteQuest(Quest parent, Quest quest);

        /// <summary>
        /// Returns command, that updates quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        ICommand UpdateQuest(Quest quest);
    }
}
