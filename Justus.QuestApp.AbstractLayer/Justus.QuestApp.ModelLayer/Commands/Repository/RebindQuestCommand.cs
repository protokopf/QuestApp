﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Rebind quest from old parent to new one.
    /// </summary>
    public class RebindQuestCommand : AddQuestToParentCommand
    {
        private Quest _oldParent;

        /// <summary>
        /// Receives repository, parent and quest.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="parent"></param>
        /// <param name="questToAdd"></param>
        public RebindQuestCommand(IQuestRepository repository, Quest questToBind, Quest newParent, Quest oldParent) : 
            base(repository, newParent, questToBind)
        {
            if(oldParent == null)
            {
                throw new ArgumentNullException(nameof(oldParent));
            }
            _oldParent = oldParent;
        }

        #region AddQuestToParentCommand overriding

        ///<inheritdoc/>
        public override void Execute()
        {
            if(!HasExecuted)
            {
                BreakWithParent(_oldParent, _toAdd);
                ConnectWithParent(_parent, _toAdd);
                Repository.Update(_toAdd);
                HasExecuted = true;
            }
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if(HasExecuted)
            {
                BreakWithParent(_parent, _toAdd);
                ConnectWithParent(_oldParent, _toAdd);
                Repository.RevertUpdate(_toAdd);
                HasExecuted = false;
            }
        }

        #endregion
    }
}
