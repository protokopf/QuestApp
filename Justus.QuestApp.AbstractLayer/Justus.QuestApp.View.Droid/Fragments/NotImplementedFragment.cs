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
    /// <summary>
    /// Stub for not implemented fragments.
    /// </summary>
    public class NotImplementedFragment : Fragment
    {
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.NotImplementedFragmentLayout, container, false);
        }
    }
}