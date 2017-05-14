using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.View.Droid.Abstract.Activities;
using Justus.QuestApp.View.Droid.Fragments;
using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Activities
{
    /// <summary>
    /// Activity for creating quest.
    /// </summary>
    [Activity(Label= "@string/QuestCreateActivityLabel")]
    public class QuestCreateActivity : AbstractFragmentActivity
    {
        private static readonly string ParentIdKey = "QuestCreateActivity.ParentIdKey";

        /// <summary>
        /// Returns start intent for this activity.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Intent GetStartIntent(int parentId, Context context)
        {
            Intent intent = new Intent(context, typeof(QuestCreateActivity));
            Bundle bundle = new Bundle();
            bundle.PutInt(ParentIdKey, parentId);
            intent.PutExtras(bundle);
            return intent;
        }

        #region AbstractFragmentActivity overriding

        ///<inhertidoc/>
        protected override Fragment CreateFragment()
        {
            int parentId = this.Intent.GetIntExtra(ParentIdKey, 0);
            return QuestCreateFragment.NewInstance(parentId);
        }

        #endregion
    }
}