﻿using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.ViewModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using NUnit.Framework;
using Rhino.Mocks; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands.Factories;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest
{
    [TestFixture]
    class QuestListViewModelTest
    {
        [TearDown]
        public void TearDown()
        {
            ServiceLocator.ReleaseAll();
        }


        [Test]
        public void PullQuestsTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(rep => rep.PullQuests()).Repeat.Once();

            ICommandManager comManager = MockRepository.GenerateStrictMock<ICommandManager>();

            ServiceLocator.Register<IQuestRepository>(() => { return repository; });
            ServiceLocator.Register<ICommandManager>(() => { return comManager; });
            ServiceLocator.Register<IQuestProgressCounter>(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register<IStateCommandsFactory>(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();
            int countCallBack = 0;
            viewModel.IsBusyChanged += (sender, args) => { countCallBack++; };

            //Act
            Task t = viewModel.PullQuests();
            t.Wait();

            //Assert
            Assert.AreEqual(2, countCallBack);
            repository.VerifyAllExpectations();
            comManager.VerifyAllExpectations();
        }

        [Test]
        public void PushQuestsTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(rep => rep.PushQuests()).Repeat.Once();

            ICommandManager comManager = MockRepository.GenerateStrictMock<ICommandManager>();

            ServiceLocator.Register<IQuestRepository>(() => { return repository; });
            ServiceLocator.Register<ICommandManager>(() => { return comManager; });
            ServiceLocator.Register<IQuestProgressCounter>(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register<IStateCommandsFactory>(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();
            int countCallBack = 0;
            viewModel.IsBusyChanged += (sender, args) => { countCallBack++; };

            //Act
            Task t = viewModel.PushQuests();
            t.Wait();

            //Assert
            Assert.AreEqual(2, countCallBack);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void QuestsListTitleWhenCurrentQuestIsNullTest()
        {
            //Arrange
            ITaskWrapper wrapper = MockRepository.GenerateStrictMock<ITaskWrapper>();
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            ICommandManager comManager = MockRepository.GenerateStrictMock<ICommandManager>();

            ServiceLocator.Register(() => { return repository; });
            ServiceLocator.Register(() => { return comManager; });
            ServiceLocator.Register<IQuestProgressCounter>(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register<IStateCommandsFactory>(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();

            //Act
            string questsListTitle = viewModel.QuestsListTitle;

            //Assert
            Assert.IsNull(questsListTitle);
        }

        [Test]
        public void QuestsListTitleWhenCurrentQuestWithoutParentTest()
        {
            //Arrange

            ITaskWrapper wrapper = MockRepository.GenerateStrictMock<ITaskWrapper>();
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            ICommandManager comManager = MockRepository.GenerateStrictMock<ICommandManager>();

            ServiceLocator.Register(() => { return repository; });
            ServiceLocator.Register(() => { return comManager; });
            ServiceLocator.Register<IQuestProgressCounter>(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register<IStateCommandsFactory>(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();
            viewModel.CurrentQuest = new FakeQuest() { Title = "fakeTitleOfFakeQuest" };

            //Act
            string questsListTitle = viewModel.QuestsListTitle;

            //Assert
            Assert.IsNotNull(questsListTitle);
            Assert.AreEqual("fakeTitleOfFakeQuest", questsListTitle);
        }

        [Test]
        public void QuestsListTitleWhenCurrentQuestWithParentTest()
        {
            //Arrange
            ITaskWrapper wrapper = MockRepository.GenerateStrictMock<ITaskWrapper>();
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            ICommandManager comManager = MockRepository.GenerateStrictMock<ICommandManager>();

            ServiceLocator.Register(() => { return repository; });
            ServiceLocator.Register(() => { return comManager; });
            ServiceLocator.Register<IQuestProgressCounter>(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register<IStateCommandsFactory>(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();
            viewModel.CurrentQuest = new FakeQuest() { Title = "child" };
            viewModel.CurrentQuest.Parent = new FakeQuest() { Title = "parent" };

            //Act
            string questsListTitle = viewModel.QuestsListTitle;


            //Assert
            Assert.IsNotNull(questsListTitle);
            Assert.AreEqual("child", questsListTitle);
        }

        [Test]
        public void CurrentChildrenWhenCurrentQuestNullTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(rep => rep.GetAll()).Repeat.Once().Return(null);

            ICommandManager comManager = MockRepository.GenerateStrictMock<ICommandManager>();

            ServiceLocator.Register(() => { return repository; });
            ServiceLocator.Register(() => { return comManager; });
            ServiceLocator.Register<IQuestProgressCounter>(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register<IStateCommandsFactory>(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();

            //Act
            List<Quest> quests = viewModel.CurrentChildren;

            //Assert
            repository.VerifyAllExpectations();
        }

        [Test]
        public void CurrentChildrenWhenCurrentQuestNotNullTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(rep => rep.GetAll()).Repeat.Never().Return(null);

            ICommandManager comManager = MockRepository.GenerateStrictMock<ICommandManager>();

            ServiceLocator.Register(() => { return repository; });
            ServiceLocator.Register(() => { return comManager; });
            ServiceLocator.Register<IQuestProgressCounter>(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register<IStateCommandsFactory>(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();
            viewModel.CurrentQuest = new FakeQuest() { Children = new List<Quest>() };

            //Act
            List<Quest> quests = viewModel.CurrentChildren;

            //Assert
            repository.VerifyAllExpectations();
        }
    }
}
