using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails.Abstract;

namespace Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails
{
    /// <summary>
    /// View model for holding quest details.
    /// </summary>
    public class QuestViewModel : IQuestViewModel
    {
        private Quest _innerQuest;

        #region IQuestViewModel implementation

        ///<inheritdoc cref="IQuestViewModel"/>
        public string Title
        {
            get
            {
                return _innerQuest?.Title;
            }
            set
            {
                if (_innerQuest != null)
                {
                    _innerQuest.Title = value.Trim();
                }              
            }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public string Description
        {
            get { return _innerQuest?.Description; }
            set
            {
                if (_innerQuest != null)
                {
                    _innerQuest.Description = value.Trim();
                }
            }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public bool IsImportant
        {
            get
            {
                return _innerQuest?.IsImportant ?? default(bool);
            }
            set
            {
                if (_innerQuest != null)
                {
                    _innerQuest.IsImportant = value;
                }       
            }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public bool UseStartTime { get; set; }

        /// <summary>
        /// Start time.
        /// </summary>
        public DateTime? StartTime
        {
            get
            {
                return _innerQuest?.StartTime;
            }
            set
            {
                if (_innerQuest != null)
                {
                    _innerQuest.StartTime = value;
                }
            }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public bool UseDeadline { get; set; }

        ///<inheritdoc cref="IQuestViewModel"/>
        public DateTime? Deadline
        {
            get
            {
                return _innerQuest?.Deadline;
            }
            set
            {
                if (_innerQuest != null)
                {
                    _innerQuest.Deadline = value;
                }
            }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public Quest Model
        {
            set { _innerQuest = value; }
            get { return _innerQuest;;}
        }

        #endregion
    }
}
