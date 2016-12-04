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
    class DeleteQuestCommandTest
    {
        [Test]
        public void InitializeFailRepositoryNullTest()
        {
            //Arrange
            Quest toDelete = QuestHelper.CreateQuest();
            IQuestRepository repository = null;

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new DeleteQuestCommand(repository, toDelete));

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
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new DeleteQuestCommand(repository, toDelete));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("questToDelete", ex.ParamName);
        }

        [Test]
        public void IsValidSuccessfulTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            List<Quest> repositoryCache = new List<Quest>()
            {
                QuestHelper.CreateCompositeQuest(2,3,QuestState.Progress)
            };
            int beforeCommandlength = QuestHelper.CountSubQuests(repositoryCache);

            Quest toDelete = repositoryCache[0].Children[0].Children[0];

            repository.Expect(rep => rep.GetAll()).Return(repositoryCache).Repeat.Once();

            Command command = new DeleteQuestCommand(repository, toDelete);

            //Act
            bool isValid = command.IsValid();

            //Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(beforeCommandlength, QuestHelper.CountSubQuests(repositoryCache));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void IsValidFailedTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            List<Quest> repositoryCache = new List<Quest>()
            {
                QuestHelper.CreateCompositeQuest(2,3,QuestState.Progress)
            };
            int length = QuestHelper.CountSubQuests(repositoryCache);

            Quest toDelete = QuestHelper.CreateQuest();

            repository.Expect(rep => rep.GetAll()).Return(repositoryCache).Repeat.Once();

            Command command = new DeleteQuestCommand(repository, toDelete);

            //Act
            bool isValid = command.IsValid();

            //Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(length, QuestHelper.CountSubQuests(repositoryCache));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteAfterIsValidTrueTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            List<Quest> repositoryCache = new List<Quest>()
            {
                QuestHelper.CreateCompositeQuest(2,3,QuestState.Progress)
            };
            int beforeCommandlength = QuestHelper.CountSubQuests(repositoryCache);

            Quest toDelete = repositoryCache[0].Children[0];
            int deletedId = toDelete.Id;

            repository.Expect(rep => rep.GetAll()).Return(repositoryCache).Repeat.Once();
            repository.Expect(rep => rep.Delete(null)).IgnoreArguments().Repeat.Once();

            Command command = new DeleteQuestCommand(repository, toDelete);

            //Act
            bool isValid = command.IsValid();
            command.Execute();

            //Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(beforeCommandlength - 4, QuestHelper.CountSubQuests(repositoryCache));

            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(repositoryCache, x => x.Parent != toDelete));
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(repositoryCache, x => !x.Children.Contains(toDelete)));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteAfterIsValidFalseTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            List<Quest> repositoryCache = new List<Quest>()
            {
                QuestHelper.CreateCompositeQuest(2,3,QuestState.Progress)
            };
            int beforeCommandlength = QuestHelper.CountSubQuests(repositoryCache);

            Quest toDelete = QuestHelper.CreateQuest();

            repository.Expect(rep => rep.GetAll()).Return(repositoryCache).Repeat.Once();
            repository.Expect(rep => rep.Delete(null)).IgnoreArguments().Repeat.Never();

            Command command = new DeleteQuestCommand(repository, toDelete);

            //Act
            bool isValid = command.IsValid();
            command.Execute();

            //Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(beforeCommandlength, QuestHelper.CountSubQuests(repositoryCache));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteWithoutIsValidTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            List<Quest> repositoryCache = new List<Quest>()
            {
                QuestHelper.CreateCompositeQuest(2,3,QuestState.Progress)
            };
            int beforeCommandlength = QuestHelper.CountSubQuests(repositoryCache);

            Quest toDelete = repositoryCache[0].Children[0];
            int deletedId = toDelete.Id;

            repository.Expect(rep => rep.GetAll()).Return(repositoryCache).Repeat.Once();
            repository.Expect(rep => rep.Delete(null)).IgnoreArguments().Repeat.Once();

            Command command = new DeleteQuestCommand(repository, toDelete);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(beforeCommandlength - 4, QuestHelper.CountSubQuests(repositoryCache));

            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(repositoryCache, x => x.Parent != toDelete));
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(repositoryCache, x => !x.Children.Contains(toDelete)));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteDeleteTopLevelQuestTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            List<Quest> repositoryCache = new List<Quest>()
            {
                QuestHelper.CreateCompositeQuest(2,3,QuestState.Progress)
            };
            int beforeCommandlength = QuestHelper.CountSubQuests(repositoryCache);

            Quest toDelete = repositoryCache[0];
            int deletedId = toDelete.Id;

            repository.Expect(rep => rep.GetAll()).Return(repositoryCache).Repeat.Once();
            repository.Expect(rep => rep.Delete(null)).IgnoreArguments().Repeat.Once();

            Command command = new DeleteQuestCommand(repository, toDelete);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(0, QuestHelper.CountSubQuests(repositoryCache));

            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(repositoryCache, x => x.Parent != toDelete));
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(repositoryCache, x => !x.Children.Contains(toDelete)));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoAfterExecutionTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            List<Quest> repositoryCache = new List<Quest>()
            {
                QuestHelper.CreateCompositeQuest(2,3,QuestState.Progress)
            };
            int beforeCommandlength = QuestHelper.CountSubQuests(repositoryCache);

            Quest toDelete = repositoryCache[0].Children[0];
            int deletedId = toDelete.Id;

            repository.Expect(rep => rep.GetAll()).Return(repositoryCache).Repeat.Once();
            repository.Expect(rep => rep.Delete(null)).IgnoreArguments().Repeat.Once();
            repository.Expect(rep => rep.RevertDelete(null)).IgnoreArguments().Return(true).Repeat.Once();

            Command command = new DeleteQuestCommand(repository, toDelete);

            //Act
            bool isValid = command.IsValid();
            command.Execute();
            command.Undo();

            //Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(beforeCommandlength, QuestHelper.CountSubQuests(repositoryCache));

            Assert.IsTrue(QuestHelper.CheckThatAnyQuestFromHierarchyMatchPredicate(repositoryCache, q => q.Id == deletedId));
            Assert.IsTrue(QuestHelper.CheckThatAnyQuestFromHierarchyMatchPredicate(repositoryCache, q => q.Children.Contains(toDelete)));

            repository.VerifyAllExpectations();
        }

        [Test]
        public void UndoWithoutExecutionTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();
            List<Quest> repositoryCache = new List<Quest>()
            {
                QuestHelper.CreateCompositeQuest(2,3,QuestState.Progress)
            };
            int beforeCommandlength = QuestHelper.CountSubQuests(repositoryCache);

            Quest toDelete = repositoryCache[0].Children[0];
            int deletedId = toDelete.Id;

            repository.Expect(rep => rep.GetAll()).Return(repositoryCache).Repeat.Never();
            repository.Expect(rep => rep.Delete(null)).IgnoreArguments().Repeat.Never();
            repository.Expect(rep => rep.RevertDelete(null)).IgnoreArguments().Return(true).Repeat.Never();

            Command command = new DeleteQuestCommand(repository, toDelete);

            //Act
            command.Undo();

            //Assert
            Assert.AreEqual(beforeCommandlength, QuestHelper.CountSubQuests(repositoryCache));

            Assert.IsTrue(QuestHelper.CheckThatAnyQuestFromHierarchyMatchPredicate(repositoryCache, q => q.Id == deletedId));
            Assert.IsTrue(QuestHelper.CheckThatAnyQuestFromHierarchyMatchPredicate(repositoryCache, q => q.Children.Contains(toDelete)));

            repository.VerifyAllExpectations();
        }
    }
}
