using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;

namespace Justus.QuestApp.ModelLayer.Commands.State.Common
{
    /// <summary>
    /// Loading quests during traversing and unload them during undo/commit.
    /// </summary>
    public class ChangeStateDownHierarchyLoadUnload : ChangeStateDownHierarchy
    {
        public ChangeStateDownHierarchyLoadUnload(Quest quest, IQuestTree questTree, AbstractLayer.Entities.Quest.State newState) : base(quest, questTree, newState)
        {
        }

        #region ChangeStateDownHierarchy overriding

        ///<inheritdoc cref="ChangeStateDownHierarchy"/>
        protected override void ExecuteOnQuest(Quest quest)
        {
            base.ExecuteOnQuest(quest);
            QuestTree.LoadChildren(quest);
        }

        ///<inheritdoc cref="ChangeStateDownHierarchy"/>
        protected override void UndoOnQuestAfterTraverse(Quest quest)
        {
            base.UndoOnQuestAfterTraverse(quest);
            QuestTree.UnloadChildren(quest);
        }

        ///<inheritdoc cref="ChangeStateDownHierarchy"/>
        public override bool Commit()
        {
            TraverseDownHierarhy(QuestRef, null, QuestTree.UnloadChildren);
            return base.Commit();
        }

        #endregion
    }
}
