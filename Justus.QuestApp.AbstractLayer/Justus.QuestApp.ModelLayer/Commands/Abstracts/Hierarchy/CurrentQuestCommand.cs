using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy
{
    /// <summary>
    /// Command for executing quest command on given through ctor quest.
    /// </summary>
    public class CurrentQuestCommand : ICommand
    {
        protected readonly Quest QuestRef;
        protected readonly IQuestCommand QuestCommand;

        /// <summary>
        /// Receives quest to operate on and command, that represents operation on quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="questCommand"></param>
        public CurrentQuestCommand(Quest quest, IQuestCommand questCommand)
        {
            quest.ThrowIfNull(nameof(quest));
            questCommand.ThrowIfNull(nameof(questCommand));
            QuestRef = quest;
            QuestCommand = questCommand;
        }

        #region ICommand implementation

        ///<inheritdoc cref="ICommand"/>
        public virtual bool Execute()
        {
            return QuestCommand.Execute(QuestRef);
        }

        ///<inheritdoc cref="ICommand"/>
        public virtual bool Undo()
        {
            return QuestCommand.Undo(QuestRef);
        }

        ///<inheritdoc cref="ICommand"/>
        public virtual bool IsValid()
        {
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public virtual bool Commit()
        {
            return QuestCommand.Commit();
        }

        #endregion
    }
}
