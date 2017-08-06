using System;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for active quests from list.
    /// </summary>
    public class ActiveQuestListViewModel : QuestListViewModel
    {
        public ActiveQuestListViewModel(IQuestListModel questListModel,
            IStateCommandsFactory stateCommandsFactory,
            ITreeCommandsFactory treeCommandsFactory) : 
            base(questListModel, stateCommandsFactory,treeCommandsFactory)
        {

        }


        /// <summary>
        /// Points, whether all quests are done or not.
        /// </summary>
        /// <returns></returns>
        public bool IsRootDone()
        {
            if (InTopRoot)
            {
                return false;
            }
            return QuestListModel.Parent.State == State.Done;
        }

        /// <summary>
        /// Count progress of quest.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public int CountProgress(int position)
        {
            Quest quest = GetQuestByPosition(position);
            if (quest == null)
            {
                return -1;
            }
            quest.ThrowIfNull(nameof(quest));
            double progress = quest.Progress;
            int result = (int) Math.Floor(progress * 100);
            return result > 100 ? 100 : result;
        }

        /// <summary>
        /// Fails given quest.
        /// </summary>
        /// <param name="position"></param>
        public Task FailQuest(int position)
        {
            Quest quest = GetQuestByPosition(position);
            return quest == null ? null : RunCommand(StateCommads.FailQuest(quest));
        }

        /// <summary>
        /// Make done given quest.
        /// </summary>
        /// <param name="position"></param>
        public Task DoneQuest(int position)
        {
            Quest quest = GetQuestByPosition(position);
            return quest == null ? null : RunCommand(StateCommads.DoneQuest(quest));
        }

        /// <summary>
        /// Cancels given quest.
        /// </summary>
        /// <param name="position"></param>
        public Task CancelQuest(int position)
        {
            Quest quest = GetQuestByPosition(position);
            return quest == null ? null : RunCommand(StateCommads.CancelQuest(quest));
        }
    }
}
