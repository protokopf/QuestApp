using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ModelLayer.Commands.QuestCommands.Wrappers.Logic
{
    /// <summary>
    /// Executes/rollbacks inner command only if given quest matches predicate
    /// </summary>
    public class IfMatchQuestCommand : IQuestCommand
    {
        private readonly IQuestCommand _innerCommand;
        private readonly Predicate<Quest> _predicate;

        public IfMatchQuestCommand(Predicate<Quest> predicate, IQuestCommand innerCommand)
        {
            predicate.ThrowIfNull(nameof(predicate));
            innerCommand.ThrowIfNull(nameof(innerCommand));
            _predicate = predicate;
            _innerCommand = innerCommand;
        }

        public bool Execute(Quest quest)
        {
            return _predicate(quest) && _innerCommand.Execute(quest);
        }

        public bool Undo(Quest quest)
        {
            return _predicate(quest) && _innerCommand.Undo(quest);
        }

        public bool Commit()
        {
            return _innerCommand.Commit();
        }
    }
}
