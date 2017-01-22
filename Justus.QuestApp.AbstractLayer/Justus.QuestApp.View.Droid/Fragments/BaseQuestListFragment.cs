using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech.Tts;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.View.Droid.Adapters.List;
using Justus.QuestApp.View.Droid.ViewHolders;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Base class for quest list fragments.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TViewHolder"></typeparam>
    public abstract class BaseQuestListFragment<TViewModel, TViewHolder> : BaseFragment<TViewModel>
        where TViewModel : QuestListViewModel
        where TViewHolder : PositionedViewHolder
    {
        /// <summary>
        /// Adapter for list view.
        /// </summary>
        protected BaseQuestListAdapter<TViewModel, TViewHolder> QuestListAdapter;

        /// <summary>
        /// Reference to list view.
        /// </summary>
        protected ListView QuestListView;
    }
}