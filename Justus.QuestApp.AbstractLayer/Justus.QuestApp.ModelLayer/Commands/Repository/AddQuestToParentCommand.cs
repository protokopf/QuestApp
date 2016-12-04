﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Adds quest to parent. If received quest has parent - command breaks relation between given
    /// quest and old parent and create new relations.
    /// </summary>
    public class AddQuestToParentCommand : RepositoryCommand
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

        #region RepositoryCommand overriding

        ///<inheritdoc/>
        public override void Execute()
        {
            if(!_hasExecuted)
            {              
                ConnectWithParent(_parent, _toAdd);
                _repository.Insert(_toAdd);               
                _hasExecuted = true;
            }
        }

        ///<inheritdoc/>
        public override void Undo()
        {
            if(_hasExecuted)
            {
                BreakWithParent(_parent,_toAdd);
                _repository.RevertInsert(_toAdd);
                _hasExecuted = false;
            }
        }

        #endregion

        private bool FindQuestRecursive(List<Quest> quests, Quest quest)
        {
            if (quests == null || quests.Count == 0)
            {
                return false;
            }
            if (quests.Find(q => q == quest) != null)
            {
                return true;
            }
            foreach (Quest q in quests)
            {
                if (FindQuestRecursive(quest.Children, quest))
                {
                    return true;
                }
            }
            return false;
        }

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
