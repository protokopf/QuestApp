﻿using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using System;

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
            _hasExecuted = false;
        }

        #region AbstractRepositoryCommand overriding

        ///<inheritdoc/>
        public override void Execute()
        {
            if(!_hasExecuted)
            {
                _repository.Insert(_toAdd);
                _repository.GetAll().Add(_toAdd);
                _hasExecuted = true;
            }
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if(_hasExecuted)
            {
                _repository.RevertInsert(_toAdd);
                _repository.GetAll().Remove(_toAdd);
                _hasExecuted = false;
            }
        } 

        #endregion
    }
}