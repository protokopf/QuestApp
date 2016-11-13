using System;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Validators;
using Justus.QuestApp.ModelLayer.Model.QuestManagement.Validators;

namespace Justus.QuestApp.ModelLayer.Model.QuestManagement
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
            quest.CurrentState = QuestState.Done;
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
            AssignStateAllChildHierarchy(quest, QuestState.Ready);
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
    }
}
