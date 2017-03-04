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
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
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
            repository.Expect(rep => rep.PullQuests()).Repeat.Once();

            ICommandManager comManager = MockRepository.GenerateStrictMock<ICommandManager>();

            ServiceLocator.Register(() => repository);
            ServiceLocator.Register(() => comManager);
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
            repository.Expect(rep => rep.PullQuests()).Repeat.Once();

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

        [Test]
        public void QuestsShouldBePulledAfterFirstCallCurrentChildrenTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(rep => rep.PullQuests()).Repeat.Once();
            repository.Expect(rep => rep.GetAll()).Repeat.Twice().Return(new List<Quest>());

            ServiceLocator.Register<IQuestRepository>(() => repository);
            ServiceLocator.Register<ICommandManager>(() => MockRepository.GenerateStrictMock<ICommandManager>());
            ServiceLocator.Register<IQuestProgressCounter>(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register<IStateCommandsFactory>(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();


            //Act
            var list = viewModel.CurrentChildren;
            viewModel.ResetChildren();
            var secondList = viewModel.CurrentChildren;


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
            repository.Expect(rep => rep.PullQuests()).Repeat.Once();
            repository.Expect(rep => rep.GetAll()).Repeat.Once().Return(new List<Quest>());

            ServiceLocator.Register<IQuestRepository>(() => repository);
            ServiceLocator.Register<ICommandManager>(() => MockRepository.GenerateStrictMock<ICommandManager>());
            ServiceLocator.Register<IQuestProgressCounter>(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register<IStateCommandsFactory>(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();


            //Act
            var list = viewModel.CurrentChildren;
            var secondList = viewModel.CurrentChildren;


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
            repository.Expect(rep => rep.PullQuests()).Repeat.Once();

            ICommandManager comManager = MockRepository.GenerateStrictMock<ICommandManager>();

            ServiceLocator.Register(() => repository);
            ServiceLocator.Register(() => comManager);
            ServiceLocator.Register(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();

            //Act
            List<Quest> quests = viewModel.CurrentChildren;

            //Assert
            Assert.AreEqual(list, quests);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void CountProgressTest()
        {
            //Arrange
            IQuestProgressCounter counter = MockRepository.GenerateStrictMock<IQuestProgressCounter>();
            counter.Expect(c => c.CountProgress(Arg<Quest>.Is.Anything)).Repeat.Once().Return(new ProgressValue()
            {
                Current = 5,
                Total = 10
            });

            ServiceLocator.Register(() => counter);
            ServiceLocator.Register(() => MockRepository.GenerateStrictMock<IQuestRepository>());
            ServiceLocator.Register(() => MockRepository.GenerateStrictMock<ICommandManager>());
            ServiceLocator.Register(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();

            //Act
            int progress = viewModel.CountProgress(new Quest());

            //Assert
            Assert.AreEqual(50, progress);
            counter.VerifyAllExpectations();
        }

        [Test]
        public void CountProgressNullException()
        {
            //Arrange
            ServiceLocator.Register(() => MockRepository.GenerateStrictMock<IQuestProgressCounter>());
            ServiceLocator.Register(() => MockRepository.GenerateStrictMock<IQuestRepository>());
            ServiceLocator.Register(() => MockRepository.GenerateStrictMock<ICommandManager>());
            ServiceLocator.Register(() => MockRepository.GenerateStrictMock<IStateCommandsFactory>());

            QuestListViewModel viewModel = new QuestListViewModel();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => viewModel.CountProgress(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "quest");
        }
    }
}
