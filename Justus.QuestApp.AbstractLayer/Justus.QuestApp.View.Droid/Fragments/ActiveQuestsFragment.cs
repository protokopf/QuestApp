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
using Justus.QuestApp.View.Droid.Adapters;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid.Fragments
{
    /// <summary>
    /// Fragment, that contains list of active quests.
    /// </summary>
    public class ActiveQuestsFragment : BaseFragment<ActiveQuestListViewModel>
    {
        private ListView _listView;

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View view = inflater.Inflate(Resource.Layout.QuestListFragmentLayout, container, false);
            _listView = view.FindViewById<ListView>(Resource.Id.questListId);

            _listView.Adapter = new QuestListAdapter(this,ViewModel);

            return view;
        }
    }
}