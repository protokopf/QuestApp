using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;

namespace Justus.QuestApp.ModelLayer.Commands.State.Common
{
    /// <summary>
    /// Changes state for all parent of current quest.
    /// </summary>
    public class ChangeStateUpHierarchy : UpHierarchyQuestCommand
    {
        private readonly IQuestTree _questTree;
        private readonly AbstractLayer.Entities.Quest.State _newState;
        private readonly Dictionary<Quest, AbstractLayer.Entities.Quest.State> _questsToStates;

        public ChangeStateUpHierarchy(Quest quest, IQuestTree questTree, AbstractLayer.Entities.Quest.State newState) : base(quest)
        {
            questTree.ThrowIfNull(nameof(questTree));
            _questTree = questTree;
            _newState = newState;
            _questsToStates = new Dictionary<Quest, AbstractLayer.Entities.Quest.State>();
        }

        #region UpHierarchyQuestCommand overriding

        ///<inehritdo cref="UpHierarchyQuestCommand"/>
        protected override bool InnerCommit()
        {
            _questTree.Save();
            _questsToStates.Clear();
            return true;
        }

        ///<inehritdo cref="UpHierarchyQuestCommand"/>
        protected override void ExecuteOnQuest(Quest quest)
        {
            _questsToStates.Add(quest, quest.State);
            quest.State = _newState;
            _questTree.Update(quest);
        }

        ///<inehritdo cref="UpHierarchyQuestCommand"/>
        protected override void UndoOnQuest(Quest quest)
        {
            quest.State = _questsToStates[quest];
            _questsToStates.Remove(quest);
            _questTree.RevertUpdate(quest);
        }

        ///<inehritdo cref="UpHierarchyQuestCommand"/>
        protected override bool ShouldStopTraversing(Quest quest)
        {
            return quest == _questTree.Root;
        }

        #endregion
    }
}
