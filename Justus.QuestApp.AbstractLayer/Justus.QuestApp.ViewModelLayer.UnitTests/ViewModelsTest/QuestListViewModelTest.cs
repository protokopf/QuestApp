using Justus.QuestApp.AbstractLayer.Commands;
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
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities;

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

        [SetUp]
        public void SetUp()
        {
            ServiceLocator.Register(() => MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>());
        }


        [Test]
        public void QuestsListTitleWhenCurrentQuestIsNullTest()
        {
            //Arrange
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
            viewModel.Root = new FakeQuest() { Title = "fakeTitleOfFakeQuest" };

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
            viewModel.Root = new FakeQuest() { Title = "child" };
            viewModel.Root.Parent = new FakeQuest() { Title = "parent" };

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

            ServiceLocator.Register(() => repository);
            ServiceLocator.Register(() => comManager);
            ServiceLocator.Register<IQuestProgressCounter>(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register<IStateCommandsFactory>(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();

            //Act
            List<Quest> quests = viewModel.Leaves;

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
            viewModel.Root = new FakeQuest() { Children = new List<Quest>() };

            //Act
            List<Quest> quests = viewModel.Leaves;

            //Assert
            repository.VerifyAllExpectations();
        }

        [Test]
        public void QuestsShouldBePulledAfterFirstCallCurrentChildrenTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(rep => rep.GetAll()).Repeat.Twice().Return(new List<Quest>());

            ServiceLocator.Register<IQuestRepository>(() => repository);
            ServiceLocator.Register<ICommandManager>(() => MockRepository.GenerateStrictMock<ICommandManager>());
            ServiceLocator.Register<IQuestProgressCounter>(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register<IStateCommandsFactory>(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();


            //Act
            var list = viewModel.Leaves;
            viewModel.ResetChildren();
            var secondList = viewModel.Leaves;


            //Assert
            Assert.IsNotNull(list);
            Assert.IsNotNull(secondList);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void CurrentChildrenTwiceEmptyTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(rep => rep.GetAll()).Repeat.Once().Return(new List<Quest>());

            ServiceLocator.Register<IQuestRepository>(() => repository);
            ServiceLocator.Register<ICommandManager>(() => MockRepository.GenerateStrictMock<ICommandManager>());
            ServiceLocator.Register<IQuestProgressCounter>(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register<IStateCommandsFactory>(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();


            //Act
            var list = viewModel.Leaves;
            var secondList = viewModel.Leaves;


            //Assert
            Assert.IsNotNull(list);
            Assert.IsNotNull(secondList);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void CurrentChildrenWHenQuestRepositoryReturnsListTest()
        {
            //Arrange
            List<Quest> list = new List<Quest>()
            {
                new FakeQuest()
                {
                    Id = 42
                }

            };

            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(rep => rep.GetAll()).Repeat.Once().Return(list);

            ICommandManager comManager = MockRepository.GenerateStrictMock<ICommandManager>();

            ServiceLocator.Register(() => repository);
            ServiceLocator.Register(() => comManager);
            ServiceLocator.Register(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();

            //Act
            List<Quest> quests = viewModel.Leaves;

            //Assert
            Assert.AreEqual(list, quests);
            repository.VerifyAllExpectations();
        }
    }
}
