using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using Justus.QuestApp.ModelLayer.Validators.Actions;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.ValidatorsTest.ActionsTest
{
    [TestFixture]
    class IdleQuestValidatorTest
    {
        [Test]
        public void NullQuestTest()
        {
            //Arrange
            IdleQuestValidator validator = new IdleQuestValidator();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => validator.Validate(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [TestCase(State.Idle)]
        public void ValidateQuestWithWrongStateTest(State state)
        {
            //Arrange
            IdleQuestValidator validator = new IdleQuestValidator();
            Quest quest = QuestHelper.CreateQuest(state);

            //Act
            StringResponse result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("ERR_QUEST_ACT_WRONG_STATE", result.Errors[0]);
        }

        [TestCase(State.Failed)]
        [TestCase(State.Progress)]
        [TestCase(State.Done)]
        public void ValidateQuestSuccessfulTest(State state)
        {
            //Arrange
            IdleQuestValidator validator = new IdleQuestValidator();
            Quest quest = QuestHelper.CreateQuest(state);

            //Act
            StringResponse result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}
