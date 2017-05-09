using System;
using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Helpers;
using Justus.QuestApp.ViewModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest
{
    [TestFixture]
    class ActiveQuestListViewModelTest
    {
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

            repository.Expect(rep => rep.GetAll()).Repeat.Once().Return(fromRepository);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();
            IQuestProgressCounter counter = MockRepository.GenerateStrictMock<IQuestProgressCounter>();


            QuestListViewModel viewModel = new ActiveQuestListViewModel(repository, stateCommands, repoCommands, counter);

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

            repository.Expect(rep => rep.GetAll()).Repeat.Once().Return(fromRepository);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();
            IQuestProgressCounter counter = MockRepository.GenerateStrictMock<IQuestProgressCounter>();


            QuestListViewModel viewModel = new ActiveQuestListViewModel(repository, stateCommands, repoCommands, counter);

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

            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(repository, stateCommands, repoCommands, counter);


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
            IQuestProgressCounter counter = MockRepository.GenerateStrictMock<IQuestProgressCounter>();
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(repository, stateCommands, repoCommands, counter);

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => viewModel.CountProgress(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "quest");
        }


    }
}
