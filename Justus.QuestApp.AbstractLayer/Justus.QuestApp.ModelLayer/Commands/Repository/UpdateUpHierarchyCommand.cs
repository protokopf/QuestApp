using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Updates up hierarchy of quest.
    /// </summary>
    public class UpdateUpHierarchyCommand : UpHierarchyQuestCommand
    {
        private readonly IQuestTree _questTree;

        /// <summary>
        /// Receives the target quest and quest tree.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="questTree"></param>
        public UpdateUpHierarchyCommand(Quest quest, IQuestTree questTree) : base(quest)
        {
            questTree.ThrowIfNull(nameof(questTree));
            _questTree = questTree;
        }

        #region UpHierarchyQuestCommand overriding

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        public override bool Commit()
        {
            _questTree.Save();
            return true;
        }

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        protected override void ExecuteOnQuest(Quest quest)
        {
            _questTree.Update(quest);
        }

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        protected override void UndoOnQuest(Quest quest)
        {
            _questTree.RevertUpdate(quest);
        }

        ///<inheritdoc cref="UpHierarchyQuestCommand"/>
        protected override bool ShouldStopTraversing(Quest quest)
        {
            return quest == _questTree.Root;
        }

        #endregion
    }
}
