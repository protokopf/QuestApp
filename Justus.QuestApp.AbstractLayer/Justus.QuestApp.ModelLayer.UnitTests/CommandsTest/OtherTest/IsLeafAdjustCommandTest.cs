using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Other;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.OtherTest
{
    [TestFixture]
    class IsLeafAdjustCommandTest
    {
        [Test]
        public void CtorNullTest()
        {
            //Arrange

            //Act
            ArgumentNullException targetEx =
                Assert.Throws<ArgumentNullException>(() => new IsLeafAdjustCommand(null));

            //Assert
            Assert.IsNotNull(targetEx);

            Assert.AreEqual("quest", targetEx.ParamName);
        }

        [Test]
        public void ExecuteTargetChildrenNullTest()
        {
            //Arrange
            Quest target = QuestHelper.CreateQuest();
            target.Children = null;

            IsLeafAdjustCommand command = new IsLeafAdjustCommand(target);

            //Act
            target.IsLeaf = true;
            bool result = command.Execute();

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(target.IsLeaf);
        }

        [Test]
        public void ExecuteTargetChildrenEmptyTest()
        {
            //Arrange
            Quest target = QuestHelper.CreateQuest();
            target.Children = new List<Quest>();

            IsLeafAdjustCommand command = new IsLeafAdjustCommand(target);

            //Act
            bool result = command.Execute();

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(target.IsLeaf);
        }

        [Test]
        public void ExecuteTargetHaveChildrenTest()
        {
            //Arrange
            Quest target = QuestHelper.CreateQuest();
            target.Children = new List<Quest>
            {
                QuestHelper.CreateQuest()
            };

            IsLeafAdjustCommand command = new IsLeafAdjustCommand(target);

            //Act
            bool result = command.Execute();

            //Assert
            Assert.IsTrue(result);
            Assert.IsFalse(target.IsLeaf);
        }

        [Test]
        public void UndoTest()
        {
            //Arrange
            Quest target = QuestHelper.CreateQuest();
            target.IsLeaf = false;
            target.Children = null;

            IsLeafAdjustCommand command = new IsLeafAdjustCommand(target);

            //Act         
            bool executeResult = command.Execute();
            bool isLeafAfterExecute = target.IsLeaf;
            bool undoResult = command.Undo();
            bool isLeafAfterUndo = target.IsLeaf;

            //Assert
            Assert.IsTrue(executeResult);
            Assert.IsTrue(isLeafAfterExecute);
            Assert.IsTrue(undoResult);
            Assert.IsFalse(isLeafAfterUndo);
        }

        [Test]
        public void IsValidTest()
        {
            //Arrange
            Quest target = QuestHelper.CreateQuest();

            IsLeafAdjustCommand command = new IsLeafAdjustCommand(target);

            //Act
            bool result = command.IsValid();

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            Quest target = QuestHelper.CreateQuest();

            IsLeafAdjustCommand command = new IsLeafAdjustCommand(target);

            //Act
            bool result = command.Commit();

            //Assert
            Assert.IsTrue(result);
        }
    }
}
