using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Repository;
using System;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Other;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;

namespace Justus.QuestApp.ModelLayer.Commands.Factories
{
    /// <summary>
    /// Default repository commands factory.
    /// </summary>
    public class DefaultTreeCommandsFactory : ITreeCommandsFactory
    {
        private readonly IQuestTree _repository;

        /// <summary>
        /// Receives dependency to IQuestTree.
        /// </summary>
        /// <param name="repository"></param>
        public DefaultTreeCommandsFactory(IQuestTree repository)
        {
            repository.ThrowIfNull(nameof(repository));
            _repository = repository;
        }

        #region ITreeCommandsFactory implementation

        ///<inheritdoc/>
        public ICommand AddQuest(Quest parent, Quest quest)
        {
            return new CompositeCommand(new ICommand[]{
                new AddQuestCommand(_repository, parent, quest),
                new IsLeafAdjustCommand(parent),
                new RecountQuestProgressCommand(quest, _repository), 
                new UpdateUpHierarchyCommand(parent, _repository)
             });
        }

        ///<inheritdoc/>
        public ICommand DeleteQuest(Quest parent, Quest quest)
        {
            return new CompositeCommand(new ICommand[]{
                new DeleteQuestCommand(_repository, parent, quest),
                new IsLeafAdjustCommand(parent),
                new RecountQuestProgressCommand(parent, _repository),
                new UpdateUpHierarchyCommand(parent, _repository)
            });
        }

        ///<inheritdoc/> 
        public ICommand UpdateQuest(Quest quest)
        {
            return new UpdateQuestCommand(_repository, quest);
        }
         
        #endregion
    }
}
