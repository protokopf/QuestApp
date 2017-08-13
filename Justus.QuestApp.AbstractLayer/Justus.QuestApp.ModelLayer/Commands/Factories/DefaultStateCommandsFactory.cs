using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.State;

namespace Justus.QuestApp.ModelLayer.Commands.Factories
{
    /// <summary>
    /// Update quests and make changes in questTree.
    /// </summary>
    public class DefaultStateCommandsFactory : IStateCommandsFactory
    {
        private readonly IQuestTree _questTree;

        /// <summary>
        /// Receives dependency to quest questTree.
        /// </summary>
        /// <param name="questTree"></param>
        public DefaultStateCommandsFactory(IQuestTree questTree)
        {
            questTree.ThrowIfNull(nameof(questTree));
            _questTree = questTree;
        }

        #region IStateCommandsFactory implementation

        ///<inheritdoc/>
        public ICommand DoneQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new DoneQuestCommand(quest, _questTree);
        }

        ///<inheritdoc/>
        public ICommand FailQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new FailQuestCommand(quest, _questTree);
        }

        ///<inheritdoc/>
        public ICommand StartQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new StartQuestCommand(quest, _questTree);
        }

        ///<inheritdoc/>
        public ICommand CancelQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new CancelQuestCommand(quest, _questTree);
        } 


        #endregion
    }
}
