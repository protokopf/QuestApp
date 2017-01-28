using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for list of available quests.
    /// </summary>
    public class AvailableQuestListViewModel : QuestListViewModel
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

        private bool FilterItem(Quest quest)
        {
            if (quest.Parent == null)
            {
                return quest.CurrentState == QuestState.Idle;
            }
            return true;
        }
    }
}
