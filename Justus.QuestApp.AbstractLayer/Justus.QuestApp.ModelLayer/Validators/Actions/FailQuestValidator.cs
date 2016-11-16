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
    public class FailQuestValidator : IQuestValidator
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
            if (quest.CurrentState != QuestState.Progress)
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
                if (children[i].CurrentState != QuestState.Failed)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
