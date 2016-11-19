using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ModelLayer.Validators.Actions
{
    /// <summary>
    /// Validates, whether quest can be idled or not.
    /// </summary>
    public class IdleQuestValidator : IQuestValidator
    {
        #region IQuestValidator implementation

        ///<inheritdoc/>
        public Response Validate(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }
            Response result = new Response();
            if (quest.CurrentState == QuestState.Idle)
            {
                result.Errors.Add("ERR_QUEST_ACT_WRONG_STATE");
            }
            return result;
        }
         
        #endregion
    }
}
