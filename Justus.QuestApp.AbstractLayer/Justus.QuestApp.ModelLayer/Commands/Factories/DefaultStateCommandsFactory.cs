using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.State;

namespace Justus.QuestApp.ModelLayer.Commands.Factories
{
    /// <summary>
    /// Update quests and make changes in repository.
    /// </summary>
    public class DefaultStateCommandsFactory : IStateCommandsFactory
    {
        private readonly IQuestRepository _repository;

        /// <summary>
        /// Receives dependency to quest repository.
        /// </summary>
        /// <param name="repository"></param>
        public DefaultStateCommandsFactory(IQuestRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            _repository = repository;
        }

        #region IStateCommandsFactory implementation

        ///<inheritdoc/>
        public Command DoneQuest(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            return new UpHierarchyStateUpdateCommand(quest, QuestState.Done, _repository);
        }

        ///<inheritdoc/>
        public Command FailQuest(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            return new UpHierarchyStateUpdateCommand(quest, QuestState.Failed, _repository);
        }

        ///<inheritdoc/>
        public Command StartQuest(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            return new ThisStateUpdateCommand(quest, QuestState.Progress, _repository);
        }

        ///<inheritdoc/>
        public Command CancelQuest(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            return new DownHierarchyStateUpdateCommand(quest, QuestState.Idle, _repository);
        } 


        #endregion
    }
}
