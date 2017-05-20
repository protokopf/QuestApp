using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.View.Droid.ViewHolders;

namespace Justus.QuestApp.View.Droid.Adapters
{
    public class RecyclerAdapter : RecyclerView.Adapter
    {
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            AvailableQuestItemViewHolder holder = new AvailableQuestItemViewHolder();
        }

        public override int ItemCount { get; }
    }
}