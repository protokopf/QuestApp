﻿using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Data;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// Base type for view models, that works with quest items.
    /// </summary>
    public class QuestListViewModel : BaseViewModel
    {
        protected IQuestRepository QuestRepository;
        protected ICommandManager CommandManager;

        /// <summary>
        /// Default constructor. Resolves references to quest repository and command manager.
        /// </summary>
        public QuestListViewModel()
        {
            QuestRepository = ServiceLocator.Resolve<IQuestRepository>();
            CommandManager = ServiceLocator.Resolve<ICommandManager>();
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
                if (children == null)
                {
                    return new List<Quest>();
                }
                if (children.Count == 0)
                {
                    return children;
                }
                return FilterQuests(children);
            }
        }

        /// <summary>
        /// Get name of current parent quest.
        /// </summary>
        public string QuestsListTitle => CurrentQuest?.Title;

        #region Protected methods

        protected virtual List<Quest> FilterQuests(List<Quest> quests)
        {
            return quests;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Build full quest name like 'topParent/secondParent/currentQuest'
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        private string BuildFullQuestListTitle(Quest quest)
        {
            Stack<string> titles = new Stack<string>();

            while (quest != null)
            {
                titles.Push(quest.Title);
                quest = quest.Parent;
            }

            StringBuilder builder = new StringBuilder();

            while(titles.Count != 0)
            {
                builder.Append(titles.Pop());
                builder.Append('/');
            }

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        #endregion
    }
}