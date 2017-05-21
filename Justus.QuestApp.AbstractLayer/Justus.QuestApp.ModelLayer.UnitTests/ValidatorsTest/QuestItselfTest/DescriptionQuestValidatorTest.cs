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
using NUnit.Framework.Internal;

namespace Justus.QuestApp.ModelLayer.UnitTests.ValidatorsTest.QuestItselfTest
{
    [TestFixture]
    class DescriptionQuestValidatorTest
    {
        [Test]
        public void ValidateNullTest()
        {
            //Arrange
            int nullOrWhitespace = 0;
            int nullOtWhitespaceClar = 1;

            DescriptionQuestValidator<int> validator = new DescriptionQuestValidator<int>(nullOrWhitespace,
                nullOtWhitespaceClar);

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
        public void NullOrWhiteSpaceDescriptionTest(string description)
        {
            //Arrange
            Quest quest = new FakeQuest()
            {
                Description = description
            };

            int nullOrWhitespace = 0;
            int nullOtWhitespaceClar = 1;

            DescriptionQuestValidator<int> validator = new DescriptionQuestValidator<int>(nullOrWhitespace,
                nullOtWhitespaceClar);

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

    }
}
