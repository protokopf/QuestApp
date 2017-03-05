using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;

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
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public Command DeleteQuest(Quest quest)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public Command UpdateQuest(Quest quest)
        {
            throw new NotImplementedException();
        }
         
        #endregion
    }
}
