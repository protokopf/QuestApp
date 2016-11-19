using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ModelLayer.Model
{
    /// <summary>
    /// Manage quests in recursive way.
    /// </summary>
    public class RecursiveQuestActionManager : IQuestActionManager
    {

        #region IQuestActionManager implementation

        ///<inheritdoc/>
        public void Start(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            AssignStateAllParentHierarchy(quest,QuestState.Progress);
        }

        ///<inheritdoc/>
        public void Finish(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            AssignStateAllParentHierarchyIfSiblingsHasSameState(quest, QuestState.Done);
        }

        ///<inheritdoc/>
        public void Fail(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            quest.CurrentState = QuestState.Failed;
        }

        ///<inheritdoc/>
        public void Idle(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            AssignStateAllChildHierarchy(quest, QuestState.Idle);
        }

        #endregion

        private void AssignStateAllParentHierarchy(Quest quest, QuestState state)
        {
            quest.CurrentState = state;
            Quest parent = quest.Parent;
            while (parent != null)
            {
                parent.CurrentState = state;
                parent = parent.Parent;
            }           
        }

        private void AssignStateAllChildHierarchy(Quest quest, QuestState state)
        {
            quest.CurrentState = state;
            int length = quest.Children.Count;
            for (int i = 0; i < length; ++i)
            {
                AssignStateAllChildHierarchy(quest.Children[i], state);
            }
        }

        private void AssignStateAllParentHierarchyIfSiblingsHasSameState(Quest parent, QuestState state)
        {
            if (parent == null)
            {
                return;
            }
            if (AllQuestsHasState(parent.Children, state))
            {
                parent.CurrentState = state;
            }
            AssignStateAllParentHierarchyIfSiblingsHasSameState(parent.Parent, state);
        }

        private bool AllQuestsHasState(List<Quest> quests, QuestState state)
        {
            int length = quests.Count;
            for (int i = 0; i < length; ++i)
            {
                if (quests[i].CurrentState != state)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
