using System;
using Justus.QuestApp.AbstractLayer.Helpers;

namespace Justus.QuestApp.ViewModelLayer.ViewModels.Quest
{
    /// <summary>
    /// View model for creating quest.
    /// </summary>
    public class QuestCreateViewModel : BaseViewModel
    {
        private readonly IDataKeeper _dataKeeper;
        private readonly DateTime _defaultDateTime;

        private bool _useStartTime;
        private bool _useDeadline;

        private DateTime _startTime;
        private DateTime _deadline;

        /// <summary>
        /// Receives reference to IDataKeeper implementation.
        /// </summary>
        /// <param name="dataKeeper"></param>
        public QuestCreateViewModel(IDataKeeper dataKeeper)
        {
            if (dataKeeper == null)
            {
                throw new ArgumentNullException(nameof(dataKeeper));
            }
            _dataKeeper = dataKeeper;
            _defaultDateTime = DateTime.MinValue;
        }

        /// <summary>
        /// Points, whether start time should be used.
        /// </summary>
        public bool UseStartTime
        {
            get
            {
                return _useStartTime;
            }
            set
            {
                _useStartTime = value;
                if (!_useStartTime)
                {
                    StartTime = _defaultDateTime;
                }
            }
        }

        /// <summary>
        /// Start time.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = UseStartTime ? value : _defaultDateTime;
            }
        }

        /// <summary>
        /// Points, whether deadline should be used.
        /// </summary>
        public bool UseDeadline
        {
            get
            {
                return _useDeadline;
            }
            set
            {
                _useDeadline = value;
                if (!_useDeadline)
                {
                    Deadline = _defaultDateTime;
                }
            }
        }

        /// <summary>
        /// Deadline.
        /// </summary>
        public DateTime Deadline
        {
            get
            {
                return _deadline;
            }
            set
            {
                _deadline = UseDeadline ? value : _defaultDateTime;
            }
        }
    }
}
