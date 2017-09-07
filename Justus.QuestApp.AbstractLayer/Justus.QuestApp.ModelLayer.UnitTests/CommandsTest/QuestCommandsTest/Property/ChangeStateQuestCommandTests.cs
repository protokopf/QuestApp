using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Property;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestCommandsTest.Property
{
    [TestFixture]
    class ChangeStateQuestCommandTests
    {
        [Test]
        public void ExecuteTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();
            quest.State = State.Done;

            ChangeStateQuestCommand command = new ChangeStateQuestCommand(State.Failed);

            //Act
            bool result = command.Execute(quest);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(State.Failed, quest.State);
        }

        [Test]
        public void UndoTest()
        {
            //Arrange
            State initialState = State.Done;

            Quest quest = QuestHelper.CreateQuest();
            quest.State = initialState;

            ChangeStateQuestCommand command = new ChangeStateQuestCommand(State.Failed);

            //Act
            bool result = command.Execute(quest);
            bool undoResult = command.Undo(quest);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(undoResult);
            Assert.AreEqual(initialState, quest.State);
        }
    }
}
