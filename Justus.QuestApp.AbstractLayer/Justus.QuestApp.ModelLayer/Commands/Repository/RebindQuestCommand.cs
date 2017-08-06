using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Rebind quest from old parent to new one.
    /// </summary>
    public class RebindQuestCommand : AbstractTreeCommand
    {
        private readonly Quest _oldParent;
        private readonly Quest _questToBind;
        private readonly Quest _newParent;

        /// <summary>
        /// Receives tree, parent and quest.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="questToBind"></param>
        /// <param name="newParent"></param>
        /// <param name="oldParent"></param>
        public RebindQuestCommand(IQuestTree tree, Quest questToBind, Quest newParent, Quest oldParent) :
            base(tree)
        {
            questToBind.ThrowIfNull(nameof(questToBind));
            newParent.ThrowIfNull(nameof(newParent));
            oldParent.ThrowIfNull(nameof(oldParent));
            _questToBind = questToBind;
            _newParent = newParent;
            _oldParent = oldParent;
        }

        #region AbstractTreeCommand overriding

        ///<inheritdoc/>
        protected override bool InnerExecute()
        {
            BreakWithParent(_oldParent, _questToBind);
            ConnectWithParent(_newParent, _questToBind);
            QuestTree.Update(_questToBind);
            return true;
        }

        ///<inheritdoc/>
        protected override bool InnerUndo()
        {
            BreakWithParent(_newParent, _questToBind);
            ConnectWithParent(_oldParent, _questToBind);
            QuestTree.RevertUpdate(_questToBind);
            return true;
        }

        #endregion

        /// <summary>
        /// Breaks connection between parent and child.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        private void BreakWithParent(Quest parent, Quest child)
        {
            parent.Children.Remove(child);
            child.Parent = null;
            child.ParentId = null;
        }

        /// <summary>
        /// Establishes connection between parent and child.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        private void ConnectWithParent(Quest parent, Quest child)
        {
            child.Parent = parent;
            child.ParentId = parent.Id;
            parent.Children.Add(child);
        }
    }
}
