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
using Justus.QuestApp.View.Droid.Helpers;
using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Fragment for editing quest info.
    /// </summary>
    public class QuestInfoFragment : Fragment
    {
        private Button _saveButton;
        private Button _cancelButton;

        #region Fragment overriding

        ///<inheritdoc/>
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View mainView = inflater.Inflate(Resource.Layout.QuestInfoFragmentLayout, container, false);

            _saveButton = mainView.FindViewById<Button>(Resource.Id.saveCreateButton);
            _cancelButton = mainView.FindViewById<Button>(Resource.Id.cancelCreateButton);

            _saveButton.Click += SaveButtonOnClick;
            _cancelButton.Click += CancelButtonOnClick;

            return mainView;
        }

        ///<inheritdoc/>
        public override void OnDestroy()
        {
            _saveButton.Click -= SaveButtonOnClick;
            _cancelButton.Click -= CancelButtonOnClick;
            base.OnDestroy();
        }

        #endregion

        #region Private methods

        private void CancelButtonOnClick(object sender, EventArgs eventArgs)
        {
            this.Activity.SetResult(Result.Canceled);
            this.Activity.Finish();
        }

        private void SaveButtonOnClick(object sender, EventArgs eventArgs)
        {
            this.Activity.SetResult(Result.Ok);
            this.Activity.Finish();
        }


        #endregion
    }
}