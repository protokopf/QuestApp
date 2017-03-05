using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.ModelLayer.Commands;
using Justus.QuestApp.ModelLayer.Commands.Repository;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for list of done and failed quests.
    /// </summary>
    public class ResultsQuestListViewModel : QuestListViewModel
    {
        #region QuestListViewModel overriding

        ///<inheritdoc/>
        protected override List<Quest> FilterQuests(List<Quest> quests)
        {
            return
                quests.Where(FilterItem)
                    .ToList();
        }

        #endregion

        /// <summary>
        /// Restarts quest.
        /// </summary>
        /// <param name="quest"></param>
        public void RestartQuest(Quest quest)
        {
            Command cancel = StateCommads.CancelQuest(quest);
            Command start = StateCommads.StartQuest(quest);
            LastCommand = new CompositeCommand(new[] { cancel, start });
            LastCommand.Execute();
            ResetChildren();
        }

        /// <summary>
        /// Deletes quest.
        /// </summary>
        /// <param name="quest"></param>
        public Task DeleteQuest(int position)
        {
            Quest quest = CurrentChildren[position];
            LastCommand = new DeleteQuestCommand(QuestRepository, quest);
            return Task.Run(() =>
            {
                LastCommand.Execute();
                QuestRepository.PushQuests();
            });
        }

        #region Private methods

        private bool FilterItem(Quest quest)
        {
            if (quest.Parent == null)
            {
                return quest.CurrentState == QuestState.Done || quest.CurrentState == QuestState.Failed;
            }
            return true;
        }

        #endregion
    }
}
