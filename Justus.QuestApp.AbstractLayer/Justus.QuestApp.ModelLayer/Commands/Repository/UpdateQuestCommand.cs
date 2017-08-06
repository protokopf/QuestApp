using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Updates quest.
    /// </summary>
    public class UpdateQuestCommand : AbstractTreeCommand
    {
        private Quest _toUpdate;

        /// <summary>
        /// Receives tree and reference to quest to update.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="questToUpdate"></param>
        public UpdateQuestCommand(IQuestTree tree, Quest questToUpdate) : base(tree)
        {
            if(questToUpdate == null)
            {
                throw new ArgumentNullException(nameof(questToUpdate));
            }
            _toUpdate = questToUpdate;
        }

        #region AbstractTreeCommand overriding

        ///<inheritdoc/>
        protected override bool InnerExecute()
        {
            QuestTree.Update(_toUpdate);
            return true;
        }

        ///<inheritdoc/>
        protected override bool InnerUndo()
        {
            QuestTree.RevertUpdate(_toUpdate);
            return true;
        } 

        #endregion
    }
}
