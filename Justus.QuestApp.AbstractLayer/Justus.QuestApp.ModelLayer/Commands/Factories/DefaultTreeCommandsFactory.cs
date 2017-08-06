using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Repository;
using System;

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
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            _repository = repository;
        }

        #region ITreeCommandsFactory implementation

        ///<inheritdoc/>
        public ICommand AddQuest(Quest parent, Quest quest)
        {
            return new AddQuestCommand(_repository, parent, quest);
        }

        ///<inheritdoc/>
        public ICommand DeleteQuest(Quest parent, Quest quest)
        {
            return new DeleteQuestCommand(_repository,parent, quest);
        }

        ///<inheritdoc/> 
        public ICommand UpdateQuest(Quest quest)
        {
            return new UpdateQuestCommand(_repository, quest);
        }
         
        #endregion
    }
}
