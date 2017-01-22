using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// View model for list of done and failed quests.
    /// </summary>
    public class ResultsQuestListVIewModel : QuestListViewModel
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
                return quest.CurrentState == QuestState.Done || quest.CurrentState == QuestState.Failed;
            }
            return true;
        }
    }
}
