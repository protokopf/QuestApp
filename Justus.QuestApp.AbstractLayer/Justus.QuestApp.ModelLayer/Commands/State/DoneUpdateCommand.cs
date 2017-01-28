using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;
using QuestState = Justus.QuestApp.AbstractLayer.Entities.Quest.QuestState;

namespace Justus.QuestApp.ModelLayer.Commands.State
{
    /// <summary>
    /// Command which marks quest as done.
    /// </summary>
    public class DoneUpdateCommand : BaseStateUpdateCommand
    {
        private readonly Dictionary<Quest, QuestState> _changedQuests; 

        /// <summary>
        /// Receives quest and repository.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="repository"></param>
        public DoneUpdateCommand(Quest quest, IQuestRepository repository) : base(quest, repository)
        {
            _changedQuests = new Dictionary<Quest, QuestState>();
        }

        #region BaseStateUpdateCommand overriding

        ///<inheritdoc/>
        public override void Execute()
        {
            if (!HasExecuted)
            {
                AssignStateAllParentHierarchyIfSiblingsHasSameState(QuestRef, QuestState.Done);
                HasExecuted = true;
            }
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if (HasExecuted)
            {
                RevertChanges();
                HasExecuted = false;
            }
        }


        #endregion

        #region Private methods

        private void AssignStateAllParentHierarchyIfSiblingsHasSameState(Quest parent, QuestState state)
        {
            while (true)
            {
                if (parent == null)
                {
                    break;
                }
                if (AllQuestsHasState(parent.Children, state))
                {
                    _changedQuests.Add(parent, parent.CurrentState);
                    parent.CurrentState = state;
                    Repository.Update(parent);
                    parent = parent.Parent;
                }
                else
                {
                    break;
                }               
            }
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

        private void RevertChanges()
        {
            foreach(KeyValuePair<Quest, QuestState> item in _changedQuests)
            {
                item.Key.CurrentState = item.Value;
                Repository.RevertUpdate(item.Key);
            }
            _changedQuests.Clear();
        }

        #endregion


    }
}
