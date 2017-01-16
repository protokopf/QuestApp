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
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Base type for all fragments.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public class BaseFragment<TViewModel> : Fragment where TViewModel : BaseViewModel
    {
        /// <summary>
        /// Reference to view model.
        /// </summary>
        protected TViewModel ViewModel;

        /// <summary>
        /// Reference to progress dialog.
        /// </summary>
        protected ProgressDialog ProgressDialogRef;

        /// <summary>
        /// Default constructor. Resolves view model reference.
        /// </summary>
        public BaseFragment()
        {
            ViewModel = ServiceLocator.Resolve<TViewModel>();
        }

        #region Fragment overriding

        ///<inheritdoc/>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitiliazeProgressDialog();
        }

        ///<inheritdoc/>
        public override void OnPause()
        {
            base.OnPause();
            ViewModel.IsBusyChanged -= OnIsBusyChanged;
        }

        ///<inheritdoc/>
        public override void OnResume()
        {
            base.OnResume();
            ViewModel.IsBusyChanged += OnIsBusyChanged;
        }

        #endregion

        #region Private methods

        private void InitiliazeProgressDialog()
        {
            ProgressDialogRef = new ProgressDialog(Context, Resource.Style.MyMaterialTheme);
            ProgressDialogRef.SetProgressStyle(ProgressDialogStyle.Spinner);
            ProgressDialogRef.SetCancelable(false);
            ProgressDialogRef.SetTitle(Resource.String.ProgressDialogTitle);
        }

        private void OnIsBusyChanged(object sender, EventArgs e)
        {
            if (ViewModel.IsBusy)
            {
                ProgressDialogRef.Show();
            }
            else
            {
                ProgressDialogRef.Hide();
            }
        }

        private void DisplayException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            AlertDialog.Builder builder = new AlertDialog.Builder(Context, Resource.Style.MyMaterialTheme);
            builder.SetTitle(Resource.String.ErrorTitle);
            builder.SetMessage(exception.Message);
            builder.SetPositiveButton(Android.Resource.String.Ok, (IDialogInterfaceOnClickListener)null);
            builder.Show();
        }

        #endregion
    }

}
