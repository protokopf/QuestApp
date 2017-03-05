using System;
using Android.App;
using Android.Runtime;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Factories;
using Justus.QuestApp.ModelLayer.Model;
using Justus.QuestApp.ServiceLayer.DataServices;
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
            //ServiceLocator.Register<IQuestRepository>(() => new StubQuestRepositoryService(10,1,3));
            ServiceLocator.Register<IQuestRepository>(() => new RecursiveQuestRepository(
                new RestDataStorage(), "http://192.168.0.104/api/Quests"));
            ServiceLocator.Register<IQuestProgressCounter>(() => new RecursiveQuestProgressCounter());
            ServiceLocator.Register<IStateCommandsFactory>(() => new DefaultStateCommandsFactory(ServiceLocator.Resolve<IQuestRepository>()));
        }

        private void InitializeViewModelServices()
        {
            ServiceLocator.Register(() => new ActiveQuestListViewModel());
            ServiceLocator.Register(() => new ResultsQuestListViewModel());
            ServiceLocator.Register(() => new AvailableQuestListViewModel());
        }

    }


}