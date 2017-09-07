using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Validators;

namespace Justus.QuestApp.ModelLayer.Validators.Actions
{
    /// <summary>
    /// Type, that validates, whether quest can be finished or not.
    /// </summary>
    public class FinishQuestVaidator : IQuestValidator<StringResponse>
    {
        #region IQuestValidator implementation

        ///<inheritdoc/>
        public StringResponse Validate(Quest quest)
        {
            quest.ThrowIfNull(nameof(quest));
            StringResponse result = new StringResponse();
            if (quest.State != State.Progress)
            {
                result.Errors.Add("ERR_QUEST_ACT_WRONG_STATE");
                return result;
            }
            if (AreChildrenDone(quest.Children) == false)
            {
                result.Errors.Add("ERR_QUEST_ACT_CHILDREN_NOT_SAME_STATE");
            }
            return result;
        }

        #endregion

        private bool AreChildrenDone(List<Quest> children)
        {
            int length = children.Count;
            for (int i = 0; i < length; ++i)
            {
                if (children[i].State != State.Done)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
