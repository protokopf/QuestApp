using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.State.Common;

namespace Justus.QuestApp.ModelLayer.Commands.State
{
    /// <summary>
    /// Marks current quest as idled.
    /// </summary>
    public class CancelQuestCommand : ChangeStateDownHierarchyLoadUnload
    {
        private readonly Dictionary<Quest, double> _questsToProgress;

        public CancelQuestCommand(Quest quest, IQuestTree questTree) :
            base(quest, questTree, AbstractLayer.Entities.Quest.State.Idle)
        {
            _questsToProgress = new Dictionary<Quest, double>();
        }

        #region ChangeStateDownHierarchyLoadUnload overriding

        ///<inheritdoc cref="ChangeStateDownHierarchyLoadUnload"/>
        protected override void ExecuteOnQuest(Quest quest)
        {
            _questsToProgress.Add(quest, quest.Progress);
            quest.Progress = 0;
            base.ExecuteOnQuest(quest);
        }

        ///<inheritdoc cref="ChangeStateDownHierarchyLoadUnload"/>
        protected override void UndoOnQuest(Quest quest)
        {
            quest.Progress = _questsToProgress[quest];
            _questsToProgress.Remove(quest);
            base.UndoOnQuest(quest);
        }

        #endregion
    }
}
