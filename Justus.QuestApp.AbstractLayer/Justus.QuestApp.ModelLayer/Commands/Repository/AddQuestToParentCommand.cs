﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Adds quest to parent. If received quest has parent - command breaks relation between given
    /// quest and old parent and create new relations.
    /// </summary>
    public class AddQuestToParentCommand : AbstractRepositoryCommand
    {
        protected Quest _parent;
        protected Quest _toAdd;

        /// <summary>
        /// Receives reference to repository, parent and new child.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="parent"></param>
        /// <param name="questToAdd"></param>
        public AddQuestToParentCommand(IQuestRepository repository, Quest parent, Quest questToAdd) : 
            base(repository)
        {
            if(parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }
            if(questToAdd == null)
            {
                throw new ArgumentNullException(nameof(questToAdd));
            }
            _parent = parent;
            _toAdd = questToAdd;
        }

        #region AbstractRepositoryCommand overriding

        ///<inheritdoc/>
        public override void Execute()
        {
            if(!HasExecuted)
            {              
                ConnectWithParent(_parent, _toAdd);
                Repository.Insert(_toAdd);               
                HasExecuted = true;
            }
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if(HasExecuted)
            {
                BreakWithParent(_parent,_toAdd);
                Repository.RevertInsert(_toAdd);
                HasExecuted = false;
            }
        }

        #endregion

        /// <summary>
        /// Breaks connection between parent and child.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="quest"></param>
        protected void BreakWithParent(Quest parent, Quest child)
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
        protected void ConnectWithParent(Quest parent, Quest child)
        {
            child.Parent = parent;
            child.ParentId = parent.ParentId;
            parent.Children.Add(child);
        }
    }
}
