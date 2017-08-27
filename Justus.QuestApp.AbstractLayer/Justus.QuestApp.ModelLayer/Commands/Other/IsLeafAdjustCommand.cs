using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;

namespace Justus.QuestApp.ModelLayer.Commands.Other
{
    /// <summary>
    /// Command for detecting, whether target quest is leaf.
    /// </summary>
    public class IsLeafAdjustCommand : ThisQuestCommand
    {
        private readonly bool _previousIsLeaf;

        /// <summary>
        /// Receives target quest.
        /// </summary>
        /// <param name="quest"></param>
        public IsLeafAdjustCommand(Quest quest) : base(quest)
        {
            _previousIsLeaf = QuestRef.IsLeaf;
        }

        #region ICommand implementation

        ///<inheritdoc cref="ThisQuestCommand"/>
        protected override void ExecuteOnQuest(Quest quest)
        {
            quest.IsLeaf = quest.Children == null || quest.Children.Count == 0;
        }

        ///<inheritdoc cref="ThisQuestCommand"/>
        protected override void UndoOnQuest(Quest quest)
        {
            quest.IsLeaf = _previousIsLeaf;
        }

        ///<inheritdoc cref="ThisQuestCommand"/>
        public override bool Commit()
        {
            return true;
        }

        #endregion
    }
}
