using System;
using Android.OS;
using Justus.QuestApp.View.Droid.Abstract.EntityStateHandlers;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails.Abstract;

namespace Justus.QuestApp.View.Droid.EntityStateHandlers.VIewModels
{
    /// <summary>
    /// Handles state of QuestCreateViewModel
    /// </summary>
    public class QuestViewModelStateHandler : IEntityStateHandler<IQuestViewModel>
    {
        private const string IsImportantKey = "QuestCreateViewModel.IsImportant";
        private const string UseDeadlineKey = "QuestCreateViewModel.UseDeadline";
        private const string UseStartTimeKey = "QuestCreateViewModel.UseStartTime";
        private const string DeadlineKey = "QuestCreateViewModel.Deadline";
        private const string StartTimeKey = "QuestCreateViewModel.StartTime";

        private readonly IEntityStateHandler<DateTime?> _dateTimeStateHandler;

        /// <summary>
        /// Receives date handler as dependency.
        /// </summary>
        /// <param name="dateTimeStateHandler"></param>
        public QuestViewModelStateHandler(IEntityStateHandler<DateTime?> dateTimeStateHandler)
        {
            if (dateTimeStateHandler == null)
            {
                throw new ArgumentNullException(nameof(dateTimeStateHandler));
            }
            _dateTimeStateHandler = dateTimeStateHandler;
        }

        #region IEntityStateHandler<QuestCreateViewModel> implementation

        ///<inheritdoc/>
        public bool Save(string key, IQuestViewModel entity, Bundle bundle)
        {
            if (bundle != null && !string.IsNullOrWhiteSpace(key) && entity != null)
            {
                Bundle viewModelBundle = CreateViewModelBundle(entity);
                bundle.PutBundle(key, viewModelBundle);
                return true;
            }
            return false;
        }

        ///<inheritdoc/>
        public bool Extract(string key, Bundle bundle, ref IQuestViewModel entity)
        {
            if (bundle != null && !string.IsNullOrWhiteSpace(key) && entity != null)
            {
                Bundle viewModelBundle = bundle.GetBundle(key);
                if (viewModelBundle != null)
                {
                    FillViewModelWithBundle(viewModelBundle, entity);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Private methods

        private Bundle CreateViewModelBundle(IQuestViewModel viewModel)
        {
            Bundle viewModelBundle = new Bundle();

            viewModelBundle.PutBoolean(IsImportantKey, viewModel.IsImportant);
            viewModelBundle.PutBoolean(UseStartTimeKey, viewModel.UseStartTime);
            viewModelBundle.PutBoolean(UseDeadlineKey, viewModel.UseDeadline);
            _dateTimeStateHandler.Save(StartTimeKey, viewModel.StartTime, viewModelBundle);
            _dateTimeStateHandler.Save(DeadlineKey, viewModel.Deadline, viewModelBundle);

            return viewModelBundle;
        }

        private void FillViewModelWithBundle(Bundle bundle, IQuestViewModel viewModel)
        {
            DateTime? startTime = null;
            DateTime? deadline = null;

            viewModel.IsImportant = bundle.GetBoolean(IsImportantKey);
            viewModel.UseStartTime = bundle.GetBoolean(UseStartTimeKey);
            viewModel.UseDeadline = bundle.GetBoolean(UseDeadlineKey);

            _dateTimeStateHandler.Extract(StartTimeKey, bundle, ref startTime);
            _dateTimeStateHandler.Extract(DeadlineKey, bundle, ref deadline);

            viewModel.StartTime = startTime;
            viewModel.Deadline = deadline;
        }

        #endregion
    }
}