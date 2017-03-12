﻿using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.ModelLayer.Commands.Repository;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// Base type for view models, that works with quest items.
    /// </summary>
    public class QuestListViewModel : BaseViewModel
    {
        private readonly List<Quest> _emptyList;
        private List<Quest> _currentChildren;

        private bool _shouldResetChildren;
        private bool _areQuestsPulled;

        protected IQuestRepository QuestRepository;
        protected IQuestProgressCounter ProgressCounter;
        protected IStateCommandsFactory StateCommads;
        protected IRepositoryCommandsFactory RepositoryCommands;
        protected Command LastCommand;
        

        /// <summary>
        /// Default constructor. Resolves references to quest repository and command manager.
        /// </summary>
        public QuestListViewModel()
        {
            QuestRepository = ServiceLocator.Resolve<IQuestRepository>();
            ProgressCounter = ServiceLocator.Resolve<IQuestProgressCounter>();
            StateCommads = ServiceLocator.Resolve<IStateCommandsFactory>();
            RepositoryCommands = ServiceLocator.Resolve<IRepositoryCommandsFactory>();
            _emptyList = new List<Quest>();
            _currentChildren = new List<Quest>();

            _shouldResetChildren = true;
            _areQuestsPulled = false;
        }

        /// <summary>
        /// Parent quest of current list.
        /// </summary>
        public Quest CurrentQuest { get; set; }

        /// <summary>
        /// Returns children of current quest. If quest is null - returns all top level quests from repository.
        /// </summary>
        public List<Quest> CurrentChildren
        {
            get
            {
                if (_shouldResetChildren)
                {
                    _shouldResetChildren = false;

                    List<Quest> children = InRoot ? QuestRepository.GetAll() : CurrentQuest.Children;
                    
                    if (children == null || children.Count == 0)
                    {
                        return _currentChildren = _emptyList;
                    }
                    
                    return _currentChildren = FilterQuests(children);
                }
                return _currentChildren;
            }
        }

        /// <summary>
        /// Get name of current parent quest.
        /// </summary>
        public string QuestsListTitle => CurrentQuest?.Title;

        /// <summary>
        /// Points, whether current quest hierarchy in root.
        /// </summary>
        public bool InRoot => CurrentQuest == null;

        /// <summary>
        /// Count progress of quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public int CountProgress(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            ProgressValue value = ProgressCounter.CountProgress(quest);
            int result = (int)(value.Current / (double)value.Total * 100);
            return result;
        }

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
        /// Push all quests in async way.
        /// </summary>
        public async Task PushQuests()
        {
            IsBusy = true;
            await Task.Run(() => QuestRepository.PushQuests());
            IsBusy = false;
        }

        /// <summary>
        /// Pull all changes in async way.
        /// </summary>
        public async Task PullQuests()
        {
            _areQuestsPulled = false;
            IsBusy = true;
            await Task.Run(() => QuestRepository.PullQuests());
            IsBusy = false;
            ResetChildren();
        }

        /// <summary>
        /// Points, whether quests have been pulled.
        /// </summary>
        /// <returns></returns>
        public bool AreQuestsPulled()
        {
            return _areQuestsPulled;
        }

        /// <summary>
        /// Traverse to parent of current quest.
        /// </summary>
        public void TraverseToParent()
        {
            if (!InRoot)
            {
                CurrentQuest = CurrentQuest.Parent;
                ResetChildren();
            }
        }

        /// <summary>
        /// Traverse to 'childPosition' child of current quest.
        /// </summary>
        /// <param name="childPosition"></param>
        public void TraverseToChild(int childPosition)
        {
            CurrentQuest = CurrentChildren[childPosition];
            ResetChildren();            
        }

        /// <summary>
        /// Traverse to root of current quest hierarchy.
        /// </summary>
        public void TraverseToRoot()
        {
            bool atLeastOneTraverse = false;
            while (!InRoot)
            {
                CurrentQuest = CurrentQuest.Parent;
                atLeastOneTraverse = true;
            }
            if (atLeastOneTraverse)
            {
                ResetChildren();
            }
        }

        /// <summary>
        /// Makes view model reset children within next call CurrentChildren property.
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
            Quest quest = CurrentChildren[position];
            LastCommand = RepositoryCommands.DeleteQuest(quest);
            return Task.Run(() =>
            {
                LastCommand.Execute();
                QuestRepository.PushQuests();
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
