using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.State;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for active quests from list.
    /// </summary>
    public class ActiveQuestListViewModel : QuestListViewModel
    {
        #region QuestListViewModel overriding

        ///<inheritdoc/>
        protected override List<Quest> FilterQuests(List<Quest> quests)
        {
            return quests.Where(FilterEachQuest).ToList();
        }

        #endregion

        /// <summary>
        /// Fails given quest.
        /// </summary>
        /// <param name="quest"></param>
        public void FailQuest(Quest quest)
        {
            LastCommand = StateCommads.FailQuest(quest);
            LastCommand.Execute();
        }

        /// <summary>
        /// Make done given quest.
        /// </summary>
        /// <param name="quest"></param>
        public void DoneQuest(Quest quest)
        {
            LastCommand = StateCommads.DoneQuest(quest);
            LastCommand.Execute();
        }

        /// <summary>
        /// Cancels given quest.
        /// </summary>
        /// <param name="quest"></param>
        public void CancelQuest(Quest quest)
        {
            LastCommand = StateCommads.CancelQuest(quest);
            LastCommand.Execute();
        }

        /// <summary>
        /// Starts given quest.
        /// </summary>
        /// <param name="quest"></param>
        public void StartQuest(Quest quest)
        {
            LastCommand = StateCommads.StartQuest(quest);
            LastCommand.Execute();
        }

        #region Private methods

        private bool FilterEachQuest(Quest quest)
        {
            QuestState state = quest.CurrentState;
            if (quest.Parent == null)
            {
                return state == QuestState.Progress;
            }
            return true;
        }

        #endregion
    }
}
