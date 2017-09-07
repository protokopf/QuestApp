using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Property;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestCommandsTest.Property
{
    [TestFixture]
    class PropertyChangeQuestCommandTest
    {
        private class MockPropertyChangeQuestCommand : PropertyChangeQuestCommand<int>
        {
            protected override int GetPropertyValue(Quest quest)
            {
                return OnGetPropertyValue?.Invoke(quest) ?? default(int);
            }

            protected override void SetPropertyValue(Quest quest, int propertValue)
            {
                OnSetPropertyValue?.Invoke(quest, propertValue);
            }

            protected override void ActionOnQuest(Quest quest, int oldValue)
            {
                OnActionOnQuest?.Invoke(quest, oldValue);
            }

            public event Func<Quest, int> OnGetPropertyValue;
            public event Action<Quest, int> OnSetPropertyValue;
            public event Action<Quest, int> OnActionOnQuest;
        }

        [Test]
        public void ExecuteTest()
        {
            //Arrange
            int getValue = 42;

            bool onGetQuestIsEqualToExpected = false;
            bool onActionOnQuestIsEqualToExpected = false;
            bool oldValueEqualsToValueFromGetter = false;

            Quest quest = QuestHelper.CreateQuest();

            MockPropertyChangeQuestCommand command = new MockPropertyChangeQuestCommand();
            command.OnGetPropertyValue += q =>
            {
                onGetQuestIsEqualToExpected = quest == q;
                return getValue;
            };
            command.OnActionOnQuest += (q, ov) =>
            {
                onActionOnQuestIsEqualToExpected = quest == q;
                oldValueEqualsToValueFromGetter = ov == getValue;
            };

            //Act
            bool result = command.Execute(quest);

            //Assert
            Assert.IsTrue(result);

            Assert.IsTrue(onGetQuestIsEqualToExpected);
            Assert.IsTrue(onActionOnQuestIsEqualToExpected);
            Assert.IsTrue(oldValueEqualsToValueFromGetter);
        }

        [Test]
        public void ExecuteSameQuestTwiceTest()
        {
            //Arrange
            int getPropertyValueCounter = 0;
            int actionOnQuestCounter = 0;

            Quest quest = QuestHelper.CreateQuest();

            MockPropertyChangeQuestCommand command = new MockPropertyChangeQuestCommand();
            command.OnGetPropertyValue += q => getPropertyValueCounter++;
            command.OnActionOnQuest += (q, ov) =>
            {
                actionOnQuestCounter++;
            };

            //Act
            bool firstResult = command.Execute(quest);
            bool secondResult = command.Execute(quest);

            //Assert
            Assert.IsTrue(firstResult);
            Assert.IsFalse(secondResult);

            Assert.AreEqual(1, getPropertyValueCounter);
            Assert.AreEqual(1, actionOnQuestCounter);
        }

        [Test]
        public void UndoWithoutExecuteTest()
        {
            //Arrange
            int setPropertyCalledValue = 0;
            Quest quest = QuestHelper.CreateQuest();

            MockPropertyChangeQuestCommand command = new MockPropertyChangeQuestCommand();
            command.OnSetPropertyValue += (q, v) => { setPropertyCalledValue++; };

            //Act
            bool undoResult = command.Undo(quest);

            //Assert
            Assert.IsFalse(undoResult);
            Assert.AreEqual(0, setPropertyCalledValue);
        }

        [Test]
        public void UndoAfterExecuteTest()
        {
            //Arrange
            int getValue = 42;

            Quest quest = QuestHelper.CreateQuest();
            Quest questToSet = null;
            int valueToSet = default(int);

            MockPropertyChangeQuestCommand command = new MockPropertyChangeQuestCommand();

            command.OnGetPropertyValue += q => getValue;
            command.OnActionOnQuest += (q, ov) => { };
            command.OnSetPropertyValue += (q, v) =>
            {
                questToSet = q;
                valueToSet = v;
            };

            //Act
            command.Execute(quest);
            bool undoResult = command.Undo(quest);

            //Assert
            Assert.IsTrue(undoResult);
            Assert.AreEqual(quest, questToSet);
            Assert.AreEqual(getValue, valueToSet);
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            int setPropertyCalledValue = 0;
            Quest quest = QuestHelper.CreateQuest();

            MockPropertyChangeQuestCommand command = new MockPropertyChangeQuestCommand();
            command.OnSetPropertyValue += (q, v) => { setPropertyCalledValue++; };

            //Act
            command.Execute(quest);
            command.Commit();
            bool undoResult = command.Undo(quest);

            //Assert
            Assert.IsFalse(undoResult);
            Assert.AreEqual(0, setPropertyCalledValue);
        }
    }
}
