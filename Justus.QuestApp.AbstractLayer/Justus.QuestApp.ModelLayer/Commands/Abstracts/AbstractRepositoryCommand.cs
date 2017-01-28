using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts
{
    /// <summary>
    /// Abstract command for repositories commands.
    /// </summary>
    public abstract class AbstractRepositoryCommand : Command
    {
        /// <summary>
        /// Reference to repository.
        /// </summary>
        protected IQuestRepository Repository;

        /// <summary>
        /// Points, whether command has been executed.
        /// </summary>
        protected bool HasExecuted;

        /// <summary>
        /// Initialize repository reference.
        /// </summary>
        /// <param name="repository"></param>
        protected AbstractRepositoryCommand(IQuestRepository repository)
        {
            if(repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            Repository = repository;
            HasExecuted = false;
        }
    }
}
