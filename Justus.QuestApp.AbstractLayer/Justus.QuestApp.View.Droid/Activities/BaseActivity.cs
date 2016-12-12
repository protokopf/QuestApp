using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using Justus.QuestApp.ModelLayer.Helpers;

namespace Justus.QuestApp.View.Droid.Activities
{
    /// <summary>
    /// Base type for all activities.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public class BaseActivity<TViewModel> : Activity where TViewModel : BaseViewModel
    {
        /// <summary>
        /// Reference to view model.
        /// </summary>
        protected readonly TViewModel _viewModel;

        /// <summary>
        /// Progress dialog for async operations.
        /// </summary>
        protected ProgressDialog _progress;

        /// <summary>
        /// Default constructor. Initializes view model reference.
        /// </summary>
        public BaseActivity()
        {
            _viewModel = ServiceLocator.Resolve<TViewModel>();
        }

        #region Activity overriding

        ///<inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _progress = new ProgressDialog(this);
            _progress.SetCancelable(false);
            _progress.SetTitle(Resource.String.ProgressDialogTitle);
        }

        ///<inheritdoc/>
        protected override void OnPause()
        {
            base.OnPause();
            _viewModel.IsBusyChanged -= OnIsBusyChanged;
        }

        ///<inheritdoc/>
        protected override void OnResume()
        {
            base.OnResume();
            _viewModel.IsBusyChanged += OnIsBusyChanged;
        }

        #endregion

        /// <summary>
        /// Displays exceptions.
        /// </summary>
        /// <param name="ex"></param>
        protected void DisplayError(Exception ex)
        {
            new AlertDialog.Builder(this)
                .SetTitle(Resource.String.ErrorTitle)
                .SetMessage(ex.Message)
                .SetPositiveButton(Android.Resource.String.Ok, (IDialogInterfaceOnClickListener)null)
                .Show();
        }

        private void OnIsBusyChanged(object sender, EventArgs e)
        {
            if(_viewModel.IsBusy)
            {
                _progress.Show();
            }
            else
            {
                _progress.Hide();
            }
        }


    }
}