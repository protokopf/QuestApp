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
using Java.Lang;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Adapters
{
    /// <summary>
    /// Adapter for exapndable quests.
    /// </summary>
    public abstract class ExpandableListOfQuestsAdapter<TViewModel> 
        : BaseExpandableListAdapter where TViewModel : BaseViewModel
    {
        /// <summary>
        /// Reference to view model.
        /// </summary>
        protected TViewModel _viewModel;

        /// <summary>
        /// Reference to activity.
        /// </summary>
        protected Activity _activity;

        /// <summary>
        /// Receives activity and view model.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="viewModel"></param>
        protected ExpandableListOfQuestsAdapter(Activity activity, TViewModel viewModel)
        {
            _activity = activity;
            _viewModel = viewModel;
        }
    }
}