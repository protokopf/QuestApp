using System;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.View.Droid.Abstract.EntityStateHandlers;
using Justus.QuestApp.View.Droid.Fragments.Dialogs;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails.Abstract;

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

        private readonly IEntityStateHandler<IQuestViewModel> _questDetailsStateHandler;

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
            _questDetailsStateHandler = ServiceLocator.Resolve<IEntityStateHandler<IQuestViewModel>>();
        }

        #region Fragment overriding

        ///<inheritdoc/>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ParseArguments(Arguments);
            ViewModel.Initialize();
            ExtractViewModelState(savedInstanceState);           
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
                DateTime? receivedDateTime = DateTimePickerFragment.GetItsDateTime(data.Extras);

                if (receivedDateTime != null)
                {
                    string receivedDateTimeString = StringifyDateTime(receivedDateTime.Value);

                    switch (requestCode)
                    {
                        case DateTimePickerStartRequestCode:
                            ViewModel.QuestViewModel.StartTime = receivedDateTime;
                            StartDateButton.Text = receivedDateTimeString;
                            break;
                        case DateTimePickerDeadlineRequestCode:
                            ViewModel.QuestViewModel.Deadline = receivedDateTime;
                            DeadlineDateButton.Text = receivedDateTimeString;
                            break;
                    }
                }
            }
        }

        ///<inheritdoc/>
        public override void OnViewCreated(Android.Views.View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            HandleUiElements();
        }

        #endregion

        #region Private methods

        private void ShowDateTimePickerFragment(int requestCode, DateTime? startDateTime)
        {
            DateTimePickerFragment fragment = DateTimePickerFragment.NewInstance(
                startDateTime ?? DateTime.Now,
                this,
                requestCode);
            fragment.Show(FragmentManager, DateTimePickerId);
        }

        #region Event handlers

        private void StartDateButtonOnClick(object sender, EventArgs e)
        {
            ShowDateTimePickerFragment(DateTimePickerStartRequestCode, ViewModel.QuestViewModel.StartTime);
        }

        private void DeadlineDateButtonOnClick(object sender, EventArgs eventArgs)
        {
            ShowDateTimePickerFragment(DateTimePickerDeadlineRequestCode, ViewModel.QuestViewModel.Deadline);
        }

        private void ImportanceCheckBoxOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs checkedChangeEventArgs)
        {
            ViewModel.QuestViewModel.IsImportant = checkedChangeEventArgs.IsChecked;
        }

        private void StartDateTimeCheckboxOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs eventArgs)
        {
            bool selectEnable = eventArgs.IsChecked;
            StartDateButton.Visibility = selectEnable ? ViewStates.Visible : ViewStates.Invisible;
            ViewModel.QuestViewModel.UseStartTime = selectEnable;
        }

        private void DeadlineCheckboxOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs eventArgs)
        {
            bool selectEnable = eventArgs.IsChecked;
            DeadlineDateButton.Visibility = selectEnable ? ViewStates.Visible : ViewStates.Invisible;
            ViewModel.QuestViewModel.UseDeadline = selectEnable;
        }

        private void SaveButtonOnClick(object sender, EventArgs eventArgs)
        {
            ViewModel.QuestViewModel.Title = TitleEditText.Text;
            ViewModel.QuestViewModel.Description = DescriptionEditText.Text;

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

        /// <summary>
        /// Handles UI elements.
        /// </summary>
        protected virtual void HandleUiElements()
        {
            HandleStartDateButton();
            HandleDeadlineDateButton();
        }

        /// <summary>
        /// Handles deadline date button.
        /// </summary>
        private void HandleDeadlineDateButton()
        {
            if (DeadlineDateButton != null)
            {
                if (ViewModel.QuestViewModel.Deadline != null)
                {
                    DeadlineDateButton.Text = StringifyDateTime(ViewModel.QuestViewModel.Deadline.Value);
                }
                if (ViewModel.QuestViewModel.UseDeadline)
                {
                    DeadlineDateButton.Visibility = ViewStates.Visible;
                }
            }
        }

        /// <summary>
        /// Handles start date button.
        /// </summary>
        private void HandleStartDateButton()
        {
            if (StartDateButton != null)
            {
                if (ViewModel.QuestViewModel.StartTime != null)
                {
                    StartDateButton.Text = StringifyDateTime(ViewModel.QuestViewModel.StartTime.Value);
                }
                if (ViewModel.QuestViewModel.UseStartTime)
                {
                    StartDateButton.Visibility = ViewStates.Visible;
                }
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
            var details = ViewModel.QuestViewModel;
            _questDetailsStateHandler.Extract(ViewModelKey, bundle, ref details);
        }

        /// <summary>
        /// Saves view model state in the bundle.
        /// </summary>
        /// <param name="bundle"></param>
        private void SaveViewModelState(Bundle bundle)
        {
            _questDetailsStateHandler.Save(ViewModelKey, ViewModel.QuestViewModel, bundle);
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