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
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest
{
    [TestFixture]
    class QuestListViewModelTest
    {
        [Test]
        public void QuestsListTitleWhenCurrentQuestIsNullTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(repository, stateCommands, repoCommands);

            //Act
            string questsListTitle = viewModel.QuestsListTitle;

            //Assert
            Assert.IsNull(questsListTitle);
        }

        [Test]
        public void QuestsListTitleWhenCurrentQuestWithoutParentTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(repository, stateCommands, repoCommands)
            {
                Root = new FakeQuest {Title = "fakeTitleOfFakeQuest"}
            };

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
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(repository, stateCommands, repoCommands);
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
            repository.Expect(rep => rep.GetAll(Arg<Predicate<Quest>>.Is.NotNull)).Repeat.Once().Return(null);
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(repository, stateCommands, repoCommands);

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
            repository.Expect(rep => rep.GetAll(Arg<Predicate<Quest>>.Is.NotNull)).Repeat.Never().Return(null);
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(repository, stateCommands, repoCommands);
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
            repository.Expect(rep => rep.GetAll(Arg<Predicate<Quest>>.Is.NotNull)).Repeat.Twice().Return(new List<Quest>());
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(repository, stateCommands, repoCommands);

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
            repository.Expect(rep => rep.GetAll(Arg<Predicate<Quest>>.Is.NotNull)).Repeat.Once().Return(new List<Quest>());
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(repository, stateCommands, repoCommands);


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
            repository.Expect(rep => rep.GetAll(Arg<Predicate<Quest>>.Is.NotNull)).Repeat.Once().Return(list);
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();

            QuestListViewModel viewModel = new QuestListViewModel(repository, stateCommands, repoCommands);

            //Act
            List<Quest> quests = viewModel.Leaves;

            //Assert
            Assert.AreEqual(list, quests);
            repository.VerifyAllExpectations();
        }
    }
}
