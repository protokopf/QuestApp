using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Rebind quest from old parent to new one.
    /// </summary>
    public class RebindQuestCommand : AddQuestCommand
    {
        private readonly Quest _oldParent;

        /// <summary>
        /// Receives repository, parent and quest.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="questToBind"></param>
        /// <param name="newParent"></param>
        /// <param name="oldParent"></param>
        public RebindQuestCommand(IQuestRepository repository, Quest questToBind, Quest newParent, Quest oldParent) :
            base(repository, newParent, questToBind)
        {
            if (newParent == null)
            {
                throw new ArgumentNullException(nameof(newParent));
            }
            if (oldParent == null)
            {
                throw new ArgumentNullException(nameof(oldParent));
            }
            _oldParent = oldParent;
        }

        #region AddQuestCommand overriding

        ///<inheritdoc/>
        public override void Execute()
        {
            if (!HasExecuted)
            {
                BreakWithParent(_oldParent, ChildToAdd);
                ConnectWithParent(Parent, ChildToAdd);
                Repository.Update(ChildToAdd);
                HasExecuted = true;
            }
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if (HasExecuted)
            {
                BreakWithParent(Parent, ChildToAdd);
                ConnectWithParent(_oldParent, ChildToAdd);
                Repository.RevertUpdate(ChildToAdd);
                HasExecuted = false;
            }
        }

        #endregion
    }
}
