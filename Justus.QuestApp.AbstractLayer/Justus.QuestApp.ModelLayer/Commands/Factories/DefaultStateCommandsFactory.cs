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
            return new UpHierarchyQuestCommand(quest, AbstractLayer.Entities.Quest.State.Done, _questTree);
        }

        ///<inheritdoc/>
        public ICommand FailQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new UpHierarchyQuestCommand(quest, AbstractLayer.Entities.Quest.State.Failed, _questTree);
        }

        ///<inheritdoc/>
        public ICommand StartQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new ThisQuestCommand(quest, AbstractLayer.Entities.Quest.State.Progress, _questTree);
        }

        ///<inheritdoc/>
        public ICommand CancelQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new DownHierarchyQuestCommand(quest, AbstractLayer.Entities.Quest.State.Idle, _questTree);
        } 


        #endregion
    }
}
