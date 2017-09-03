using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy
{
    /// <summary>
    /// Quest for traversing quest hierarchy and operate on each quest.
    /// </summary>
    public class DownHierarchyQuestCommand : ICommand
    {
        private readonly Quest _target;
        private readonly IQuestCommand _beforeTraverseCommand;
        private readonly IQuestCommand _afterTraverseCommand;

        /// <summary>
        /// Receives quest to traverse.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="beforeTraverseCommand"></param>
        /// <param name="afterTraverseCommand"></param>
        public DownHierarchyQuestCommand(Quest quest, IQuestCommand beforeTraverseCommand, IQuestCommand afterTraverseCommand)
        {
            quest.ThrowIfNull(nameof(quest));
            beforeTraverseCommand.ThrowIfNull(nameof(beforeTraverseCommand));
            afterTraverseCommand.ThrowIfNull(nameof(afterTraverseCommand));
            _target = quest;
            _beforeTraverseCommand = beforeTraverseCommand;
            _afterTraverseCommand = afterTraverseCommand;
        }

        #region ICommand implementation

        ///<inheritdoc cref="ICommand"/>
        public bool Execute()
        {
            TraverseDownHierarhy(_target, _beforeTraverseCommand.Execute, _afterTraverseCommand.Execute);
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool Undo()
        {
            TraverseDownHierarhy(_target, _beforeTraverseCommand.Undo, _afterTraverseCommand.Undo);
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool IsValid()
        {
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool Commit()
        {
            bool beforeCommandResult = _beforeTraverseCommand.Commit();
            bool afterCommandResult = _afterTraverseCommand.Commit();
            return beforeCommandResult && afterCommandResult;
        }

        #endregion

        /// <summary>
        /// Traverses quest hierarchy and calls callbacks.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="beforeTraverseAction"></param>
        /// <param name="afterTraverseAction"></param>
        protected static void TraverseDownHierarhy(Quest quest, Func<Quest, bool> beforeTraverseAction, Func<Quest, bool> afterTraverseAction)
        {
            if (quest == null)
            {
                return;
            }
            if (!beforeTraverseAction.Invoke(quest))
            {
                return;
            }
            List<Quest> children = quest.Children;
            if (children != null && children.Count != 0)
            {
                int length = children.Count;
                for (int i = 0; i < length; ++i)
                {
                    TraverseDownHierarhy(children[i], beforeTraverseAction, afterTraverseAction);
                }
                afterTraverseAction.Invoke(quest);
            }
        }
    }
}
