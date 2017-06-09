using System;
using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using Justus.QuestApp.ViewModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest
{
    [TestFixture]
    class ActiveQuestListViewModelTest
    {

        #region Helper methods

        private void HandleQuests(List<Quest> quests, Action<Quest> childrenHandler)
        {
            foreach (Quest child in quests)
            {
                childrenHandler(child);
            }
        }

        #endregion

        [Test]
        public void FilterQuest()
        {
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();

            List<Quest> fromRepository = new List<Quest>()
            {
                new FakeQuest {CurrentState = QuestState.Done},
                new FakeQuest {CurrentState = QuestState.Failed},
                new FakeQuest {CurrentState = QuestState.Idle},
                new FakeQuest {CurrentState = QuestState.Progress}
            };

            repository.Expect(rep => rep.GetAll(Arg<Predicate<Quest>>.Is.NotNull)).Repeat.Once().Return(fromRepository);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();


            QuestListViewModel viewModel = new ActiveQuestListViewModel(repository, stateCommands, repoCommands);

            //Act
            List<Quest> quests = viewModel.Leaves;

            //Assert
            Assert.IsNotNull(quests);
            Assert.AreEqual(1, quests.Count);
            Assert.AreEqual(QuestState.Progress, quests[0].CurrentState);

            Assert.AreEqual(4, fromRepository.Count);
            Assert.IsTrue(fromRepository.Any(quest => quest.CurrentState == QuestState.Done));
            Assert.IsTrue(fromRepository.Any(quest => quest.CurrentState == QuestState.Failed));
            Assert.IsTrue(fromRepository.Any(quest => quest.CurrentState == QuestState.Idle));
            Assert.IsTrue(fromRepository.Any(quest => quest.CurrentState == QuestState.Progress));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void FilterSubQuests()
        {
            //Arrange
            Quest parent = new FakeQuest();
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();

            List<Quest> fromRepository = new List<Quest>
            {
                new FakeQuest {CurrentState = QuestState.Done , Parent = parent},
                new FakeQuest {CurrentState = QuestState.Failed , Parent = parent},
                new FakeQuest {CurrentState = QuestState.Idle , Parent = parent},
                new FakeQuest {CurrentState = QuestState.Progress , Parent = parent}
            };

            repository.Expect(rep => rep.GetAll(Arg<Predicate<Quest>>.Is.NotNull)).Repeat.Once().Return(fromRepository);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();


            QuestListViewModel viewModel = new ActiveQuestListViewModel(repository, stateCommands, repoCommands);

            //Act
            List<Quest> quests = viewModel.Leaves;

            //Assert
            Assert.IsNotNull(quests);
            Assert.AreEqual(4, quests.Count);
            Assert.IsTrue(quests.Any(q => q.CurrentState == QuestState.Done));
            Assert.IsTrue(quests.Any(q => q.CurrentState == QuestState.Failed));
            Assert.IsTrue(quests.Any(q => q.CurrentState == QuestState.Idle));
            Assert.IsTrue(quests.Any(q => q.CurrentState == QuestState.Progress));

            Assert.AreEqual(4, fromRepository.Count);
            Assert.IsTrue(fromRepository.Any(quest => quest.CurrentState == QuestState.Done));
            Assert.IsTrue(fromRepository.Any(quest => quest.CurrentState == QuestState.Failed));
            Assert.IsTrue(fromRepository.Any(quest => quest.CurrentState == QuestState.Idle));
            Assert.IsTrue(fromRepository.Any(quest => quest.CurrentState == QuestState.Progress));

            repository.VerifyAllExpectations();
        }

        [TestCase(0)]
        [TestCase(0.33333)]
        [TestCase(0.5)]
        [TestCase(0.75)]
        [TestCase(1)]
        public void CountProgressTest(double progress)
        {
            //Arrange
            int expected = (int)Math.Floor(progress * 100);

            Quest quest = new Quest()
            {
                Progress = progress
            };

            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(repository, stateCommands, repoCommands);


            //Act
            int progressInt = viewModel.CountProgress(quest);

            //Assert
            Assert.AreEqual(expected, progressInt);
        }


        [TestCase(1.1)]
        [TestCase(2.1)]
        public void CountProgressMoreThanOne(double progress)
        {
            //Arrange
            int expected = 100;

            Quest quest = new Quest()
            {
                Progress = progress
            };

            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(repository, stateCommands, repoCommands);


            //Act
            int progressInt = viewModel.CountProgress(quest);

            //Assert
            Assert.AreEqual(expected, progressInt);
        }

        [Test]
        public void CountProgressNullException()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(repository, stateCommands, repoCommands);

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => viewModel.CountProgress(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "quest");
        }

        [Test]
        public void IsRootDoneTest()
        {
            //Arrange
            List<Quest> list = new List<Quest>()
            {
                new FakeQuest()
                {
                    CurrentState = QuestState.Progress,
                    Children = new List<Quest>()
                    {
                        new FakeQuest()
                    }
                }
            };

            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            repository.Expect(rep => rep.GetAll(Arg<Predicate<Quest>>.Is.NotNull)).Repeat.Once().Return(list);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();

            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(repository, stateCommands, repoCommands);

            //Act
            viewModel.TraverseToLeaf(0);
            bool isRootDoneBefore = viewModel.IsRootDone();
            viewModel.Root.CurrentState = QuestState.Done;
            bool isRootDoneAfter = viewModel.IsRootDone();

            //Assert
            Assert.IsFalse(isRootDoneBefore);
            Assert.IsTrue(isRootDoneAfter);
        }
    }
}
