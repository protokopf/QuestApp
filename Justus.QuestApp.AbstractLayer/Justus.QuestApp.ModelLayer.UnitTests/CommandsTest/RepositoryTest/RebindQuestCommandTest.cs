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
    class RebindQuestCommandTest
    {
        [Test]
        public void InitializeFailRepositoryNullTest()
        {
            //Arrange
            Quest toAdd = QuestHelper.CreateQuest();
            Quest parent = QuestHelper.CreateQuest();
            Quest oldParent = QuestHelper.CreateQuest();
            IQuestRepository repository = null;

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new RebindQuestCommand(repository, toAdd, parent, oldParent));

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
            Quest oldParent = QuestHelper.CreateQuest();
            IQuestRepository repository = MockRepository.GenerateMock<IQuestRepository>();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new RebindQuestCommand(repository, toAdd, parent, oldParent));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("questToBind", ex.ParamName);
        }

        [Test]
        public void InitializeFailParentNullTest()
        {
            //Arrange
            Quest toAdd = QuestHelper.CreateQuest();
            Quest parent = null;
            Quest oldParent = QuestHelper.CreateQuest();
            IQuestRepository repository = MockRepository.GenerateMock<IQuestRepository>();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new RebindQuestCommand(repository, toAdd, parent, oldParent));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("newParent", ex.ParamName);
        }

        [Test]
        public void InitializeFailOldParentNullTest()
        {
            //Arrange
            Quest toAdd = QuestHelper.CreateQuest();
            Quest oldParent = null;
            Quest parent = QuestHelper.CreateQuest();
            IQuestRepository repository = MockRepository.GenerateMock<IQuestRepository>();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new RebindQuestCommand(repository, toAdd, parent, oldParent));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("oldParent", ex.ParamName);
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

            Quest newParent = repositoryCache[0];
            Quest toRebind = repositoryCache[0].Children[0].Children[0];
            Quest oldParent = toRebind.Parent;

            repository.Expect(rep => rep.Update(null)).IgnoreArguments().Repeat.Once();

            Command command = new RebindQuestCommand(repository, toRebind, newParent, oldParent);

            //Act
            command.Execute();

            //Assert
            Assert.AreEqual(beforeCommandlength, QuestHelper.CountSubQuests(repositoryCache));
            Assert.AreEqual(newParent, toRebind.Parent);
            Assert.Contains(toRebind, newParent.Children);

            Assert.AreNotEqual(oldParent, toRebind.Parent);
            Assert.IsFalse(oldParent.Children.Contains(toRebind));

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

            Quest newParent = repositoryCache[0];
            Quest toRebind = repositoryCache[0].Children[0].Children[0];
            Quest oldParent = toRebind.Parent;

            repository.Expect(rep => rep.Update(null)).IgnoreArguments().Repeat.Once();
            repository.Expect(rep => rep.RevertUpdate(null)).IgnoreArguments().Return(true).Repeat.Once();

            Command command = new RebindQuestCommand(repository, toRebind, newParent, oldParent);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.AreEqual(beforeCommandlength, QuestHelper.CountSubQuests(repositoryCache));
            Assert.AreNotEqual(newParent, toRebind.Parent);
            Assert.IsFalse(newParent.Children.Contains(toRebind));

            Assert.AreEqual(oldParent, toRebind.Parent);
            Assert.IsTrue(oldParent.Children.Contains(toRebind));

            repository.VerifyAllExpectations();
        }
    }
}
