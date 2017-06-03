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
using Justus.QuestApp.View.Droid.Abstract.Activities;
using Justus.QuestApp.View.Droid.Fragments.Quest;
using Fragment = Android.Support.V4.App.Fragment;

namespace Justus.QuestApp.View.Droid.Activities
{
    [Activity(Label = "@string/QuestEditActivityLabel")]
    public class QuestEditActivity : AbstractFragmentActivity
    {
        private const string CurrentQuestId = "CurrentQuestIdKey";

        /// <summary>
        /// Returns intent fot starting current activity.
        /// </summary>
        /// <param name="questId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Intent GetStartIntent(int questId, Context context)
        {
            Intent intent = new Intent(context, typeof(QuestEditActivity));
            intent.PutExtra(CurrentQuestId, questId);
            return intent;
        }

        #region AbstractFragmentActivity overriding

        ///<inheritdoc cref="AbstractFragmentActivity"/>
        protected override Fragment CreateFragment()
        {
            int questId = Intent.GetIntExtra(CurrentQuestId, 0);
            return QuestEditFragment.NewInstance(questId);
        }

        #endregion
    }
}