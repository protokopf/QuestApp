using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            StartTimeDeadlineQuestValidator validator = new StartTimeDeadlineQuestValidator();

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
            StartTimeDeadlineQuestValidator validator = new StartTimeDeadlineQuestValidator();
            Quest quest = new FakeQuest()
            {
                StartTime = default(DateTime),
                Deadline = default(DateTime)
            };

            //Act
            ClarifiedResponse<string> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessful);
            Assert.IsEmpty(response.Errors);
        }

        [Test]
        public void OnlyDeadlineDefaultTest()
        {
            //Arrange
            StartTimeDeadlineQuestValidator validator = new StartTimeDeadlineQuestValidator();
            Quest quest = new FakeQuest()
            {
                StartTime = DateTime.Now,
                Deadline = default(DateTime)
            };

            //Act
            ClarifiedResponse<string> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessful);
            Assert.IsEmpty(response.Errors);
        }

        [Test]
        public void OnlyStartTimeDefaultDeadlineBeforeNowTest()
        {
            //Arrange
            StartTimeDeadlineQuestValidator validator = new StartTimeDeadlineQuestValidator();
            Quest quest = new FakeQuest()
            {
                StartTime = default(DateTime),
                Deadline = DateTime.Now - new TimeSpan(1,1,1)
            };

            //Act
            ClarifiedResponse<string> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessful);
            Assert.IsEmpty(response.Errors);
        }

        [Test]
        public void OnlyStartTimeDefaultDeadlineAfterNowTest()
        {
            //Arrange
            StartTimeDeadlineQuestValidator validator = new StartTimeDeadlineQuestValidator();
            Quest quest = new FakeQuest()
            {
                StartTime = default(DateTime),
                Deadline = DateTime.Now + new TimeSpan(1, 1, 1)
            };

            //Act
            ClarifiedResponse<string> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccessful);
            Assert.AreEqual(1, response.Errors.Count);
            ClarifiedError<string> error = response.Errors[0];
            Assert.AreEqual("0", error.Error);
            Assert.AreEqual("0_CL", error.Clarification);
        }

        [Test]
        public void DeadlineMoreThanStartTimeTest()
        {
            //Arrange
            StartTimeDeadlineQuestValidator validator = new StartTimeDeadlineQuestValidator();

            DateTime current = DateTime.Now;

            Quest quest = new FakeQuest()
            {
                StartTime = current,
                Deadline = current + new TimeSpan(1,1,1)
            };

            //Act
            ClarifiedResponse<string> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessful);
            Assert.IsEmpty(response.Errors);
        }

        [Test]
        public void DeadlineLessThanStartTimeTest()
        {
            //Arrange
            StartTimeDeadlineQuestValidator validator = new StartTimeDeadlineQuestValidator();

            DateTime current = DateTime.Now;

            Quest quest = new FakeQuest()
            {
                StartTime = current + new TimeSpan(1,1,1),
                Deadline = current
            };

            //Act
            ClarifiedResponse<string> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccessful);
            Assert.AreEqual(1, response.Errors.Count);
            ClarifiedError<string> error = response.Errors[0];
            Assert.AreEqual("1", error.Error);
            Assert.AreEqual("1_CL", error.Clarification);
        }
    }
}
