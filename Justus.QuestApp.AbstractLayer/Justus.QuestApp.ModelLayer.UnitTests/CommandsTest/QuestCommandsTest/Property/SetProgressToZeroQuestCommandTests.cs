using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Property;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestCommandsTest.Property
{
    [TestFixture]
    class SetProgressToZeroQuestCommandTests
    {
        [Test]
        public void ExecuteTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();
            quest.Progress = 42;

            SetProgressToZeroQuestCommand command = new SetProgressToZeroQuestCommand();

            //Act
            bool result = command.Execute(quest);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, quest.Progress);
        }

        [Test]
        public void UndoTest()
        {
            //Arrange
            double initialProgress = 42;

            Quest quest = QuestHelper.CreateQuest();
            quest.Progress = initialProgress;

            SetProgressToZeroQuestCommand command = new SetProgressToZeroQuestCommand();

            //Act
            bool result = command.Execute(quest);
            bool undoResult = command.Undo(quest);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(undoResult);
            Assert.AreEqual(initialProgress, quest.Progress);
        }
    }
}
