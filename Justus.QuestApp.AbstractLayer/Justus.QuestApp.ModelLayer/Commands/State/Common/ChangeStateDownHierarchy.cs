using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;

namespace Justus.QuestApp.ModelLayer.Commands.State.Common
{
    /// <summary>
    /// Changes state for all down hierarchy.
    /// </summary>
    public class ChangeStateDownHierarchy : DownHierarchyQuestCommand
    {
        protected readonly IQuestTree QuestTree;
        private readonly AbstractLayer.Entities.Quest.State _newState;
        private readonly Dictionary<Quest, AbstractLayer.Entities.Quest.State> _questsStateDictionary;

        public ChangeStateDownHierarchy(Quest quest, IQuestTree questTree, AbstractLayer.Entities.Quest.State newState) : base(quest)
        {
            questTree.ThrowIfNull(nameof(questTree));
            QuestTree = questTree;
            _newState = newState;
            _questsStateDictionary = new Dictionary<Quest, AbstractLayer.Entities.Quest.State>();
        }

        #region DownHierarchyQuestCommand overriding

        ///<inheritdoc cref="DownHierarchyQuestCommand"/>
        public override bool Commit()
        {
            QuestTree.Save();
            _questsStateDictionary.Clear();
            return true;
        }

        ///<inheritdoc cref="DownHierarchyQuestCommand"/>
        protected override void ExecuteOnQuest(Quest quest)
        {
            _questsStateDictionary.Add(quest, quest.State);
            quest.State = _newState;
            QuestTree.Update(quest);
        }

        ///<inheritdoc cref="DownHierarchyQuestCommand"/>
        protected override void UndoOnQuest(Quest quest)
        {
            quest.State = _questsStateDictionary[quest];
            _questsStateDictionary.Remove(quest);
            QuestTree.RevertUpdate(quest);
        }

        ///<inheritdoc cref="DownHierarchyQuestCommand"/>
        protected override void ExecuteOnQuestAfterTraverse(Quest quest)
        {
            
        }

        ///<inheritdoc cref="DownHierarchyQuestCommand"/>
        protected override void UndoOnQuestAfterTraverse(Quest quest)
        {
            
        }

        #endregion
    }
}
