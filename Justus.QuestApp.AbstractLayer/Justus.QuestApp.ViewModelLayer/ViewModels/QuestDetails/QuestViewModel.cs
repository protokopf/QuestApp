using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails
{
    /// <summary>
    /// View model for holding quest details.
    /// </summary>
    public class QuestViewModel : BaseViewModel, IQuestViewModel
    {
        private readonly Quest _innerQuest;

        /// <summary>
        /// Receives reference to quest model.
        /// </summary>
        /// <param name="questModel"></param>
        public QuestViewModel(Quest questModel)
        {
            if (questModel == null)
            {
                throw new ArgumentNullException(nameof(questModel));
            }
            _innerQuest = questModel;
        }

        #region IQuestViewModel implementation

        ///<inheritdoc cref="IQuestViewModel"/>
        public string Title
        {
            get { return _innerQuest.Title; }
            set { _innerQuest.Title = value.Trim(); }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public string Description
        {
            get { return _innerQuest.Description; }
            set { _innerQuest.Description = value.Trim(); }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public bool IsImportant
        {
            get { return _innerQuest.IsImportant; }
            set { _innerQuest.IsImportant = value; }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public bool UseStartTime { get; set; }

        /// <summary>
        /// Start time.
        /// </summary>
        public DateTime StartTime
        {
            get { return _innerQuest.StartTime; }
            set { _innerQuest.StartTime = value; }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public bool UseDeadline { get; set; }

        ///<inheritdoc cref="IQuestViewModel"/>
        public DateTime Deadline
        {
            get { return _innerQuest.Deadline; }
            set { _innerQuest.Deadline = value; }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public Quest Model => _innerQuest;

        #endregion
    }
}
