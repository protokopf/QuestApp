using System;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Validators;
using Justus.QuestApp.ModelLayer.Model.QuestManagement.Validators;

namespace Justus.QuestApp.ModelLayer.Model.QuestManagement
{
    /// <summary>
    /// Manage quests in recursive way.
    /// </summary>
    public class RecursiveQuestManager : IQuestManager
    {
        private IQuestValidator _startQuestValidator = null;

        /// <summary>
        /// Default constructor. Initializes inner validators.
        /// </summary>
        public RecursiveQuestManager()
        {
            _startQuestValidator = new StartQuestValidator();
        }

        #region IQuestManager implementation

        ///<inheritdoc/>
        public Response Start(Quest quest)
        {
            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest));
            }

            return InnerStart(quest);            
        }

        ///<inheritdoc/>
        public Response Finish(Quest quest)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public Response Fail(Quest quest)
        {
            throw new NotImplementedException();
        } 

        #endregion

        private Response InnerStart(Quest quest)
        {
            Response result = _startQuestValidator.Validate(quest);
            return result;
        }
    }
}
