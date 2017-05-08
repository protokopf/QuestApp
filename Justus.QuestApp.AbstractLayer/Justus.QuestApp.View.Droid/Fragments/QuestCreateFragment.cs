using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
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

        private EditText _titleEditText;

        private EditText _descriptionEditText;

        private CheckBox _importanceCheckBox;

        private FloatingActionButton _saveButton;

        private CheckBox _startDateTimeCheckbox;

        private Button _startDateButton;

        private CheckBox _deadlineCheckbox;

        private Button _deadlineDateButton;

        #region Fragment overriding

        ///<inheritdoc/>
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View mainView = inflater.Inflate(Resource.Layout.QuestCreateFragmentLayout, container, false);

            _titleEditText = mainView.FindViewById<EditText>(Resource.Id.questTitleEditText);
            _descriptionEditText = mainView.FindViewById<EditText>(Resource.Id.questDescriptionEditText);

            _importanceCheckBox = mainView.FindViewById<CheckBox>(Resource.Id.questCreateImportantCheckbox);

            _startDateTimeCheckbox = mainView.FindViewById<CheckBox>(Resource.Id.startCheckbox);
            _startDateButton = mainView.FindViewById<Button>(Resource.Id.startDateButton);

            _deadlineCheckbox = mainView.FindViewById<CheckBox>(Resource.Id.deadlineCheckbox);
            _deadlineDateButton = mainView.FindViewById<Button>(Resource.Id.deadlineDateButton);

            _saveButton = mainView.FindViewById<FloatingActionButton>(Resource.Id.questCreateSaveButton);

            _startDateTimeCheckbox.Click += StartDateTimeCheckboxOnClick;
            _startDateButton.Click += StartDateButtonOnClick;

            _deadlineCheckbox.Click += DeadlineCheckboxOnClick;
            _deadlineDateButton.Click += DeadlineDateButtonOnClick;

            _saveButton.Click += SaveButtonOnClick;

            return mainView;
        }


        ///<inheritdoc/>
        public override void OnDestroy()
        {
            _saveButton.Click -= SaveButtonOnClick;

            _startDateTimeCheckbox.Click -= StartDateTimeCheckboxOnClick;
            _startDateButton.Click -= StartDateButtonOnClick;

            _deadlineCheckbox.Click -= DeadlineCheckboxOnClick;
            base.OnDestroy();
        }

        #endregion

        #region Private methods

        #region Handlers

        private void StartDateTimeCheckboxOnClick(object sender, EventArgs eventArgs)
        {
            HandleStartTimeSection(_startDateTimeCheckbox.Checked);
        }

        private void StartDateButtonOnClick(object sender, EventArgs e)
        {
            ShowDateTimePickerFragment();
        }

        private void DeadlineCheckboxOnClick(object sender, EventArgs eventArgs)
        {
            HandleDeadlineSection(_deadlineCheckbox.Checked);
        }

        private void DeadlineDateButtonOnClick(object sender, EventArgs eventArgs)
        {
            ShowDateTimePickerFragment();
        }

        private void SaveButtonOnClick(object sender, EventArgs eventArgs)
        {
            this.Activity.SetResult(Result.Ok);
            this.Activity.Finish();
        }

        #endregion

        private void ShowDateTimePickerFragment()
        {
            DateTimePickerFragment fragment = new DateTimePickerFragment();
            fragment.Show(FragmentManager, DateTimePickerId);
        }

        private void HandleStartTimeSection(bool selectEnable)
        {
            _startDateButton.Visibility = selectEnable ? ViewStates.Visible : ViewStates.Invisible;
        }

        private void HandleDeadlineSection(bool selectEnable)
        {
            _deadlineDateButton.Visibility = selectEnable ? ViewStates.Visible : ViewStates.Invisible;
        }

        #endregion
    }
}