using System;
using Justus.QuestApp.AbstractLayer.Entities.Errors;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ModelLayer.Validators.QuestItself;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.ValidatorsTest.QuestItselfTest
{
    [TestFixture]
    class StartTimeDeadlineQuestValidatorTest
    {
        [Test]
        public void NullValidateTest()
        {
            //Arrange
            int startTimeMoreThanDeadline = 0;
            int startTimeMoreThanDeadlineClar = 1;
            int deadlineLessThanNow = 2;
            int deadlineLessThanNowClar = 3;

            StartTimeDeadlineQuestValidator<int> validator = new StartTimeDeadlineQuestValidator<int>(
                startTimeMoreThanDeadline,
                startTimeMoreThanDeadlineClar,
                deadlineLessThanNow, 
                deadlineLessThanNowClar);

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => validator.Validate(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void StartDeadlineDefaultTest()
        {
            //Arrange
            int startTimeMoreThanDeadline = 0;
            int startTimeMoreThanDeadlineClar = 1;
            int deadlineLessThanNow = 2;
            int deadlineLessThanNowClar = 3;

            StartTimeDeadlineQuestValidator<int> validator = new StartTimeDeadlineQuestValidator<int>(
                startTimeMoreThanDeadline,
                startTimeMoreThanDeadlineClar,
                deadlineLessThanNow,
                deadlineLessThanNowClar);
            Quest quest = new FakeQuest()
            {
                StartTime = null,
                Deadline = null
            };

            //Act
            ClarifiedResponse<int> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessful);
            Assert.IsEmpty(response.Errors);
        }

        [Test]
        public void OnlyDeadlineDefaultTest()
        {
            //Arrange
            int startTimeMoreThanDeadline = 0;
            int startTimeMoreThanDeadlineClar = 1;
            int deadlineLessThanNow = 2;
            int deadlineLessThanNowClar = 3;

            StartTimeDeadlineQuestValidator<int> validator = new StartTimeDeadlineQuestValidator<int>(
                startTimeMoreThanDeadline,
                startTimeMoreThanDeadlineClar,
                deadlineLessThanNow,
                deadlineLessThanNowClar);
            Quest quest = new FakeQuest()
            {
                StartTime = DateTime.Now,
                Deadline = null
            };

            //Act
            ClarifiedResponse<int> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessful);
            Assert.IsEmpty(response.Errors);
        }

        [Test]
        public void OnlyStartTimeDefaultDeadlineBeforeNowTest()
        {
            //Arrange
            int startTimeMoreThanDeadline = 0;
            int startTimeMoreThanDeadlineClar = 1;
            int deadlineLessThanNow = 2;
            int deadlineLessThanNowClar = 3;

            StartTimeDeadlineQuestValidator<int> validator = new StartTimeDeadlineQuestValidator<int>(
                startTimeMoreThanDeadline,
                startTimeMoreThanDeadlineClar,
                deadlineLessThanNow,
                deadlineLessThanNowClar);
            Quest quest = new FakeQuest()
            {
                StartTime = null,
                Deadline = DateTime.Now - new TimeSpan(1,1,1)
            };

            //Act
            ClarifiedResponse<int> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccessful);
            Assert.AreEqual(1, response.Errors.Count);
            ClarifiedError<int> error = response.Errors[0];
            Assert.AreEqual(deadlineLessThanNow, error.Error);
            Assert.AreEqual(deadlineLessThanNowClar, error.Clarification);

        }

        [Test]
        public void OnlyStartTimeDefaultDeadlineAfterNowTest()
        {
            //Arrange
            int startTimeMoreThanDeadline = 0;
            int startTimeMoreThanDeadlineClar = 1;
            int deadlineLessThanNow = 2;
            int deadlineLessThanNowClar = 3;

            StartTimeDeadlineQuestValidator<int> validator = new StartTimeDeadlineQuestValidator<int>(
                startTimeMoreThanDeadline,
                startTimeMoreThanDeadlineClar,
                deadlineLessThanNow,
                deadlineLessThanNowClar);
            Quest quest = new FakeQuest()
            {
                StartTime = null,
                Deadline = DateTime.Now + new TimeSpan(1, 1, 1)
            };

            //Act
            ClarifiedResponse<int> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessful);
            Assert.IsEmpty(response.Errors);
        }

        [Test]
        public void DeadlineMoreThanStartTimeTest()
        {
            //Arrange
            int startTimeMoreThanDeadline = 0;
            int startTimeMoreThanDeadlineClar = 1;
            int deadlineLessThanNow = 2;
            int deadlineLessThanNowClar = 3;

            StartTimeDeadlineQuestValidator<int> validator = new StartTimeDeadlineQuestValidator<int>(
                startTimeMoreThanDeadline,
                startTimeMoreThanDeadlineClar,
                deadlineLessThanNow,
                deadlineLessThanNowClar);

            DateTime current = DateTime.Now;

            Quest quest = new FakeQuest()
            {
                StartTime = current,
                Deadline = current + new TimeSpan(1,1,1)
            };

            //Act
            ClarifiedResponse<int> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessful);
            Assert.IsEmpty(response.Errors);
        }

        [Test]
        public void DeadlineLessThanStartTimeTest()
        {
            //Arrange
            int startTimeMoreThanDeadline = 0;
            int startTimeMoreThanDeadlineClar = 1;
            int deadlineLessThanNow = 2;
            int deadlineLessThanNowClar = 3;

            StartTimeDeadlineQuestValidator<int> validator = new StartTimeDeadlineQuestValidator<int>(
                startTimeMoreThanDeadline,
                startTimeMoreThanDeadlineClar,
                deadlineLessThanNow,
                deadlineLessThanNowClar);

            DateTime current = DateTime.Now + new TimeSpan(1,1,1);

            Quest quest = new FakeQuest()
            {
                StartTime = current + new TimeSpan(1,1,1),
                Deadline = current
            };

            //Act
            ClarifiedResponse<int> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccessful);
            Assert.AreEqual(1, response.Errors.Count);
            ClarifiedError<int> error = response.Errors[0];
            Assert.AreEqual(startTimeMoreThanDeadline, error.Error);
            Assert.AreEqual(startTimeMoreThanDeadlineClar, error.Clarification);
        }

        [Test]
        public void DeadlineLessThanNowButMoreThanStartTimeTest()
        {
            //Arrange
            int startTimeMoreThanDeadline = 0;
            int startTimeMoreThanDeadlineClar = 1;
            int deadlineLessThanNow = 2;
            int deadlineLessThanNowClar = 3;

            StartTimeDeadlineQuestValidator<int> validator = new StartTimeDeadlineQuestValidator<int>(
                startTimeMoreThanDeadline,
                startTimeMoreThanDeadlineClar,
                deadlineLessThanNow,
                deadlineLessThanNowClar);

            DateTime current = DateTime.Now - new TimeSpan(1,1,1);

            Quest quest = new FakeQuest()
            {
                StartTime = current - new TimeSpan(1,1,1),
                Deadline = current
            };

            //Act
            ClarifiedResponse<int> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccessful);
            Assert.AreEqual(1, response.Errors.Count);
            ClarifiedError<int> error = response.Errors[0];
            Assert.AreEqual(deadlineLessThanNow, error.Error);
            Assert.AreEqual(deadlineLessThanNowClar, error.Clarification);
        }
    }
}
