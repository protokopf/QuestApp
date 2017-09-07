using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Adds quest to parent. If received quest has parent - command breaks relation between given
    /// quest and old parent and create new relations.
    /// </summary>
    public class AddQuestCommand : AbstractTreeCommand
    {
        protected readonly Quest Parent;
        protected readonly Quest ChildToAdd;

        /// <summary>
        /// Receives reference to tree, parent and new child.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="parent"></param>
        /// <param name="childToAdd"></param>
        public AddQuestCommand(IQuestTree tree, Quest parent, Quest childToAdd) : 
            base(tree)
        {
            parent.ThrowIfNull(nameof(parent));
            childToAdd.ThrowIfNull(nameof(childToAdd));

            Parent = parent;
            ChildToAdd = childToAdd;
        }

        #region AbstractTreeCommand overriding

        ///<inheritdoc/>
        public override bool Execute()
        {
            QuestTree.AddChild(Parent, ChildToAdd);
            return true;
        }

        ///<inheritdoc/>
        public override bool Undo()
        {
            QuestTree.RemoveChild(Parent, ChildToAdd);
            return true;
        }

        #endregion


    }
}
