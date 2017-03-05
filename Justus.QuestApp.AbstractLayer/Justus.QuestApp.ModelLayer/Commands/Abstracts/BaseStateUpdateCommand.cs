using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts
{
    /// <summary>
    /// Base class for command, which changes state and updates command.
    /// </summary>
    public abstract class BaseStateUpdateCommand : AbstractRepositoryCommand
    {
        /// <summary>
        /// Guess what.
        /// </summary>
        protected Quest QuestRef;

        /// <summary>
        /// Receives quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="repository"></param>
        protected BaseStateUpdateCommand(Quest quest, IQuestRepository repository) : base(repository)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            QuestRef = quest;
        }
    }
}
