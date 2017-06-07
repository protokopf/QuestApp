using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts
{
    /// <summary>
    /// Abstract command for repositories commands.
    /// </summary>
    public abstract class AbstractRepositoryCommand : SwitchCommand
    {
        /// <summary>
        /// Reference to repository.
        /// </summary>
        protected IQuestRepository Repository;

        /// <summary>
        /// Initialize repository reference.
        /// </summary>
        /// <param name="repository"></param>
        protected AbstractRepositoryCommand(IQuestRepository repository)
        {
            repository.ThrowIfNull(nameof(repository));
            Repository = repository;
        }
    }
}
