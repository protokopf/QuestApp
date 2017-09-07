using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Property;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestCommandsTest.Property
{
    [TestFixture]
    class IsLeafQuestCommandTests
    {
        [Test]
        public void ExecuteWithoutChildrenTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();
            quest.IsLeaf = false;

            IsLeafAdjustQuestCommand command = new IsLeafAdjustQuestCommand();

            //Act
            bool result = command.Execute(quest);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(quest.IsLeaf);

        }

        [Test]
        public void ExecuteWithChildrenTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();
            quest.IsLeaf = true;
            quest.Children = new List<Quest> {QuestHelper.CreateQuest()};

            IsLeafAdjustQuestCommand command = new IsLeafAdjustQuestCommand();

            //Act
            bool result = command.Execute(quest);

            //Assert
            Assert.IsTrue(result);
            Assert.IsFalse(quest.IsLeaf);
        }

        [Test]
        public void UndoTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();
            quest.IsLeaf = true;
            quest.Children = new List<Quest> { QuestHelper.CreateQuest() };

            IsLeafAdjustQuestCommand command = new IsLeafAdjustQuestCommand();

            //Act
            bool result = command.Execute(quest);
            bool undoResult = command.Undo(quest);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(undoResult);
            Assert.IsTrue(quest.IsLeaf);
        }
    }
}
