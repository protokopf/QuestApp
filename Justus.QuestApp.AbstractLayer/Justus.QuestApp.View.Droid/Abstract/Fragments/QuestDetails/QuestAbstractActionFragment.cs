using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.View.Droid.Abstract.EntityStateHandlers;
using Justus.QuestApp.View.Droid.Fragments.Dialogs;
using Justus.QuestApp.View.Droid.Fragments.Quest;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails;

namespace Justus.QuestApp.View.Droid.Abstract.Fragments.QuestDetails
{
    public abstract class QuestAbstractActionFragment<TViewModel> : BaseFragment<TViewModel>
        where TViewModel : QuestAbstractActionViewModel
    {
        private static readonly CultureInfo DateTimeCulture = CultureInfo.CurrentUICulture;

        private const int DateTimePickerStartRequestCode = 0;
        private const int DateTimePickerDeadlineRequestCode = 1;

        private const string DateTimePickerId = "DateTimePickerId";
        private const string ValidationErrorsId = "ValidationErrorsId";

        private const string ViewModelKey = "QuestCreateViewModel.Key";

        private readonly IEntityStateHandler<QuestViewModel> _questDetailsStateHandler;
        private readonly DateTime _defaultDateTime = default(DateTime);

        protected EditText TitleEditText;
        protected EditText DescriptionEditText;

        protected CheckBox ImportanceCheckBox;

        protected CheckBox StartDateTimeCheckbox;
        protected Button StartDateButton;

        protected CheckBox DeadlineCheckbox;
        protected Button DeadlineDateButton;

        protected FloatingActionButton SaveButton;

        /// <summary>
        /// Default constructor. Resolves dependency on view model state handler.
        /// </summary>
        protected QuestAbstractActionFragment()
        {
            _questDetailsStateHandler = ServiceLocator.Resolve<IEntityStateHandler<QuestViewModel>>();
        }

        #region Fragment overriding

        ///<inheritdoc/>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ExtractViewModelState(savedInstanceState);           
            ParseArguments(Arguments);
            
        }

