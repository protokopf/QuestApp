using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ModelLayer.Validators.Actions
{
    /// <summary>
    /// Validates, whether quest is ready for start or not.
    /// </summary>
    public class StartQuestValidator : IQuestValidator<StringResponse>
    {
        #region IQuestValidator implementation

        ///<inheritdoc/>
        public StringResponse Validate(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }

            StringResponse result = new StringResponse();
            if (quest.State != State.Idle)
            {
                result.Errors.Add("ERR_QUEST_ACT_WRONG_STATE");
                return result;
            }
            if (quest.Children.Count != 0)
            {
                result.Errors.Add("ERR_QUEST_ACT_HAS_CHILDREN");
                return result;
            }
            return result;
        }

        #endregion
    }
}
