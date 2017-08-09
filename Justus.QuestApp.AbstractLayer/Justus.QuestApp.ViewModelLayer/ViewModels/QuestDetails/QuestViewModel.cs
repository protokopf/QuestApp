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
        private bool _useStartTime = true;
        private DateTime? _cachedStartTime;

        private bool _useDeadline = true;
        private DateTime? _cachedDeadline;

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
        public bool UseStartTime
        {
            get { return _useStartTime; }
            set
            {
                if (value && !_useStartTime)
                {
                    if (_innerQuest != null)
                    {
                        _innerQuest.StartTime = _cachedStartTime;
                    }
                    _useStartTime = true;         
                }
                else if(!value && _useStartTime)
                {
                    if (_innerQuest != null)
                    {
                        _innerQuest.StartTime = null;
                    }
                    _useStartTime = false;            
                }
            }
        }

        /// <summary>
        /// Start time.
        /// </summary>
        public DateTime? StartTime
        {
            get
            {
                return UseStartTime ? _innerQuest?.StartTime : _cachedStartTime;
            }
            set
            {
                _cachedStartTime = value;
                if (_innerQuest != null && UseStartTime)
                {
                    _innerQuest.StartTime = value;
                }
            }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public bool UseDeadline
        {
            get
            {
                return _useDeadline;
            }
            set
            {
                if (value && !_useDeadline)
                {
                    if (_innerQuest != null)
                    {
                        _innerQuest.Deadline = _cachedDeadline;
                    }
                    _useDeadline = true;
                }
                else if (!value && _useDeadline)
                {
                    if (_innerQuest != null)
                    {
                        _innerQuest.Deadline = null;
                    }
                    _useDeadline = false;
                }
            }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public DateTime? Deadline
        {
            get
            {
                return UseDeadline ? _innerQuest?.Deadline : _cachedDeadline;
            }
            set
            {
                _cachedDeadline = value;
                if (_innerQuest != null && UseDeadline)
                {
                    _innerQuest.Deadline = value;
                }
            }
        }

        ///<inheritdoc cref="IQuestViewModel"/>
        public Quest Model
        {
            set
            {
                _innerQuest = value;
                _cachedStartTime = _innerQuest?.StartTime;
                _cachedDeadline = _innerQuest?.Deadline;
            }
            get
            {
                return _innerQuest;;
            }
        }

        #endregion
    }
}
