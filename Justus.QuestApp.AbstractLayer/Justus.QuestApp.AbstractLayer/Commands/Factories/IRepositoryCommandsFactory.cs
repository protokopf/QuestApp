using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Commands.Factories
{
    /// <summary>
    /// Interface for repository commands factories.
    /// </summary>
    public interface IRepositoryCommandsFactory
    {
        /// <summary>
        /// Return command, that add quest.
        /// </summary>
        /// <param name="quest"></param>
        Command AddQuest(Quest quest);

        /// <summary>
        /// Returns command that delete quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        Command DeleteQuest(Quest quest);

        /// <summary>
        /// Returns command, that updates quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        Command UpdateQuest(Quest quest);
    }
}
