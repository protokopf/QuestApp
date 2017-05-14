using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.View.Droid.Abstract.EntityStateHandlers;
using Justus.QuestApp.View.Droid.Abstract.Fragments;
using Justus.QuestApp.View.Droid.Fragments.Dialogs;
using Justus.QuestApp.View.Droid.Helpers;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Fragment for editing quest info.
    /// </summary>
    public class QuestCreateFragment : BaseFragment<QuestCreateViewModel>
    {
        private static readonly CultureInfo DateTimeCulture = CultureInfo.CurrentUICulture;

        private const string DateTimePickerId = "DateTimePickerId";

        private const string DeadlineDateTimeKey = "DeadlineDateTimeKey";
        private const string StartDateTimeKey = "StartDateTimeKey";
        private const string UseDeadlineKey = "UseDeadlineKey";
        private const string UseStartKey = "UseStartKey";
        private const string IsImportantKey = "IsImportantKey";

        private const int DateTimePickerStartRequestCode = 0;
        private const int DateTimePickerDeadlineRequestCode = 1;

        private readonly IEntityStateHandler<DateTime> _dateTimeHandler;
        private readonly DateTime _defaultDateTime = DateTime.MinValue;

        private EditText _titleEditText;
        private EditText _descriptionEditText;

        private CheckBox _importanceCheckBox;

        private CheckBox _startDateTimeCheckbox;
        private Button _startDateButton;

        private CheckBox _deadlineCheckbox;
        private Button _deadlineDateButton;

        private FloatingActionButton _saveButton;

        public QuestCreateFragment()
        {
            _dateTimeHandler = ServiceLocator.Resolve<IEntityStateHandler<DateTime>>();
        }

        #region Fragment overriding

        ///<inheritdoc/>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ExtractViewModelState(savedInstanceState);
        }

        ///<inheritdoc/>
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.Activity.Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);
            
            Android.Views.View mainView = inflater.Inflate(Resource.Layout.QuestCreateFragmentLayout, container, false);

            _titleEditText = mainView.FindViewById<EditText>(Resource.Id.questTitleEditText);
            _descriptionEditText = mainView.FindViewById<EditText>(Resource.Id.questDescriptionEditText);

            _importanceCheckBox = mainView.FindViewById<CheckBox>(Resource.Id.questCreateImportantCheckbox);
            _importanceCheckBox.CheckedChange += ImportanceCheckBoxOnCheckedChange;

            _startDateTimeCheckbox = mainView.FindViewById<CheckBox>(Resource.Id.startCheckbox);
            _startDateButton = mainView.FindViewById<Button>(Resource.Id.startDateButton);
            
            _deadlineCheckbox = mainView.FindViewById<CheckBox>(Resource.Id.deadlineCheckbox);
            _deadlineDateButton = mainView.FindViewById<Button>(Resource.Id.deadlineDateButton);
            
            _saveButton = mainView.FindViewById<FloatingActionButton>(Resource.Id.questCreateSaveButton);

            _startDateTimeCheckbox.CheckedChange += StartDateTimeCheckboxOnCheckedChange;
            _startDateButton.Click += StartDateButtonOnClick;

            _deadlineCheckbox.CheckedChange += DeadlineCheckboxOnCheckedChange;
            _deadlineDateButton.Click += DeadlineDateButtonOnClick;

            _saveButton.Click += SaveButtonOnClick;

            HandleStartDateButton(_startDateButton);
            HandleDeadlineDateButton(_deadlineDateButton);

            return mainView;
        }

        ///<inheritdoc/>
        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (resultCode == (int) Result.Ok)
            {
                DateTime receivedDateTime = DateTimePickerFragment.GetItsDateTime(data.Extras);
                string receivedDateTimeString = StringifyDateTime(receivedDateTime);

                switch (requestCode)
                {
                    case DateTimePickerStartRequestCode:
                        ViewModel.StartTime = receivedDateTime;
                        _startDateButton.Text = receivedDateTimeString;
                        break;
                    case DateTimePickerDeadlineRequestCode:
                        ViewModel.Deadline = receivedDateTime;
                        _deadlineDateButton.Text = receivedDateTimeString;
                        break;
                }
            }
        }

        ///<inheritdoc/>
        public override void OnDestroyView()
        {
            _saveButton.Click -= SaveButtonOnClick;

            _startDateTimeCheckbox.CheckedChange -= StartDateTimeCheckboxOnCheckedChange;
            _startDateButton.Click -= StartDateButtonOnClick;

            _deadlineCheckbox.CheckedChange -= DeadlineCheckboxOnCheckedChange;
            _deadlineDateButton.Click -= DeadlineDateButtonOnClick;

            _importanceCheckBox.CheckedChange -= ImportanceCheckBoxOnCheckedChange;

            base.OnDestroy();
        }

        ///<inheritdoc/>
        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            SaveViewModelState(outState);
        }

        #endregion

        #region Private methods

        private void ShowDateTimePickerFragment(int requestCode, DateTime startDateTime)
        {
            DateTimePickerFragment fragment = DateTimePickerFragment.NewInstance(
                startDateTime == _defaultDateTime ? DateTime.Now : startDateTime,
                this,
                requestCode);
            fragment.Show(FragmentManager, DateTimePickerId);
        }

        #region Event handlers

        private void ImportanceCheckBoxOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs checkedChangeEventArgs)
        {
            ViewModel.IsImportant = checkedChangeEventArgs.IsChecked;
        }

        private void StartDateTimeCheckboxOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs eventArgs)
        {
            bool selectEnable = eventArgs.IsChecked;
            _startDateButton.Visibility = selectEnable ? ViewStates.Visible : ViewStates.Invisible;
            ViewModel.UseStartTime = selectEnable;
        }

        private void StartDateButtonOnClick(object sender, EventArgs e)
        {
            ShowDateTimePickerFragment(DateTimePickerStartRequestCode, ViewModel.StartTime);
        }

        private void DeadlineCheckboxOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs eventArgs)
        {
            bool selectEnable = eventArgs.IsChecked;
            _deadlineDateButton.Visibility = selectEnable ? ViewStates.Visible : ViewStates.Invisible;
            ViewModel.UseDeadline = selectEnable;
        }

        private void DeadlineDateButtonOnClick(object sender, EventArgs eventArgs)
        {
            ShowDateTimePickerFragment(DateTimePickerDeadlineRequestCode, ViewModel.Deadline);
        }

        private void SaveButtonOnClick(object sender, EventArgs eventArgs)
        {
            this.Activity.SetResult(Result.Ok);
            this.Activity.Finish();
        }

        #endregion

        #region UI control handlers

        private void HandleDeadlineDateButton(Button deadlineDateButton)
        {
            if (ViewModel.UseDeadline)
            {
                deadlineDateButton.Visibility = ViewStates.Visible;
                if (ViewModel.Deadline != _defaultDateTime)
                {
                    deadlineDateButton.Text = StringifyDateTime(ViewModel.Deadline);
                }
            }
        }

        private void HandleStartDateButton(Button startDateButton)
        {
            if (ViewModel.UseStartTime)
            {
                startDateButton.Visibility = ViewStates.Visible;
                if (ViewModel.StartTime != _defaultDateTime)
                {
                    startDateButton.Text = StringifyDateTime(ViewModel.StartTime);
                }
            }
        }

        #endregion

        #region Saving state methods

        private void SaveViewModelState(Bundle bundle)
        {
            if (bundle != null)
            {
                _dateTimeHandler.Save(StartDateTimeKey, ViewModel.StartTime, bundle);
                _dateTimeHandler.Save(DeadlineDateTimeKey, ViewModel.Deadline, bundle);

                bundle.PutBoolean(UseStartKey, ViewModel.UseStartTime);
                bundle.PutBoolean(UseDeadlineKey, ViewModel.UseDeadline);
                bundle.PutBoolean(IsImportantKey, ViewModel.IsImportant);
            }
        }

        private void ExtractViewModelState(Bundle bundle)
        {
            if (bundle != null)
            {
                ViewModel.StartTime = _dateTimeHandler.Extract(StartDateTimeKey, bundle);
                ViewModel.Deadline = _dateTimeHandler.Extract(DeadlineDateTimeKey, bundle);
                ViewModel.UseStartTime = bundle.GetBoolean(UseStartKey);
                ViewModel.UseDeadline = bundle.GetBoolean(UseDeadlineKey);
                ViewModel.IsImportant = bundle.GetBoolean(IsImportantKey);
            }
        }

        /// <summary>
        /// Stringify dateTime to string.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private string StringifyDateTime(DateTime dateTime)
        {
            return dateTime.ToString(DateTimeCulture);
        }

        /// <summary>
        /// Try parse string to dateTime.
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns></returns>
        private DateTime ParseDateTimeString(string dateTimeString)
        {
            DateTime dt = _defaultDateTime;
            if (!string.IsNullOrWhiteSpace(dateTimeString))
            {
                DateTime.TryParse(dateTimeString, DateTimeCulture, DateTimeStyles.None, out dt);
            }
            return dt;
        }

        /// <summary>
        /// Puts dateTime to bundle under specific key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dateTime"></param>
        /// <param name="bundle"></param>
        private void PutDateTimeToBundle(string key, DateTime dateTime, Bundle bundle)
        {
            bundle.PutString(key, StringifyDateTime(dateTime));
        }

        /// <summary>
        /// Get dateTime from bundle using specific key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bundle"></param>
        /// <returns></returns>
        private DateTime GetDateTimeFromBundle(string key, Bundle bundle)
        {
            string dateTimeString = bundle.GetString(key);       
            return ParseDateTimeString(dateTimeString); ;
        }

        #endregion

        #endregion
    }
}