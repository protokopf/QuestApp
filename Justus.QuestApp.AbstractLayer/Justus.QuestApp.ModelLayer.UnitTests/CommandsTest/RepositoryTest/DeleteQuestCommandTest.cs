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
        public void ExecuteTest()
        {
            //Arrange
            IQuestRepository repository = MockRepository.GenerateStrictMock<IQuestRepository>();

            Quest toDelete = QuestHelper.CreateQuest(42);

            repository.Expect(rep => rep.Delete(null)).IgnoreArguments().Repeat.Once();

            Command command = new DeleteQuestCommand(repository, toDelete);

            //Act
            command.Execute();

            //Assert
            Quest deleted =
                repository.GetArgumentsForCallsMadeOn(rep => rep.Delete(Arg<Quest>.Is.Anything))[0][0] as Quest;
            Assert.IsNotNull(deleted);
            Assert.AreEqual(toDelete, deleted);

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

            repository.Expect(rep => rep.Delete(null)).IgnoreArguments().Repeat.Once();
            repository.Expect(rep => rep.RevertDelete(null)).IgnoreArguments().Return(true).Repeat.Once();

            Command command = new DeleteQuestCommand(repository, toDelete);

            //Act
            command.Execute();
            command.Undo();

            //Assert
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

            repository.Expect(rep => rep.GetAll(null)).IgnoreArguments().Return(repositoryCache).Repeat.Never();
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
