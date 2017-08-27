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
        private readonly AbstractLayer.Entities.Quest.State _newState;
        private readonly AbstractLayer.Entities.Quest.State _oldState;

        public ChangeStateCurrent(Quest quest, IQuestTree questTree, AbstractLayer.Entities.Quest.State newState) : base(quest)
        {
            questTree.ThrowIfNull(nameof(questTree));
            _newState = newState;
            _oldState = QuestRef.State;
        }

        #region ThisQuestCommand overriding


        ///<inheritdoc cref="Abstracts.Hierarchy.ThisQuestCommand"/>
        protected override void ExecuteOnQuest(Quest quest)
        {
            quest.State = _newState;
        }

        ///<inheritdoc cref="Abstracts.Hierarchy.ThisQuestCommand"/>
        protected override void UndoOnQuest(Quest quest)
        {
            quest.State = _oldState;
        }

        ///<inheritdoc cref="Abstracts.Hierarchy.ThisQuestCommand"/>
        public override bool Commit()
        {
            return true;
        }

        #endregion
    }
}
