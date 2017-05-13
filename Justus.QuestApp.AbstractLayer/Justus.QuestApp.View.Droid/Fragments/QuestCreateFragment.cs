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
        private const string DateTimePickerId = "DateTimePickerId";

        private const int DateTimePickerStartRequestCode = 0;
        private const int DateTimePickerDeadlineRequestCode = 1;

        private EditText _titleEditText;

        private EditText _descriptionEditText;

        private CheckBox _importanceCheckBox;

        private FloatingActionButton _saveButton;

        private CheckBox _startDateTimeCheckbox;

        private Button _startDateButton;

        private CheckBox _deadlineCheckbox;

        private Button _deadlineDateButton;

        public QuestCreateFragment()
        {
            _titleEditText = null;
        }

        #region Fragment overriding

        ///<inheritdoc/>
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.Activity.Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

            Android.Views.View mainView = inflater.Inflate(Resource.Layout.QuestCreateFragmentLayout, container, false);

            _titleEditText = mainView.FindViewById<EditText>(Resource.Id.questTitleEditText);
            HandleTitleEditText(_titleEditText);

            _descriptionEditText = mainView.FindViewById<EditText>(Resource.Id.questDescriptionEditText);
            HandleDescriptionEditText(_descriptionEditText);

            _importanceCheckBox = mainView.FindViewById<CheckBox>(Resource.Id.questCreateImportantCheckbox);

            _startDateTimeCheckbox = mainView.FindViewById<CheckBox>(Resource.Id.startCheckbox);
            _startDateButton = mainView.FindViewById<Button>(Resource.Id.startDateButton);

            _deadlineCheckbox = mainView.FindViewById<CheckBox>(Resource.Id.deadlineCheckbox);
            _deadlineDateButton = mainView.FindViewById<Button>(Resource.Id.deadlineDateButton);

            _saveButton = mainView.FindViewById<FloatingActionButton>(Resource.Id.questCreateSaveButton);

            _startDateTimeCheckbox.Click += StartDateTimeCheckboxOnClick;
            HandleStartDateButton(_startDateButton);

            _deadlineCheckbox.Click += DeadlineCheckboxOnClick;
            HandleDeadlineDateButton(_deadlineDateButton);

            _saveButton.Click += SaveButtonOnClick;

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
        public override void OnDestroy()
        {
            _saveButton.Click -= SaveButtonOnClick;

            _startDateTimeCheckbox.Click -= StartDateTimeCheckboxOnClick;
            _startDateButton.Click -= StartDateButtonOnClick;

            _deadlineCheckbox.Click -= DeadlineCheckboxOnClick;
            _deadlineDateButton.Click -= DeadlineDateButtonOnClick;

            base.OnDestroy();
        }

        #endregion

        #region Private methods

        #region Event handlers

        private void StartDateTimeCheckboxOnClick(object sender, EventArgs eventArgs)
        {
            HandleStartTimeSection(_startDateTimeCheckbox.Checked);
        }

        private void StartDateButtonOnClick(object sender, EventArgs e)
        {
            ShowDateTimePickerFragment(DateTimePickerStartRequestCode);
        }

        private void DeadlineCheckboxOnClick(object sender, EventArgs eventArgs)
        {
            HandleDeadlineSection(_deadlineCheckbox.Checked);
        }

        private void DeadlineDateButtonOnClick(object sender, EventArgs eventArgs)
        {
            ShowDateTimePickerFragment(DateTimePickerDeadlineRequestCode);
        }

        private void SaveButtonOnClick(object sender, EventArgs eventArgs)
        {
            ViewModel.Title = _titleEditText.Text;
            ViewModel.Description = _descriptionEditText.Text;
            ViewModel.IsImportant = _importanceCheckBox.Checked;

            this.Activity.SetResult(Result.Ok);
            this.Activity.Finish();
        }

        #endregion

        #region UI control handlers

        private void HandleTitleEditText(EditText title)
        {
            title.Text = ViewModel.Title;
        }

        private void HandleDescriptionEditText(EditText description)
        {
            description.Text = ViewModel.Description;
        }

        private void HandleDeadlineDateButton(Button deadlineDateButton)
        {
            deadlineDateButton.Click += DeadlineDateButtonOnClick;
            if (ViewModel.UseDeadline)
            {
                deadlineDateButton.Visibility = ViewStates.Visible;
                if (ViewModel.Deadline != DateTime.MinValue)
                {
                    deadlineDateButton.Text = StringifyDateTime(ViewModel.Deadline);
                }
            }
        }

        private void HandleStartDateButton(Button startDateButton)
        {
            startDateButton.Click += StartDateButtonOnClick;
            if (ViewModel.UseStartTime)
            {
                startDateButton.Visibility = ViewStates.Visible;
                if (ViewModel.StartTime != DateTime.MinValue)
                {
                    startDateButton.Text = StringifyDateTime(ViewModel.StartTime);
                }
            }
        }

        #endregion

        private void ShowDateTimePickerFragment(int requestCode)
        {
            DateTimePickerFragment fragment = DateTimePickerFragment.NewInstance(
                DateTime.Now - new TimeSpan(1,1,1), 
                this, 
                requestCode);
            fragment.Show(FragmentManager, DateTimePickerId);
        }

        private void HandleStartTimeSection(bool selectEnable)
        {
            _startDateButton.Visibility = selectEnable ? ViewStates.Visible : ViewStates.Invisible;
            ViewModel.UseStartTime = selectEnable;
        }

        private void HandleDeadlineSection(bool selectEnable)
        {
            _deadlineDateButton.Visibility = selectEnable ? ViewStates.Visible : ViewStates.Invisible;
            ViewModel.UseDeadline = selectEnable;
        }

        private string StringifyDateTime(DateTime dateTime)
        {
            return dateTime.ToString(CultureInfo.CurrentUICulture);
        }

        #endregion
    }
}