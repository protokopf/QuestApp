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
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.View.Droid.Activities;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.View.Droid.StubServices;
using Justus.QuestApp.ViewModelLayer.ViewModels;

namespace Justus.QuestApp.View.Droid
{
    [Application(Theme = "@style/MyMaterialTheme")]
    public class MyApplication : Application
    {
        /// <summary>
        /// Need for base class.
        /// </summary>
        /// <param name="javaReference"></param>
        /// <param name="transfer"></param>
        public MyApplication(IntPtr javaReference, JniHandleOwnership transfer) :
            base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
            InitializeModelServices();
            InitializeViewModelServices();          
        }

        private void InitializeModelServices()
        {
            ServiceLocator.Register<IQuestRepository>(() => new StubQuestRepositoryService(15));
            ServiceLocator.Register<ICommandManager>(() => new StubCommandManager());
        }

        private void InitializeViewModelServices()
        {
            ServiceLocator.Register<ActiveQuestListViewModel>(() => new ActiveQuestListViewModel());
        }

    }


}