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
    class UpdateQuestTest
    {
        [Test]
        public void InitializeFailRepositoryNullTest()
        {
            //Arrange
            Quest toUpdate = QuestHelper.CreateQuest();
            IQuestRepository repository = null;

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new UpdateQuestCommand(repository, toUpdate));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("repository", ex.ParamName);
        }

        [Test]
        public void InitializeFailQuestNullTest()
        {
            //Arrange
            Quest toUpdate = null;
            IQuestRepository repository = MockRepository.GenerateMock<IQuestRepository>();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new UpdateQuestCommand(repository, toUpdate));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("questToUpdate", ex.ParamName);
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

            Quest toUpdate = repositoryCache[0].Children[0];
            toUpdate.Title = "new title";

            repository.Expect(rep => rep.Update(null)).IgnoreArguments().Repeat.Once();

            Command command = new UpdateQuestCommand(repository, toUpdate);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(beforeCommandlength, QuestHelper.CountSubQuests(repositoryCache));

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

            Quest toUpdate = repositoryCache[0].Children[0];
            toUpdate.Title = "new title";

            repository.Expect(rep => rep.Update(null)).IgnoreArguments().Repeat.Once();
            repository.Expect(rep => rep.RevertUpdate(null)).IgnoreArguments().Return(true).Repeat.Once();

            Command command = new UpdateQuestCommand(repository, toUpdate);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(beforeCommandlength, QuestHelper.CountSubQuests(repositoryCache));

            repository.VerifyAllExpectations();
        }
    }
}
