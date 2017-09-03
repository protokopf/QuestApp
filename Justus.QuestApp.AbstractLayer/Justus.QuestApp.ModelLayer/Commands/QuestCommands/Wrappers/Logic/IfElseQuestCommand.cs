using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ModelLayer.Commands.QuestCommands.Wrappers.Logic
{
    /// <summary>
    /// Provides if-else logic for IQuestCommand implementation.
    /// </summary>
    public class IfElseQuestCommand : IQuestCommand
    {
        private readonly IQuestCommand _ifCommand;
        private readonly IQuestCommand _elseCommand;

        public IfElseQuestCommand(IQuestCommand ifCommand, IQuestCommand elseCommand)
        {
            ifCommand.ThrowIfNull(nameof(ifCommand));
            elseCommand.ThrowIfNull(nameof(elseCommand));

            _ifCommand = ifCommand;
            _elseCommand = elseCommand;
        }

        ///<inheritdoc cref="IQuestCommand"/>
        public bool Execute(Quest quest)
        {
            if (_ifCommand.Execute(quest))
            {
                return true;
            }
            return _elseCommand.Execute(quest);
        }

        ///<inheritdoc cref="IQuestCommand"/>
        public bool Undo(Quest quest)
        {
            return _ifCommand.Undo(quest) || _elseCommand.Undo(quest);
        }

        ///<inheritdoc cref="IQuestCommand"/>
        public bool Commit()
        {
            return _ifCommand.Commit() || _elseCommand.Commit();
        }
    }
}
