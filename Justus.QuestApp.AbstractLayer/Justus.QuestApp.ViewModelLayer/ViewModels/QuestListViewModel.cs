using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// Base type for view models, that works with quest items.
    /// </summary>
    public class QuestListViewModel : BaseViewModel
    {
        private readonly List<Quest> _emptyList; 
        protected IQuestRepository QuestRepository;
        protected IQuestProgressCounter ProgressCounter;
        protected IStateCommandsFactory StateCommads;
        protected Command LastCommand;

        /// <summary>
        /// Default constructor. Resolves references to quest repository and command manager.
        /// </summary>
        public QuestListViewModel()
        {
            QuestRepository = ServiceLocator.Resolve<IQuestRepository>();
            ProgressCounter = ServiceLocator.Resolve<IQuestProgressCounter>();
            StateCommads = ServiceLocator.Resolve<IStateCommandsFactory>();
            _emptyList = new List<Quest>();
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
                List<Quest> children = CurrentQuest == null ? QuestRepository.GetAll() : CurrentQuest.Children;
                if (children == null || children.Count == 0)
                {
                    return _emptyList;
                }
                return FilterQuests(children);
            }
        }

        /// <summary>
        /// Get name of current parent quest.
        /// </summary>
        public string QuestsListTitle => CurrentQuest?.Title;

        /// <summary>
        /// Count progress of quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public int CountProgress(Quest quest)
        {
            ProgressValue value = ProgressCounter.CountProgress(quest);
            int result = (int) (((double) value.Current/(double) value.Total)*100);
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
            IsBusy = true;
            await Task.Run(() => QuestRepository.PullQuests());
            IsBusy = false;
        }

        #region Protected methods

        protected virtual List<Quest> FilterQuests(List<Quest> quests)
        {
            return quests;
        }

        #endregion

        #region Private methods
        #endregion
    }
}
