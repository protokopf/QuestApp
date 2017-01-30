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

        public void FailQuest(Quest quest)
        {
            LastCommand = new UpHierarchyStateUpdateCommand(quest, QuestState.Failed, QuestRepository);
            LastCommand.Execute();
        }

        public void DoneQuest(Quest quest)
        {
            LastCommand = new UpHierarchyStateUpdateCommand(quest, QuestState.Done, QuestRepository);
            LastCommand.Execute();
        }

        private bool FilterEachQuest(Quest quest)
        {
            QuestState state = quest.CurrentState;
            if (quest.Parent == null)
            {
                return state == QuestState.Progress;
            }
            return true;
        }
    }
}