        ///<inheritdoc/>
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.Activity.Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);

            Android.Views.View mainView = inflater.Inflate(Resource.Layout.QuestCreateFragmentLayout, container, false);

            TitleEditText = mainView.FindViewById<EditText>(Resource.Id.questTitleEditText);
            DescriptionEditText = mainView.FindViewById<EditText>(Resource.Id.questDescriptionEditText);

            ImportanceCheckBox = mainView.FindViewById<CheckBox>(Resource.Id.questCreateImportantCheckbox);
            ImportanceCheckBox.CheckedChange += ImportanceCheckBoxOnCheckedChange;

            StartDateTimeCheckbox = mainView.FindViewById<CheckBox>(Resource.Id.startCheckbox);
            StartDateButton = mainView.FindViewById<Button>(Resource.Id.startDateButton);

            DeadlineCheckbox = mainView.FindViewById<CheckBox>(Resource.Id.deadlineCheckbox);
            DeadlineDateButton = mainView.FindViewById<Button>(Resource.Id.deadlineDateButton);

            SaveButton = mainView.FindViewById<FloatingActionButton>(Resource.Id.questCreateSaveButton);

            StartDateTimeCheckbox.CheckedChange += StartDateTimeCheckboxOnCheckedChange;
            StartDateButton.Click += StartDateButtonOnClick;

            DeadlineCheckbox.CheckedChange += DeadlineCheckboxOnCheckedChange;
            DeadlineDateButton.Click += DeadlineDateButtonOnClick;

            SaveButton.Click += SaveButtonOnClick;

            HandleStartDateButton(StartDateButton);
            HandleDeadlineDateButton(DeadlineDateButton);

            return mainView;
        }

        ///<inheritdoc/>
        public override void OnDestroyView()
        {
            SaveButton.Click -= SaveButtonOnClick;

            StartDateTimeCheckbox.CheckedChange -= StartDateTimeCheckboxOnCheckedChange;
            StartDateButton.Click -= StartDateButtonOnClick;

            DeadlineCheckbox.CheckedChange -= DeadlineCheckboxOnCheckedChange;
            DeadlineDateButton.Click -= DeadlineDateButtonOnClick;

            ImportanceCheckBox.CheckedChange -= ImportanceCheckBoxOnCheckedChange;

            base.OnDestroy();
        }

        ///<inheritdoc/>
        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            SaveViewModelState(outState);
        }

        ///<inheritdoc/>
        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (resultCode == (int)Result.Ok)
            {
                DateTime receivedDateTime = DateTimePickerFragment.GetItsDateTime(data.Extras);
                string receivedDateTimeString = StringifyDateTime(receivedDateTime);

                switch (requestCode)
                {
                    case DateTimePickerStartRequestCode:
                        ViewModel.QuestDetails.StartTime = receivedDateTime;
                        StartDateButton.Text = receivedDateTimeString;
                        break;
                    case DateTimePickerDeadlineRequestCode:
                        ViewModel.QuestDetails.Deadline = receivedDateTime;
                        DeadlineDateButton.Text = receivedDateTimeString;
                        break;
                }
            }
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

        private void StartDateButtonOnClick(object sender, EventArgs e)
        {
            ShowDateTimePickerFragment(DateTimePickerStartRequestCode, ViewModel.QuestDetails.StartTime);
        }

        private void DeadlineDateButtonOnClick(object sender, EventArgs eventArgs)
        {
            ShowDateTimePickerFragment(DateTimePickerDeadlineRequestCode, ViewModel.QuestDetails.Deadline);
        }

        private void ImportanceCheckBoxOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs checkedChangeEventArgs)
        {
            ViewModel.QuestDetails.IsImportant = checkedChangeEventArgs.IsChecked;
        }

        private void StartDateTimeCheckboxOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs eventArgs)
        {
            bool selectEnable = eventArgs.IsChecked;
            StartDateButton.Visibility = selectEnable ? ViewStates.Visible : ViewStates.Invisible;
            ViewModel.QuestDetails.UseStartTime = selectEnable;
        }

        private void DeadlineCheckboxOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs eventArgs)
        {
            bool selectEnable = eventArgs.IsChecked;
            DeadlineDateButton.Visibility = selectEnable ? ViewStates.Visible : ViewStates.Invisible;
            ViewModel.QuestDetails.UseDeadline = selectEnable;
        }



        private void SaveButtonOnClick(object sender, EventArgs eventArgs)
        {
            ViewModel.QuestDetails.Title = TitleEditText.Text;
            ViewModel.QuestDetails.Description = DescriptionEditText.Text;

            ClarifiedResponse<int> validationResult = ViewModel.Validate();
            if (validationResult.IsSuccessful)
            {
                ViewModel.Action();
                this.Activity.SetResult(Result.Ok);
                this.Activity.Finish();
            }
            else
            {
                ValidationErrorsFragment errorsFragment = ValidationErrorsFragment.NewInstance(validationResult.Errors);
                errorsFragment.Show(FragmentManager, ValidationErrorsId);
            }
        }

        #endregion

        #region UI control handlers

        private void HandleDeadlineDateButton(Button deadlineDateButton)
        {
            if (ViewModel.QuestDetails.Deadline != _defaultDateTime)
            {
                deadlineDateButton.Text = StringifyDateTime(ViewModel.QuestDetails.Deadline);
            }
            if (ViewModel.QuestDetails.UseDeadline)
            {
                deadlineDateButton.Visibility = ViewStates.Visible;
            }
        }

        private void HandleStartDateButton(Button startDateButton)
        {
            if (ViewModel.QuestDetails.StartTime != _defaultDateTime)
            {
                startDateButton.Text = StringifyDateTime(ViewModel.QuestDetails.StartTime);
            }
            if (ViewModel.QuestDetails.UseStartTime)
            {
                startDateButton.Visibility = ViewStates.Visible;
            }
        }

        #endregion

        #region Saving state methods

        /// <summary>
        /// Parses arguments.
        /// </summary>
        /// <param name="arguments"></param>
        protected abstract void ParseArguments(Bundle arguments);

        /// <summary>
        /// Parses saved instance state.
        /// </summary>
        /// <param name="bundle"></param>
        private void ExtractViewModelState(Bundle bundle)
        {
            var details = ViewModel.QuestDetails;
            _questDetailsStateHandler.Extract(ViewModelKey, bundle, ref details);
        }

        /// <summary>
        /// Saves view model state in the bundle.
        /// </summary>
        /// <param name="bundle"></param>
        private void SaveViewModelState(Bundle bundle)
        {
            _questDetailsStateHandler.Save(ViewModelKey, ViewModel.QuestDetails, bundle);
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

        #endregion

        #endregion
    }
}