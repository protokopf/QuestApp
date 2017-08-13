using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Commands.State.Common
{
    /// <summary>
    /// Changes state only for current quest.
    /// </summary>
    public class ChangeStateCurrent : Abstracts.Hierarchy.ThisQuestCommand
    {
        private readonly IQuestTree _questTree;
        private readonly AbstractLayer.Entities.Quest.State _newState;
        private readonly AbstractLayer.Entities.Quest.State _oldState;

        public ChangeStateCurrent(Quest quest, IQuestTree questTree, AbstractLayer.Entities.Quest.State newState) : base(quest)
        {
            questTree.ThrowIfNull(nameof(questTree));
            _questTree = questTree;
            _newState = newState;
            _oldState = QuestRef.State;
        }

        #region ThisQuestCommand overriding

        ///<inheritdoc cref="Abstracts.Hierarchy.ThisQuestCommand"/>
        protected override bool InnerCommit()
        {
            _questTree.Save();
            return true;
        }

        ///<inheritdoc cref="Abstracts.Hierarchy.ThisQuestCommand"/>
        protected override void ExecuteOnQuest(Quest quest)
        {
            quest.State = _newState;
            _questTree.Update(quest);
        }

        ///<inheritdoc cref="Abstracts.Hierarchy.ThisQuestCommand"/>
        protected override void UndoOnQuest(Quest quest)
        {
            quest.State = _oldState;
            _questTree.RevertUpdate(quest);
        }

        #endregion
    }
}
