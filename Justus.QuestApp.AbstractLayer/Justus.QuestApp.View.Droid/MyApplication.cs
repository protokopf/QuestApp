using System;
using System.Collections;
using System.Collections.Generic;
using Android.App;
using Android.Runtime;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities.Errors;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.AbstractLayer.Validators;
using Justus.QuestApp.ModelLayer.Commands.Factories;
using Justus.QuestApp.ModelLayer.Factories;
using Justus.QuestApp.ModelLayer.Model;
using Justus.QuestApp.ModelLayer.Validators.QuestItself;
using Justus.QuestApp.ServiceLayer.DataServices;
using Justus.QuestApp.View.Droid.Abstract.EntityStateHandlers;
using Justus.QuestApp.View.Droid.EntityStateHandlers;
using Justus.QuestApp.View.Droid.EntityStateHandlers.VIewModels;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using Justus.QuestApp.View.Droid.StubServices;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails;

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
            InitializeQuestValidators();

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
                ServiceLocator.Resolve<IQuestCreator>().
                    Create(),
                ServiceLocator.Resolve<IQuestValidator<ClarifiedResponse<int>>>(),
                ServiceLocator.Resolve<IRepositoryCommandsFactory>()), useLikeFactory: true);
        }

        private void InitializeApplicationServices()
        {
            ServiceLocator.Register(() => new DateTimeStateHandler(), useLikeFactory: true);
            ServiceLocator.Register<IEntityStateHandler<IList<ClarifiedError<int>>>>(
                () => new ListOfClarifiedErrorIntStateHandler());            
            ServiceLocator.Register(() => new QuestViewModelStateHandler(
                ServiceLocator.Resolve<IEntityStateHandler<DateTime>>()));
        }

        private void InitializeQuestValidators()
        {
            IList<IQuestValidator<ClarifiedResponse<int>>> innerValidators = new List
                <IQuestValidator<ClarifiedResponse<int>>>()
                {
                    new TitleQuestValidator<int>(
                        Resource.String.ValidationTitleEmpty,
                        Resource.String.ValidationTitleEmptyClar,
                        Resource.String.ValidationTitleLong,
                        Resource.String.ValidationTitleLongClar),
                    new DescriptionQuestValidator<int>(
                        Resource.String.ValidationDescriptionEmpty,
                        Resource.String.ValidationDescriptionEmptyClar),
                    new StartTimeDeadlineQuestValidator<int>(
                        Resource.String.ValidationStartMoreThanDeadline,
                        Resource.String.ValidationStartMoreThanDeadlineClar,
                        Resource.String.ValidationDeadlineLessThanNow,
                        Resource.String.ValidationDeadlineLessThanNowClar)
                };                 
            ServiceLocator.Register(() => new CompositeQuestValidator<ClarifiedResponse<int>>(innerValidators));
        }
    }


}