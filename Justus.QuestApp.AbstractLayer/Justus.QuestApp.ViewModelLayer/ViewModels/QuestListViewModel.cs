using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Model.Composite;
using Justus.QuestApp.ModelLayer.Commands.Repository;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// Base type for view models, that works with quest items.
    /// </summary>
    public class QuestListViewModel : BaseViewModel, IQuestCompositeModel, ICompositeTraversing
    {
        private readonly List<Quest> _emptyList;
        private List<Quest> _currentChildren;

        private bool _shouldResetChildren;

        protected IQuestRepository QuestRepository;
        protected IStateCommandsFactory StateCommads;
        protected IRepositoryCommandsFactory RepositoryCommands;
        protected Command LastCommand;

        /// <summary>
        /// Default constructor. Resolves references to quest repository and command manager.
        /// </summary>
        public QuestListViewModel(
            IQuestRepository repository, 
            IStateCommandsFactory stateCommandsFactory, 
            IRepositoryCommandsFactory repositoryCommandsFactory)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            if (stateCommandsFactory == null)
            {
                throw new ArgumentNullException(nameof(stateCommandsFactory));
            }
            if (repositoryCommandsFactory == null)
            {
                throw new ArgumentNullException(nameof(repositoryCommandsFactory));
            }

            QuestRepository = repository;
            StateCommads = stateCommandsFactory;
            RepositoryCommands = repositoryCommandsFactory;
            _emptyList = new List<Quest>();
            _currentChildren = new List<Quest>();

            _shouldResetChildren = true;
        }

        #region IQuestCompositeModel implementation

        ///<inheritdoc/>
        public Quest Root { get; set; }

        ///<inheritdoc/>
        public List<Quest> Leaves
        {
            get
            {
                if (_shouldResetChildren)
                {
                    _shouldResetChildren = false;

                    List<Quest> children = InRoot ? QuestRepository.GetAll() : Root.Children;
                    
                    if (children == null || children.Count == 0)
                    {
                        return _currentChildren = _emptyList;
                    }
                    
                    return _currentChildren = FilterQuests(children);
                }
                return _currentChildren;
            }
        }

        #endregion

        #region ICompositeTraversing implementation

        /// <summary>
        /// Traverse to root of current quest hierarchy.
        /// </summary>
        public bool TraverseToRoot()
        {
            bool atLeastOneTraverse = false;
            while (!InRoot)
            {
                Root = Root.Parent;
                atLeastOneTraverse = true;
            }
            if (atLeastOneTraverse)
            {
                ResetChildren();
            }
            return atLeastOneTraverse;
        }

        /// <summary>
        /// Traverse to parent of current quest.
        /// </summary>
        public bool TraverseToParent()
        {
            if (!InRoot)
            {
                Root = Root.Parent;
                ResetChildren();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Traverse to 'childPosition' child of current quest.
        /// </summary>
        /// <param name="leafNumber"></param>
        public bool TraverseToLeaf(int leafNumber)
        {
            Root = Leaves[leafNumber];
            ResetChildren();
            return true;
        }

        #endregion


        /// <summary>
        /// Get name of current parent quest.
        /// </summary>
        public string QuestsListTitle => Root?.Title;

        /// <summary>
        /// Points, whether current quest hierarchy in root.
        /// </summary>
        public bool InRoot => Root == null;

        /// <summary>
        /// Undo last made command.
        /// </summary>
        public void UndoLastCommand()
        {
            if (LastCommand != null)
            {
                LastCommand.Undo();
                LastCommand = null;
            }
        }

        /// <summary>
        /// Makes view model reset children within next call Leaves property.
        /// </summary>
        public void ResetChildren()
        {
            _shouldResetChildren = true;
        }

        /// <summary>
        /// Deletes quest.
        /// </summary>
        /// <param name="position"></param>
        public Task DeleteQuest(int position)
        {
            Quest quest = Leaves[position];
            LastCommand = RepositoryCommands.DeleteQuest(quest);
            return Task.Run(() =>
            {
                LastCommand.Execute();
                QuestRepository.Save();
            });
        }

        #region Protected methods

        /// <summary>
        /// Filters quests.
        /// </summary>
        /// <param name="quests"></param>
        /// <returns></returns>
        protected virtual List<Quest> FilterQuests(List<Quest> quests)
        {
            return quests;
        }

        #endregion
    }
}
