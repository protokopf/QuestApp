﻿using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Repository;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.RepositoryTest
{
    [TestFixture]
    class AddQuestCommandTest
    {
        [Test]
        public void InitializeFailRepositoryNullTest()
        {
            //Arrange
            Quest toAdd = QuestHelper.CreateQuest();
            Quest parent = QuestHelper.CreateQuest();
            IQuestRepository repository = null;

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new AddQuestCommand(repository, parent, toAdd));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("repository", ex.ParamName);
        }

        [Test]
        public void InitializeFailQuestNullTest()
        {
            //Arrange
            Quest toAdd = null;
            Quest parent = QuestHelper.CreateQuest();
            IQuestRepository repository = MockRepository.GenerateMock<IQuestRepository>();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new AddQuestCommand(repository, parent, toAdd));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("childToAdd", ex.ParamName);
        }

        [Test]
        public void InitializeNotFailParentNullTest()
        {
            //Arrange
            Quest toAdd = QuestHelper.CreateQuest();
            Quest parent = null;
            IQuestRepository repository = MockRepository.GenerateMock<IQuestRepository>();

            //Act && Assert
            Assert.DoesNotThrow(() => new AddQuestCommand(repository, parent, toAdd));
        }

        [Test]
        public void ExecuteTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            List<Quest> repositoryCache = new List<Quest>()
            {
                QuestHelper.CreateCompositeQuest(2,3,QuestState.Progress)
            };
            int beforeCommandlength = QuestHelper.CountSubQuests(repositoryCache);

            Quest parent = repositoryCache[0].Children[0].Children[0];

            Quest toAdd = QuestHelper.CreateQuest(100);
            int addedId = toAdd.Id;

            repository.Expect(rep => rep.Insert(null)).IgnoreArguments().Repeat.Once();

            Command command = new AddQuestCommand(repository,parent, toAdd);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(beforeCommandlength + 1, QuestHelper.CountSubQuests(repositoryCache));          
            Assert.AreEqual(parent, toAdd.Parent);
            Assert.Contains(toAdd, parent.Children);

            Assert.IsTrue(QuestHelper.CheckThatAnyQuestFromHierarchyMatchPredicate(repositoryCache, q => q.Id == addedId));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteParentNullTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();

            Quest toAdd = QuestHelper.CreateQuest(100);
            int addedId = toAdd.Id;

            repository.Expect(rep => rep.Insert(null)).IgnoreArguments().Repeat.Once();

            Command command = new AddQuestCommand(repository, null, toAdd);

            //Act
            command.Execute();

            //Assert
            Quest quest = repository.GetArgumentsForCallsMadeOn(r => r.Insert(Arg<Quest>.Is.Anything))[0][0] as Quest;

            Assert.IsNotNull(quest);
            Assert.AreEqual(addedId, quest.Id);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            List<Quest> repositoryCache = new List<Quest>()
            {
                QuestHelper.CreateCompositeQuest(2,3,QuestState.Progress)
            };
            int beforeCommandlength = QuestHelper.CountSubQuests(repositoryCache);

            Quest parent = repositoryCache[0].Children[0].Children[0];

            Quest toAdd = QuestHelper.CreateQuest(100);
            int addedId = toAdd.Id;

            repository.Expect(rep => rep.Insert(null)).IgnoreArguments().Repeat.Once();
            repository.Expect(rep => rep.RevertInsert(null)).IgnoreArguments().Return(true).Repeat.Once();

            Command command = new AddQuestCommand(repository, parent, toAdd);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(beforeCommandlength , QuestHelper.CountSubQuests(repositoryCache));
            Assert.AreEqual(null, toAdd.Parent);
            Assert.IsFalse(parent.Children.Contains(toAdd));

            Assert.IsFalse(QuestHelper.CheckThatAnyQuestFromHierarchyMatchPredicate(repositoryCache, q => q.Id == addedId));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoParentNullTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();

            Quest toAdd = QuestHelper.CreateQuest(100);
            int addedId = toAdd.Id;

            repository.Expect(rep => rep.Insert(null)).IgnoreArguments().Repeat.Once();
            repository.Expect(rep => rep.RevertInsert(null)).IgnoreArguments().Repeat.Once().Return(true);

            Command command = new AddQuestCommand(repository, null, toAdd);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Quest insetedQuest = repository.GetArgumentsForCallsMadeOn(r => r.Insert(Arg<Quest>.Is.Anything))[0][0] as Quest;
            Quest revertedQuest = repository.GetArgumentsForCallsMadeOn(r => r.RevertInsert(Arg<Quest>.Is.Anything))[0][0] as Quest;

            Assert.IsNotNull(insetedQuest);
            Assert.IsNotNull(revertedQuest);

            Assert.AreEqual(addedId, insetedQuest.Id);
            Assert.AreEqual(insetedQuest, revertedQuest);

            repository.VerifyAllExpectations();
        }
    }
}
