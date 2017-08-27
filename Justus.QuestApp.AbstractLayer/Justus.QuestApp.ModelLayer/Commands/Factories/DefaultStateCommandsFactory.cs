using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Other;
using Justus.QuestApp.ModelLayer.Commands.Repository;
using Justus.QuestApp.ModelLayer.Commands.State;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;

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
            return new CompositeCommand(new ICommand[]
            {
                new DoneQuestCommand(quest, _questTree),
                new RecountQuestProgressCommand(quest, _questTree), 
            });
        }

        ///<inheritdoc/>
        public ICommand FailQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new CompositeCommand(new ICommand[]
            {
                new FailQuestCommand(quest, _questTree),
                new RecountQuestProgressCommand(quest, _questTree),
            });
        }

        ///<inheritdoc/>
        public ICommand StartQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new CompositeCommand(new ICommand[]
            {
                new StartQuestCommand(quest, _questTree),
                new RecountQuestProgressCommand(quest, _questTree),
                new UpdateUpHierarchyCommand(quest, _questTree)
            });
        }

        ///<inheritdoc/>
        public ICommand CancelQuest(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            return new CompositeCommand(new ICommand[]
            {
                new CancelQuestCommand(quest, _questTree),
                new RecountQuestProgressCommand(quest, _questTree),
            });
        } 


        #endregion
    }
}
