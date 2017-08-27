using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy
{
    /// <summary>
    /// Quest for traversing quest hierarchy and operate on each quest.
    /// </summary>
    public abstract class DownHierarchyQuestCommand : AbstractQuestCommand
    {
        /// <summary>
        /// Receives quest to traverse.
        /// </summary>
        /// <param name="quest"></param>
        protected DownHierarchyQuestCommand(Quest quest) : base(quest)
        {
        }

        #region AbstractQuestCommand overriding

        ///<inheritdoc cref="AbstractQuestCommand"/>
        public override bool Execute()
        {
            TraverseDownHierarhy(QuestRef, ExecuteOnQuest, ExecuteOnQuestAfterTraverse);
            return true;
        }

        ///<inheritdoc cref="AbstractQuestCommand"/>
        public override bool Undo()
        {
            TraverseDownHierarhy(QuestRef, UndoOnQuest, UndoOnQuestAfterTraverse);
            return true;
        }

        #endregion

        #region Protected abstract methods

        /// <summary>
        /// Called during command execution after traverse quest children. 
        /// </summary>
        /// <param name="quest"></param>
        protected abstract void ExecuteOnQuestAfterTraverse(Quest quest);

        /// <summary>
        /// Called during command rollback after traverse quest children. 
        /// </summary>
        /// <param name="quest"></param>
        protected abstract void UndoOnQuestAfterTraverse(Quest quest);

        #endregion

        /// <summary>
        /// Traverses quest hierarchy and calls callbacks.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="beforeTraverseAction"></param>
        /// <param name="afterTraverseAction"></param>
        protected static void TraverseDownHierarhy(Quest quest, Action<Quest> beforeTraverseAction, Action<Quest> afterTraverseAction)
        {
            if (quest == null)
            {
                return;
            }
            beforeTraverseAction?.Invoke(quest);
            List<Quest> children = quest.Children;
            if (children != null && children.Count != 0)
            {
                int length = children.Count;
                for (int i = 0; i < length; ++i)
                {
                    TraverseDownHierarhy(children[i], beforeTraverseAction, afterTraverseAction);
                }
                afterTraverseAction?.Invoke(quest);
            }
        }
    }
}
