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
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.View.Droid.Adapters;
using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Fragments
{
    public class StubQuestListFragment : Fragment
    {
        private ListView mListView;

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View view = inflater.Inflate(Resource.Layout.QuestListFragmentLayout, container, false);
            mListView = view.FindViewById<ListView>(Resource.Id.questListId);

            StubQuestListAdapter adapter = new StubQuestListAdapter(this,GetQuests(20));
            mListView.Adapter = adapter;

            return view;
        }

        private List<Quest> GetQuests(int count)
        {
            List<Quest> quests = new List<Quest>();
            for (int i = 0; i < count; ++i)
            {
                quests.Add(new StubQuest()
                {
                    Title = "title_" + i,
                    Deadline = DateTime.Now + new TimeSpan(i, 0, 0)
                });
            }
            return quests;
        }

        
    }

    class StubQuest : Quest
    {
        public override int Id { get; set; }
        public override int ParentId { get; set; }
        public override string Title { get; set; }
        public override string Description { get; set; }
        public override Quest Parent { get; set; }
        public override List<Quest> Children { get; set; }
        public override DateTime Deadline { get; set; }
    }
}