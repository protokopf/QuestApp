using System;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Validators;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using Justus.QuestApp.ModelLayer.Validators.Actions;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.ValidatorsTest.ActionsTest
{
    [TestFixture]
    class StartQuestValidatorTest
    {
        [Test]
        public void NullQuestTest()
        {
            //Arrange
            IQuestValidator validator = new StartQuestValidator();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => validator.Validate(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void ValidateProgressQuestTest()
        {
            //Arrange
            IQuestValidator validator = new StartQuestValidator();
            Quest quest = QuestHelper.CreateQuest(QuestState.Progress);

            //Act
            Response result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("ERR_QUEST_ACT_WRONG_STATE", result.Errors[0]);
        }

        [Test]
        public void ValidateDoneQuestTest()
        {
            //Arrange
            IQuestValidator validator = new StartQuestValidator();
            Quest quest = QuestHelper.CreateQuest(QuestState.Done);

            //Act
            Response result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("ERR_QUEST_ACT_WRONG_STATE", result.Errors[0]);
        }

        [Test]
        public void ValidateFailedQuestTest()
        {
            //Arrange
            IQuestValidator validator = new StartQuestValidator();
            Quest quest = QuestHelper.CreateQuest(QuestState.Failed);

            //Act
            Response result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("ERR_QUEST_ACT_WRONG_STATE", result.Errors[0]);
        }

        [Test]
        public void ValidateReadyQuestWithChildren()
        {
            //Arrange
            IQuestValidator validator = new StartQuestValidator();
            Quest quest = QuestHelper.CreateQuest(QuestState.Idle);
            quest.Children.Add(QuestHelper.CreateQuest(QuestState.Idle));

            //Act
            Response result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("ERR_QUEST_ACT_HAS_CHILDREN", result.Errors[0]);
        }

        [Test]
        public void ValidateSuccessfulTest()
        {
            //Arrange
            IQuestValidator validator = new StartQuestValidator();
            Quest quest = QuestHelper.CreateQuest(QuestState.Idle);

            //Act
            Response result = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}
