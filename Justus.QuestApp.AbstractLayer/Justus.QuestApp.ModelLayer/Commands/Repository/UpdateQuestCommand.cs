using System;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Updates quest.
    /// </summary>
    public class UpdateQuestCommand : AbstractRepositoryCommand
    {
        private Quest _toUpdate;

        /// <summary>
        /// Receives repository and reference to quest to update.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="questToUpdate"></param>
        public UpdateQuestCommand(IQuestRepository repository, Quest questToUpdate) : base(repository)
        {
            if(questToUpdate == null)
            {
                throw new ArgumentNullException(nameof(questToUpdate));
            }
            _toUpdate = questToUpdate;
        }

        #region AbstractRepositoryCommand overriding

        ///<inheritdoc/>
        protected override bool InnerExecute()
        {
            Repository.Update(_toUpdate);
            return true;
        }

        ///<inheritdoc/>
        protected override bool InnerUndo()
        {
            Repository.RevertUpdate(_toUpdate);
            return true;
        } 

        #endregion
    }
}
