using Android.OS;
using Android.Views;
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