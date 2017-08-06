using System;
using System.Collections.Generic;
using System.IO;
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
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.AbstractLayer.Validators;
using Justus.QuestApp.ModelLayer.Commands.Factories;
using Justus.QuestApp.ModelLayer.Commands.Factories.Wrappers;
using Justus.QuestApp.ModelLayer.Factories;
using Justus.QuestApp.ModelLayer.Model.Progress;
using Justus.QuestApp.ModelLayer.Validators.QuestItself;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails;
using Justus.QuestApp.DataLayer.Data;
using Justus.QuestApp.DataLayer.Platform;
using Justus.QuestApp.ModelLayer.Model.QuestList;
using Justus.QuestApp.ModelLayer.Model.QuestTree;
using Justus.QuestApp.View.Droid.Abstract.EntityStateHandlers;
using Justus.QuestApp.View.Droid.Data.Platform;
using Justus.QuestApp.View.Droid.EntityStateHandlers;
using Justus.QuestApp.View.Droid.EntityStateHandlers.VIewModels;
using Justus.QuestApp.ViewModelLayer.Factories;
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
            InitializeDataLayerServices();
            InitializeModelServices();
            InitializeViewModelServices();
            InitializeApplicationServices();
        }

        private void InitializeDataLayerServices()
        {
            //Quest data access layer.
            //ServiceLocator.Register(() => new StubQuestRepositoryService(12, 1, 3));
            string pathToStorage = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "test.db3");
            ServiceLocator.Register<IQuestDataLayer>(() => new SqliteOrmQuestDataLayer(pathToStorage, new AndroidPlatformSqliteFactory()));
            ServiceLocator.Register<IQuestFactory>(() => new SqliteQuestFactory());
        }

        private void InitializeModelServices()
        {
            //Quest tree. Model layer.
            ServiceLocator.Register<IQuestTree>(() => new QuestTreeInMemory(
                ServiceLocator.Resolve<IQuestDataLayer>(),
                ServiceLocator.Resolve<IQuestFactory>()
            ));

            //Quest progress recounter.
            ServiceLocator.Register<IQuestProgressRecounter>(() => 
                new AllUpperQuestProgressRecounter(
                    ServiceLocator.Resolve<IQuestTree>()));


            //Quest state commands factory. Used to produce commands for changing quest state.
            ServiceLocator.Register<IStateCommandsFactory>(() => 
                new RecountProgressStateCommandsFactory(
                    new DefaultStateCommandsFactory(
                        ServiceLocator.Resolve<IQuestTree>()), 
                    ServiceLocator.Resolve<IQuestProgressRecounter>()));

            //Quest repository commands factory. Used to produce commands for changing quest trees.
            ServiceLocator.Register<ITreeCommandsFactory>(() =>
                new RecountProgressTreeCommandsFactory(
                    new DefaultTreeCommandsFactory(
                        ServiceLocator.Resolve<IQuestTree>()),
                    ServiceLocator.Resolve<IQuestProgressRecounter>()));

            InitializeQuestValidators();
        }

        private void InitializeViewModelServices()
        {
            ServiceLocator.Register<IQuestViewModelFactory>(() => new QuestViewModelFactory(
                ServiceLocator.Resolve<IQuestFactory>()));

            IQuestListModel activeListModel = new QuestListModelTopChildrenPredicate(
                ServiceLocator.Resolve<IQuestTree>(),
                q => q.State == State.Progress);

            activeListModel.Initialize();

            ServiceLocator.Register(() => new ActiveQuestListViewModel(
                activeListModel,
                ServiceLocator.Resolve<IStateCommandsFactory>(), 
                ServiceLocator.Resolve<ITreeCommandsFactory>()));

            IQuestListModel resultsListModel = new QuestListModelTopChildrenPredicate(
                ServiceLocator.Resolve<IQuestTree>(),
                q => q.State == State.Done || q.State == State.Failed);
            resultsListModel.Initialize();

            ServiceLocator.Register(() => new ResultsQuestListViewModel(
                resultsListModel,
                ServiceLocator.Resolve<IStateCommandsFactory>(),
                ServiceLocator.Resolve<ITreeCommandsFactory>()));

            IQuestListModel availableListModel = new QuestListModelTopChildrenPredicate(
                ServiceLocator.Resolve<IQuestTree>(),
                q => q.State == State.Idle);
            availableListModel.Initialize();

            ServiceLocator.Register(() => new AvailableQuestListViewModel(
                availableListModel,
                ServiceLocator.Resolve<IStateCommandsFactory>(),
                ServiceLocator.Resolve<ITreeCommandsFactory>()));

            ServiceLocator.Register(() => new QuestCreateViewModel(
                ServiceLocator.Resolve<IQuestViewModelFactory>(),
                ServiceLocator.Resolve<IQuestValidator<ClarifiedResponse<int>>>(),
                ServiceLocator.Resolve<IQuestTree>(),
                ServiceLocator.Resolve<ITreeCommandsFactory>()), useLikeFactory: true);

            ServiceLocator.Register(() => new QuestEditViewModel(
                ServiceLocator.Resolve<IQuestViewModelFactory>(),
                ServiceLocator.Resolve<IQuestValidator<ClarifiedResponse<int>>>(),
                ServiceLocator.Resolve<IQuestTree>(),
                ServiceLocator.Resolve<ITreeCommandsFactory>()), useLikeFactory: true);
        }

        private void InitializeApplicationServices()
        {
            ServiceLocator.Register(() => new DateTimeStateHandler(), useLikeFactory: true);
            ServiceLocator.Register<IEntityStateHandler<IList<ClarifiedError<int>>>>(
                () => new ListOfClarifiedErrorIntStateHandler());
            ServiceLocator.Register(
                () => new QuestViewModelStateHandler(
                    ServiceLocator.Resolve<IEntityStateHandler<DateTime?>>()));
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