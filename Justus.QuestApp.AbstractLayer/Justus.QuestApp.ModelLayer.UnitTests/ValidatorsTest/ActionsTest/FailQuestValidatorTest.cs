using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using Justus.QuestApp.ModelLayer.Validators.Actions;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.ValidatorsTest.ActionsTest
{
    [TestFixture]
    class FailQuestValidatorTest
    {
        [Test]
        public void NullQuestTest()
        {
            //Arrange
            FailQuestValidator validator = new FailQuestValidator();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => validator.Validate(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [TestCase(State.Failed)]
        [TestCase(State.Done)]
        [TestCase(State.Idle)]
        public void ValidateQuestWithWrongStateTest(State state)
        {
            //Arrange
            FailQuestValidator validator = new FailQuestValidator();
            Quest quest = QuestHelper.CreateQuest(state);

            //Act
            StringResponse result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("ERR_QUEST_ACT_WRONG_STATE", result.Errors[0]);
        }

        [Test]
        public void ValidateSuccessfulWhenChildrenDoneTest()
        {
            //Arrange
            FailQuestValidator validator = new FailQuestValidator();
            Quest quest = QuestHelper.CreateQuest(State.Progress);
            quest.Children.Add(QuestHelper.CreateQuest(State.Failed));
            quest.Children.Add(QuestHelper.CreateQuest(State.Failed));

            //Act
            StringResponse result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public void ValidateSuccessfulWithouChildrenTest()
        {
            //Arrange
            FailQuestValidator validator = new FailQuestValidator();
            Quest quest = QuestHelper.CreateQuest(State.Progress);

            //Act
            StringResponse result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public void ValidateFailedWithChildrenNotDoneTest()
        {
            //Arrange
            FailQuestValidator validator = new FailQuestValidator();
            Quest quest = QuestHelper.CreateQuest(State.Progress);
            quest.Children.Add(QuestHelper.CreateQuest(State.Done));
            quest.Children.Add(QuestHelper.CreateQuest(State.Progress));

            //Act
            StringResponse result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("ERR_QUEST_ACT_CHILDREN_NOT_SAME_STATE", result.Errors[0]);
        }
    }
}
