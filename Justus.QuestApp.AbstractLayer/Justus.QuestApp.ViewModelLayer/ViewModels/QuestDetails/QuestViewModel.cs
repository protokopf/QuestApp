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
    public class QuestViewModel : BaseViewModel
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

        /// <summary>
        /// Return quest model.
        /// </summary>
        public Quest QuestModel => _innerQuest;

        /// <summary>
        /// Id of parent quest.
        /// </summary>
        public int ParentId
        {
            get { return _innerQuest.ParentId; }
            set { _innerQuest.ParentId = value; }
        }

        /// <summary>
        /// Title of current quest.
        /// </summary>
        public string Title
        {
            get { return _innerQuest.Title; }
            set { _innerQuest.Title = value.Trim(); }
        }

        /// <summary>
        /// Description of current quest.
        /// </summary>
        public string Description
        {
            get { return _innerQuest.Description; }
            set { _innerQuest.Description = value.Trim(); }
        }

        /// <summary>
        /// Points, whether current quest important or not.
        /// </summary>
        public bool IsImportant
        {
            get { return _innerQuest.IsImportant; }
            set { _innerQuest.IsImportant = value; }
        }

        /// <summary>
        /// Points, whether start time should be used.
        /// </summary>
        public bool UseStartTime { get; set; }

        /// <summary>
        /// Start time.
        /// </summary>
        public DateTime StartTime
        {
            get { return _innerQuest.StartTime; }
            set { _innerQuest.StartTime = value; }
        }

        /// <summary>
        /// Points, whether deadline should be used.
        /// </summary>
        public bool UseDeadline { get; set; }

        /// <summary>
        /// Deadline.
        /// </summary>
        public DateTime Deadline
        {
            get { return _innerQuest.Deadline; }
            set { _innerQuest.Deadline = value; }
        }
    }
}
