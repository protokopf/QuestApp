using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;

namespace Justus.QuestApp.ModelLayer.Commands.State.Common
{
    /// <summary>
    /// Changes state of quest and traverse up only if all children of this quest has the same state. 
    /// </summary>
    public class ChangeStateUpHierarchyIfChildrenHaveTheSameState : Abstracts.Hierarchy.UpHierarchyQuestCommand
    {
        private readonly IQuestTree _questTree;
        private readonly AbstractLayer.Entities.Quest.State _newState;
        private readonly Dictionary<Quest, AbstractLayer.Entities.Quest.State> _questsStateDictionary;

        public ChangeStateUpHierarchyIfChildrenHaveTheSameState(Quest quest, IQuestTree questTree, AbstractLayer.Entities.Quest.State newState) : base(quest)
        {
            questTree.ThrowIfNull(nameof(questTree));
            _questTree = questTree;
            _newState = newState;
            _questsStateDictionary = new Dictionary<Quest, AbstractLayer.Entities.Quest.State>();
        }

        #region UpHierarchyQuestCommand overriding

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        /// Overwritten to match requirements.
        protected override bool InnerUndo()
        {
            RevertChanges();
            return true;
        }

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        protected override bool InnerCommit()
        {
            _questTree.Save();
            _questsStateDictionary.Clear();
            return true;
        }

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        protected override void ExecuteOnQuest(Quest quest)
        {
            _questsStateDictionary.Add(quest, quest.State);
            quest.State = _newState;
            _questTree.Update(quest);
        }

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        protected override void UndoOnQuest(Quest quest)
        {
            //Won't be called because InnerUndo is overwritten.
        }

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        protected override bool ShouldStopTraversing(Quest quest)
        {
            return quest == _questTree.Root || !AllQuestsHaveState(quest.Children, _newState);
        }

        #endregion

        private static bool AllQuestsHaveState(List<Quest> quests, AbstractLayer.Entities.Quest.State state)
        {
            if (quests != null)
            {
                int length = quests.Count;
                for (int i = 0; i < length; ++i)
                {
                    if (quests[i].State != state)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void RevertChanges()
        {
            foreach (KeyValuePair<Quest, AbstractLayer.Entities.Quest.State> item in _questsStateDictionary)
            {
                item.Key.State = item.Value;
                _questTree.RevertUpdate(item.Key);
            }
            _questsStateDictionary.Clear();
        }
    }
}
