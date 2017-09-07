using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Rebind quest from old parent to new one.
    /// </summary>
    public class RebindQuestCommand : ICommand
    {
        private readonly Quest _oldParent;
        private readonly Quest _questToBind;
        private readonly Quest _newParent;

        /// <summary>
        /// Receives tree, parent and quest.
        /// </summary>
        /// <param name="questToBind"></param>
        /// <param name="newParent"></param>
        /// <param name="oldParent"></param>
        public RebindQuestCommand(Quest questToBind, Quest newParent, Quest oldParent)
        {
            questToBind.ThrowIfNull(nameof(questToBind));
            newParent.ThrowIfNull(nameof(newParent));
            oldParent.ThrowIfNull(nameof(oldParent));
            _questToBind = questToBind;
            _newParent = newParent;
            _oldParent = oldParent;
        }

        #region ICommand overriding

        ///<inheritdoc cref="ICommand"/>
        public bool Execute()
        {
            BreakWithParent(_oldParent, _questToBind);
            ConnectWithParent(_newParent, _questToBind);
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool Undo()
        {
            BreakWithParent(_newParent, _questToBind);
            ConnectWithParent(_oldParent, _questToBind);
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool Commit()
        {
            return true;
        }

        ///<inheritdoc cref="ICommand"/>
        public bool IsValid()
        {
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
