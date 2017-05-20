using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Model.Composite;
using Justus.QuestApp.AbstractLayer.Validators;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ModelLayer.Validators.QuestItself;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.ValidatorsTest.QuestItselfTest
{
    [TestFixture]
    public class CompositeQuestValidatorTest
    {
        public class StubResponse : IResponse, IComposable<StubResponse>
        {
            public bool IsSuccessful => true;
            public void Compose(StubResponse other)
            {
               
            }
        }

        [Test]
        public void CtolNullTest()
        {
            //Arrange && Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new CompositeQuestValidator<StubResponse>(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("questValidators", ex.ParamName);
        }

        [Test]
        public void ValidateNullTest()
        {
            //Act
            CompositeQuestValidator<StubResponse> validator = new CompositeQuestValidator<StubResponse>(new IQuestValidator<StubResponse>[0]);

            //Arrange
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => validator.Validate(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void ValidateTest()
        {
            //Act
            Quest quest = new FakeQuest();
            StubResponse response = new StubResponse();

            IQuestValidator<StubResponse> firstValidator = MockRepository.GenerateStrictMock<IQuestValidator<StubResponse>>();
            firstValidator.
                Expect(v => v.Validate(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once().
                Return(response);

            IQuestValidator<StubResponse> secondValidator = MockRepository.GenerateStrictMock<IQuestValidator<StubResponse>>();
            secondValidator.
                Expect(v => v.Validate(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once().
                Return(response);

            CompositeQuestValidator<StubResponse> validator = new CompositeQuestValidator<StubResponse>(new []{firstValidator, secondValidator});

            //Arrange
            StubResponse returnedResponse = validator.Validate(quest);

            //Assert
            Assert.IsNotNull(returnedResponse);

            firstValidator.VerifyAllExpectations();
            secondValidator.VerifyAllExpectations();
        }
    }
}
