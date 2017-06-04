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
using Justus.QuestApp.View.Droid.Abstract.Fragments.QuestDetails;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails;

namespace Justus.QuestApp.View.Droid.Fragments.Quest
{
    /// <summary>
    /// Fragment for editing quest.
    /// </summary>
    public class QuestEditFragment : QuestAbstractActionFragment<QuestEditViewModel>
    {
        private const string QuestIdKey = "QuestIdKey";

        /// <summary>
        /// Returns new instance of fragment.
        /// </summary>
        /// <param name="questId"></param>
        /// <returns></returns>
        public static  QuestEditFragment NewInstance(int questId)
        {
            QuestEditFragment fragment = new QuestEditFragment();

            Bundle arguments = new Bundle();
            arguments.PutInt(QuestIdKey, questId);

            fragment.Arguments = arguments;

            return fragment;
        }

        #region QuestAbstractActionFragment overriding

        ///<inheritdoc />
        public override void OnViewCreated(Android.Views.View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            FillInputs();
        }

        ///<inheritdoc />
        protected override void ParseArguments(Bundle arguments)
        {
            if (arguments != null)
            {
                int questId = arguments.GetInt(QuestIdKey);
                ViewModel.QuestId = questId;
            }
        }

        #endregion

        private void FillInputs()
        {
            IQuestViewModel questViewModel = ViewModel.QuestViewModel;

            TitleEditText.Text = questViewModel.Title;
            DescriptionEditText.Text = questViewModel.Description;
            ImportanceCheckBox.Checked = questViewModel.IsImportant;
            StartDateTimeCheckbox.Checked = questViewModel.UseStartTime;
            DeadlineCheckbox.Checked = questViewModel.UseDeadline;
        }
    }
}