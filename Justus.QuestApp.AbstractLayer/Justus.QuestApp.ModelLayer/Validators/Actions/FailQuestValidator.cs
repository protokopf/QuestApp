using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ModelLayer.Validators.Actions
{
    /// <summary>
    /// Validates, whether quest can be failed or not.
    /// </summary>
    public class FailQuestValidator : IQuestValidator<StringResponse>
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
            if (quest.State != State.Progress)
            {
                result.Errors.Add("ERR_QUEST_ACT_WRONG_STATE");
                return result;
            }
            if (AreChildrenFailed(quest.Children) == false)
            {
                result.Errors.Add("ERR_QUEST_ACT_CHILDREN_NOT_SAME_STATE");
            }
            return result;
        }

        #endregion

        private bool AreChildrenFailed(List<Quest> children)
        {
            int length = children.Count;
            for (int i = 0; i < length; ++i)
            {
                if (children[i].State != State.Failed)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
