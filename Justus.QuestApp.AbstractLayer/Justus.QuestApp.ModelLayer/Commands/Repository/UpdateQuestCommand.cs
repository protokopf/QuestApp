using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

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
        public override void Execute()
        {
            if(!_hasExecuted)
            {
                _repository.Update(_toUpdate);
                _hasExecuted = true;
            }
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if (_hasExecuted)
            {
                _repository.RevertUpdate(_toUpdate);
                _hasExecuted = false;
            }
        } 

        #endregion
    }
}
