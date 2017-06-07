using System;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Abstracts;

namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Adds quest to parent. If received quest has parent - command breaks relation between given
    /// quest and old parent and create new relations.
    /// </summary>
    public class AddQuestCommand : AbstractRepositoryCommand
    {
        protected readonly Quest Parent;
        protected readonly Quest ChildToAdd;

        /// <summary>
        /// Receives reference to repository, parent and new child.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="childToAdd"></param>
        public AddQuestCommand(IQuestRepository repository, Quest childToAdd) : 
            base(repository)
        {
            if(childToAdd == null)
            {
                throw new ArgumentNullException(nameof(childToAdd));
            }
            Parent = repository.Get(q => q.Id == childToAdd.ParentId);
            ChildToAdd = childToAdd;
        }

        #region AbstractRepositoryCommand overriding

        ///<inheritdoc/>
        protected override bool InnerExecute()
        {
                if (Parent != null)
                {
                    ConnectWithParent(Parent, ChildToAdd);
                }
                Repository.Insert(ChildToAdd);               
                return true;
        }

        ///<inheritdoc/>
        protected override bool InnerUndo()
        {
            if (Parent != null)
            {
                BreakWithParent(Parent, ChildToAdd);
            }
            Repository.RevertInsert(ChildToAdd);
            return true;
        }

        #endregion

        /// <summary>
        /// Breaks connection between parent and child.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
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
            child.ParentId = parent.Id;
            parent.Children.Add(child);
        }
    }
}
