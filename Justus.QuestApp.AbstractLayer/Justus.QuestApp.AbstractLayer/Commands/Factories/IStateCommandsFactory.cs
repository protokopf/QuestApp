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
        ICommand DoneQuest(Quest quest);

        /// <summary>
        /// Returns command which makes quest failed.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        ICommand FailQuest(Quest quest);

        /// <summary>
        /// Returns command which starts quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        ICommand StartQuest(Quest quest);

        /// <summary>
        /// Returns command which cancel quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        ICommand CancelQuest(Quest quest);
    }
}
