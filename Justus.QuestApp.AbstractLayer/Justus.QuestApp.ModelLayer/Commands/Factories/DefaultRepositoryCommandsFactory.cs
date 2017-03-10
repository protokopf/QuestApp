using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Repository;

namespace Justus.QuestApp.ModelLayer.Commands.Factories
{
    /// <summary>
    /// Defalt repository commands factory.
    /// </summary>
    public class DefaultRepositoryCommandsFactory : IRepositoryCommandsFactory
    {
        private readonly IQuestRepository _repository;

        /// <summary>
        /// Receives dependency to IQuestRepository.
        /// </summary>
        /// <param name="repository"></param>
        public DefaultRepositoryCommandsFactory(IQuestRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            _repository = repository;
        }

        #region IRepositoryCommandsFactory implementation

        ///<inheritdoc/>
        public Command AddQuest(Quest quest, Quest parentQuest)
        {
            return new AddQuestCommand(_repository,parentQuest, quest);
        }

        ///<inheritdoc/>
        public Command DeleteQuest(Quest quest)
        {
            return new DeleteQuestCommand(_repository, quest);
        }

        ///<inheritdoc/>
        public Command UpdateQuest(Quest quest)
        {
            return new UpdateQuestCommand(_repository, quest);
        }
         
        #endregion
    }
}
