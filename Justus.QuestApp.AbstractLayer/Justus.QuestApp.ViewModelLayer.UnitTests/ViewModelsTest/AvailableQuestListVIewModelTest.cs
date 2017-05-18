﻿using System;
using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
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
    class AvailableQuestListVIewModelTest
    {
        [Test]
        public void FilterOnTopLevelQuests()
        {
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();

            List<Quest> fromRepository = new List<Quest>
            {
                new FakeQuest {CurrentState = QuestState.Done , Parent = null},
                new FakeQuest {CurrentState = QuestState.Failed , Parent = null},
                new FakeQuest {CurrentState = QuestState.Idle , Parent = null},
                new FakeQuest {CurrentState = QuestState.Progress , Parent = null}
            };

            repository.Expect(rep => rep.GetAll(Arg<Predicate<Quest>>.Is.NotNull)).Repeat.Once().Return(fromRepository);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            IRepositoryCommandsFactory repoCommands = MockRepository.GenerateStrictMock<IRepositoryCommandsFactory>();

            AvailableQuestListViewModel viewModel = new AvailableQuestListViewModel(repository, stateCommands, repoCommands);


            //Act
            List<Quest> quests = viewModel.Leaves;

            //Assert
            Assert.IsNotNull(quests);
            Assert.AreEqual(1, quests.Count);
            Assert.IsTrue(quests.Any(q => q.CurrentState == QuestState.Idle));

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

            AvailableQuestListViewModel viewModel = new AvailableQuestListViewModel(repository, stateCommands, repoCommands);

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
    }
}
