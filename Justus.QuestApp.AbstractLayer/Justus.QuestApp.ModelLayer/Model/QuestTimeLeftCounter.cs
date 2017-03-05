using Justus.QuestApp.AbstractLayer.Model;
using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ModelLayer.Model
{
    /// <summary>
    /// Simple quest time left counter, that counts time from now.
    /// </summary>
    public class QuestTimeLeftCounter : IQuestTimeLeftCounter
    {
        #region IQuestTimeLeftCounter implementation

        ///<inheritdoc/>
        public TimeSpan CountTimeLeft(Quest quest)
        {
            if(quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            return quest.Deadline.Subtract(DateTime.Now);
        }
         
        #endregion
    }
}
