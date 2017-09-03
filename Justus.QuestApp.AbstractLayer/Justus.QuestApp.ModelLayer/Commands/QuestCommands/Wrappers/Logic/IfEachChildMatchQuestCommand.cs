using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ModelLayer.Commands.QuestCommands.Wrappers.Logic
{
    class IfEachChildMatchQuestCommand : IQuestCommand
    {
        private readonly Predicate<Quest> _predicate;
        private readonly IQuestCommand _inner;

        public IfEachChildMatchQuestCommand(Predicate<Quest> predicate, IQuestCommand innerCommand)
        {
            predicate.ThrowIfNull(nameof(predicate));
            innerCommand.ThrowIfNull(nameof(innerCommand));
            _predicate = predicate;
            _inner = innerCommand;
        }

        public bool Execute(Quest quest)
        {
            if (IsEachChildMatch(quest.Children, _predicate))
            {
                return _inner.Execute(quest);
            }
            return false;
        }

        public bool Undo(Quest quest)
        {
            if (IsEachChildMatch(quest.Children, _predicate))
            {
                return _inner.Undo(quest);
            }
            return false;
        }

        public bool Commit()
        {
            return _inner.Commit();
        }

        private bool IsEachChildMatch(List<Quest> children, Predicate<Quest> predicate)
        {
            if (children == null)
            {
                return true;
            }
            int length = children.Count;
            for (int i = 0; i < length; ++i)
            {
                if (!predicate(children[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
