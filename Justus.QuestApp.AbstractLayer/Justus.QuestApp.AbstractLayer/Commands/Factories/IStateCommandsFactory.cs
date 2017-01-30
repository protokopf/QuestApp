using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.AbstractLayer.Commands.Factories
{
    /// <summary>
    /// Provide commands, which change state of quest.
    /// </summary>
    public interface IStateCommandsFactory
    {
        /// <summary>
        /// Returns command which make quest done.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        Command DoneQuest(Quest quest);

        /// <summary>
        /// Returns command which makes quest failed.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        Command FailQuest(Quest quest);

        /// <summary>
        /// Returns command which starts quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        Command StartQuest(Quest quest);

        /// <summary>
        /// Returns command which cancel quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        Command CancelQuest(Quest quest);
    }
}
