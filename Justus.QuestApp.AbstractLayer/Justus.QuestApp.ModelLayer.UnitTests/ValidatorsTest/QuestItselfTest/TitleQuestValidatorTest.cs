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
    class TitleQuestValidatorTest
    {
        [Test]
        public void ValidateNullTest()
        {
            //Arrange
            int nullOrWhitespace = 0;
            int nullOtWhitespaceClar = 1;
            int tooLong = 2;
            int tooLongClar = 3;

            TitleQuestValidator<int> validator = new TitleQuestValidator<int>(nullOrWhitespace, nullOtWhitespaceClar, tooLong, tooLongClar);

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => validator.Validate(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("\t\t")]
        public void NullOrWhiteSpaceTitleTest(string title)
        {
            //Arrange
            Quest quest = new FakeQuest()
            {
                Title = title
            };

            int nullOrWhitespace = 0;
            int nullOtWhitespaceClar = 1;
            int tooLong = 2;
            int tooLongClar = 3;

            TitleQuestValidator<int> validator = new TitleQuestValidator<int>(nullOrWhitespace, nullOtWhitespaceClar,tooLong, tooLongClar);

            //Act
            ClarifiedResponse<int> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccessful);
            Assert.AreEqual(1, response.Errors.Count);

            ClarifiedError<int> error = response.Errors[0];

            Assert.AreEqual(nullOrWhitespace, error.Error);
            Assert.AreEqual(nullOtWhitespaceClar, error.Clarification);

        }

        [Test]
        public void TitleTooLongTest()
        {
            //Arrange

            string title = new string('h', 26);

            Quest quest = new FakeQuest()
            {
                Title = title
            };

            int nullOrWhitespace = 0;
            int nullOtWhitespaceClar = 1;
            int tooLong = 2;
            int tooLongClar = 3;

            TitleQuestValidator<int> validator = new TitleQuestValidator<int>(nullOrWhitespace, nullOtWhitespaceClar, tooLong, tooLongClar);

            //Act
            ClarifiedResponse<int> response = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccessful);
            Assert.AreEqual(1, response.Errors.Count);

            ClarifiedError<int> error = response.Errors[0];

            Assert.AreEqual(tooLong, error.Error);
            Assert.AreEqual(tooLongClar, error.Clarification);
        }
    }
}
