using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


using Fragment = Android.Support.V4.App.Fragment;


namespace Justus.QuestApp.View.Droid.Fragments
{
    class StubFragment : Fragment
    {
        private static int counter = 0;

        public StubFragment()
        {
            counter++;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            counter--;
        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Android.Views.View view =  inflater.Inflate(Resource.Layout.StubFragmentLayout, container, false);
            TextView textView = view.FindViewById<TextView>(Resource.Id.stubTextView);
            textView.Text += $"_{counter.ToString()}";
            return view;
        }
    }
}