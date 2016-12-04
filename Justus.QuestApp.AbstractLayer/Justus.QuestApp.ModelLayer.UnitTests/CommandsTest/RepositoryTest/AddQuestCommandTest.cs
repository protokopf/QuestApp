using Justus.QuestApp.AbstractLayer.Commands;
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
            Quest toDelete = QuestHelper.CreateQuest();
            IQuestRepository repository = null;

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new AddQuestCommand(repository, toDelete));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("repository", ex.ParamName);
        }

        [Test]
        public void InitializeFailQuestNullTest()
        {
            //Arrange
            Quest toDelete = null;
            IQuestRepository repository = MockRepository.GenerateMock<IQuestRepository>();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new AddQuestCommand(repository, toDelete));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("questToAdd", ex.ParamName);
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

            Quest toAdd = QuestHelper.CreateQuest(100);
            int addedId = toAdd.Id;

            repository.Expect(rep => rep.Insert(null)).IgnoreArguments().Repeat.Once();
            repository.Expect(rep => rep.GetAll()).IgnoreArguments().Return(repositoryCache).Repeat.Once();

            Command command = new AddQuestCommand(repository, toAdd);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(beforeCommandlength + 1, QuestHelper.CountSubQuests(repositoryCache));

            Assert.IsTrue(QuestHelper.CheckThatAnyQuestFromHierarchyMatchPredicate(repositoryCache, q => q.Id == addedId));

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
            int beforeCommandLength = QuestHelper.CountSubQuests(repositoryCache);

            Quest toAdd = QuestHelper.CreateQuest(100);
            int addedId = toAdd.Id;

            repository.Expect(rep => rep.Insert(null)).IgnoreArguments().Repeat.Once();
            repository.Expect(rep => rep.RevertInsert(null)).IgnoreArguments().Return(true).Repeat.Once();
            repository.Expect(rep => rep.GetAll()).IgnoreArguments().Return(repositoryCache).Repeat.Twice();

            Command command = new AddQuestCommand(repository, toAdd);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(beforeCommandLength, QuestHelper.CountSubQuests(repositoryCache));

            Assert.IsFalse(QuestHelper.CheckThatAnyQuestFromHierarchyMatchPredicate(repositoryCache, q => q.Id == addedId));

            repository.VerifyAllExpectations();
        }
    }
}
