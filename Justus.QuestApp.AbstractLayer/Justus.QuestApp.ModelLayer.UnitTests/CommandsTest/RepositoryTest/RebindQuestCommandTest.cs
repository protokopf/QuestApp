using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Repository;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.RepositoryTest
{
    [TestFixture]
    class RebindQuestCommandTest
    {
        [Test]
        public void InitializeFailQuestNullTest()
        {
            //Arrange
            Quest toAdd = null;
            Quest parent = QuestHelper.CreateQuest();
            Quest oldParent = QuestHelper.CreateQuest();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new RebindQuestCommand(toAdd, parent, oldParent));

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

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new RebindQuestCommand(toAdd, parent, oldParent));

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

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new RebindQuestCommand(toAdd, parent, oldParent));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("oldParent", ex.ParamName);
        }

        [Test]
        public void ExecuteTest()
        {
            //Arrange
            int oldParentId = 12;
            int newParentId = 42;
            int rebindId = 144;

            Quest oldParent = new Quest() {Id = oldParentId, Children = new List<Quest>()};

            Quest newParent = new Quest() {Id = newParentId, Children = new List<Quest>()};
            Quest toRebind = new Quest() {Id = rebindId, Parent = oldParent, ParentId = oldParentId};

            oldParent.Children.Add(toRebind);
            

            ICommand command = new RebindQuestCommand(toRebind, newParent, oldParent);

            //Act
            command.Execute();

            //Assert
            Assert.IsFalse(oldParent.Children.Contains(toRebind));
            Assert.IsFalse(toRebind.Parent == oldParent);
            Assert.IsFalse(toRebind.ParentId == oldParentId);

            Assert.IsTrue(newParent.Children.Contains(toRebind));
            Assert.IsTrue(toRebind.Parent == newParent);
            Assert.IsTrue(toRebind.ParentId == newParentId);
        }

        [Test]
        public void UndoTest()
        {
            //Arrange
            int oldParentId = 12;
            int newParentId = 42;
            int rebindId = 144;

            Quest oldParent = new Quest() { Id = oldParentId, Children = new List<Quest>() };

            Quest newParent = new Quest() { Id = newParentId, Children = new List<Quest>() };
            Quest toRebind = new Quest() { Id = rebindId, Parent = oldParent, ParentId = oldParentId };

            oldParent.Children.Add(toRebind);
            ICommand command = new RebindQuestCommand(toRebind, newParent, oldParent);

            //Act
            command.Execute();
            command.Undo();

            //Assert
            Assert.IsTrue(oldParent.Children.Contains(toRebind));
            Assert.IsTrue(toRebind.Parent == oldParent);
            Assert.IsTrue(toRebind.ParentId == oldParentId);

            Assert.IsFalse(newParent.Children.Contains(toRebind));
            Assert.IsFalse(toRebind.Parent == newParent);
            Assert.IsFalse(toRebind.ParentId == newParentId);
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            Quest oldParent = new Quest();
            Quest newParent = new Quest();
            Quest toRebind = new Quest();

            ICommand command = new RebindQuestCommand(toRebind, newParent, oldParent);

            //Act
            bool result = command.Commit();

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidTest()
        {
            //Arrange
            Quest oldParent = new Quest();
            Quest newParent = new Quest();
            Quest toRebind = new Quest();

            ICommand command = new RebindQuestCommand(toRebind, newParent, oldParent);

            //Act
            bool result = command.IsValid();

            //Assert
            Assert.IsTrue(result);
        }
    }
}
