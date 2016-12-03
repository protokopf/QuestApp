using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Model;
using System;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Abstract command for repositories commands.
    /// </summary>
    public abstract class RepositoryCommand : Command
    {
        /// <summary>
        /// Reference to repository.
        /// </summary>
        protected IQuestRepository _repository;

        /// <summary>
        /// Initialize repository reference.
        /// </summary>
        /// <param name="repository"></param>
        protected RepositoryCommand(IQuestRepository repository)
        {
            if(repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            _repository = repository;
        }
    }
}
