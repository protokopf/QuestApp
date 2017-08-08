using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ModelLayer.Validators.Actions
{
    /// <summary>
    /// Validates, whether quest can be idled or not.
    /// </summary>
    public class IdleQuestValidator : IQuestValidator<StringResponse>
    {
        #region IQuestValidator implementation

        ///<inheritdoc/>
        public StringResponse Validate(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            StringResponse result = new StringResponse();
            if (quest.State == State.Idle)
            {
                result.Errors.Add("ERR_QUEST_ACT_WRONG_STATE");
            }
            return result;
        }
         
        #endregion
    }
}
