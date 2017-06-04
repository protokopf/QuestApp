using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Rebind quest from old parent to new one.
    /// </summary>
    public class RebindQuestCommand : AbstractRepositoryCommand
    {
        private readonly Quest _oldParent;
        private readonly Quest _questToBind;
        private readonly Quest _newParent;

        /// <summary>
        /// Receives repository, parent and quest.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="questToBind"></param>
        /// <param name="newParent"></param>
        /// <param name="oldParent"></param>
        public RebindQuestCommand(IQuestRepository repository, Quest questToBind, Quest newParent, Quest oldParent) :
            base(repository)
        {
            questToBind.ThrowIfNull(nameof(questToBind));
            newParent.ThrowIfNull(nameof(newParent));
            oldParent.ThrowIfNull(nameof(oldParent));
            _questToBind = questToBind;
            _newParent = newParent;
            _oldParent = oldParent;
        }

        #region AddQuestCommand overriding

        ///<inheritdoc/>
        public override void Execute()
        {
            if (!HasExecuted)
            {
                BreakWithParent(_oldParent, _questToBind);
                ConnectWithParent(_newParent, _questToBind);
                Repository.Update(_questToBind);
                HasExecuted = true;
            }
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if (HasExecuted)
            {
                BreakWithParent(_newParent, _questToBind);
                ConnectWithParent(_oldParent, _questToBind);
                Repository.RevertUpdate(_questToBind);
                HasExecuted = false;
            }
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
            child.ParentId = 0;
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
