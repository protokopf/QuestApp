using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using System;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Add quest to repository.
    /// </summary>
    public class AddQuestCommand : AbstractRepositoryCommand
    {
        private Quest _toAdd;

        /// <summary>
        /// Initialize command with repository and quest to add.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="toAdd"></param>
        public AddQuestCommand(IQuestRepository repository, Quest questToAdd) : base(repository)
        {
            if(questToAdd == null)
            {
                throw new ArgumentNullException(nameof(questToAdd));
            }
            _toAdd = questToAdd;
            HasExecuted = false;
        }

        #region AbstractRepositoryCommand overriding

        ///<inheritdoc/>
        public override void Execute()
        {
            if(!HasExecuted)
            {
                Repository.Insert(_toAdd);
                Repository.GetAll().Add(_toAdd);
                HasExecuted = true;
            }
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if(HasExecuted)
            {
                Repository.RevertInsert(_toAdd);
                Repository.GetAll().Remove(_toAdd);
                HasExecuted = false;
            }
        } 

        #endregion
    }
}
