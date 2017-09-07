using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Property;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestCommandsTest.Property
{
    [TestFixture]
    class RecountProgressQuestCommandTests
    {
        [TestCase(State.Progress)]
        [TestCase(State.Done)]
        [TestCase(State.Failed)]
        [TestCase(State.Idle)]
        public void ExecuteWithoutChildrenTest(State state)
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();
            quest.Progress = 0;
            quest.State = state;

            RecountProgressQuestCommand command = new RecountProgressQuestCommand();

            //Act
            bool result = command.Execute(quest);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(state == State.Done ? 1 : 0, quest.Progress);
        }

        [TestCase(new double[] { 0, 0, 0, 0 })]
        [TestCase(new double[] { 1, 0, 1, 0 })]
        [TestCase(new double[] { 1, 1, 1, 1 })]
        public void ExecuteWithChildrenTest(double[] progresses)
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();
            quest.Progress = 0;
            quest.Children = new List<Quest>();
            foreach (double prog in progresses)
            {
                quest.Children.Add(new Quest{Progress = prog});
            }

            RecountProgressQuestCommand command = new RecountProgressQuestCommand();

            //Act
            bool result = command.Execute(quest);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(progresses.Average(), quest.Progress);
        }

        [Test]
        public void UndoTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();
            quest.State = State.Done;
            quest.Progress = 0;

            RecountProgressQuestCommand command = new RecountProgressQuestCommand();

            //Act
            bool executeResult = command.Execute(quest);
            bool undoResult = command.Undo(quest);

            //Assert
            Assert.IsTrue(executeResult);
            Assert.IsTrue(undoResult);
            Assert.AreEqual(0, quest.Progress);
        }
    }
}
