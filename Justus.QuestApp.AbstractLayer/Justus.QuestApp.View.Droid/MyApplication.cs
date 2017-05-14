using System;
using Android.App;
using Android.Runtime;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Factories;
using Justus.QuestApp.ModelLayer.Factories;
using Justus.QuestApp.ModelLayer.Model;
using Justus.QuestApp.ServiceLayer.DataServices;
using Justus.QuestApp.View.Droid.EntityStateHandlers;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using Justus.QuestApp.View.Droid.StubServices;

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
            InitializeApplicationServices();
        }

        private void InitializeModelServices()
        {
            ServiceLocator.Register(() => new StubQuestRepositoryService(12, 1, 3));
            ServiceLocator.Register(() => new RecursiveQuestRepository(
                ServiceLocator.Resolve<IDataAccessInterface<Quest>>(),
                "some connection string"
                ));
            ServiceLocator.Register(() => new SqliteQuestCreator());
            //ServiceLocator.Register<IQuestRepository>(() => new RecursiveQuestRepository(
            //    new RestDataStorage(), "http://192.168.0.104/api/Quests"));
            ServiceLocator.Register(() => new RecursiveQuestProgressCounter());
            ServiceLocator.Register(() => new DefaultStateCommandsFactory(ServiceLocator.Resolve<IQuestRepository>()));
            ServiceLocator.Register(() => new DefaultRepositoryCommandsFactory(ServiceLocator.Resolve<IQuestRepository>()));
        }

        private void InitializeViewModelServices()
        {
            ServiceLocator.Register(() => new ActiveQuestListViewModel(
                ServiceLocator.Resolve<IQuestRepository>(),
                ServiceLocator.Resolve<IStateCommandsFactory>(),
                ServiceLocator.Resolve<IRepositoryCommandsFactory>(),
                ServiceLocator.Resolve<IQuestProgressCounter>()
                ));

            ServiceLocator.Register(() => new ResultsQuestListViewModel(
                ServiceLocator.Resolve<IQuestRepository>(),
                ServiceLocator.Resolve<IStateCommandsFactory>(),
                ServiceLocator.Resolve<IRepositoryCommandsFactory>()));

            ServiceLocator.Register(() => new AvailableQuestListViewModel(
                ServiceLocator.Resolve<IQuestRepository>(),
                ServiceLocator.Resolve<IStateCommandsFactory>(),
                ServiceLocator.Resolve<IRepositoryCommandsFactory>()));

            ServiceLocator.Register(() => new QuestCreateViewModel(
                ServiceLocator.Resolve<IQuestCreator>(),
                ServiceLocator.Resolve<IRepositoryCommandsFactory>()), false);
        }

        private void InitializeApplicationServices()
        {
            ServiceLocator.Register(() => new DateTimeStateHandler());
        }

    }


}