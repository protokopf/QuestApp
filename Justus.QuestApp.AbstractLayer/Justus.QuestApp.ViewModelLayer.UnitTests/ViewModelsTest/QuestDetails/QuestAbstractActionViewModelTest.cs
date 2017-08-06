using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Validators;
using Justus.QuestApp.ViewModelLayer.Factories;
using Justus.QuestApp.ViewModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails;
using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails.Abstract;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest.QuestDetails
{
    [TestFixture]
    class QuestAbstractActionViewModelTest
    {
        public class MockQuestAbstractActionViewModel : QuestAbstractActionViewModel
        {
            public MockQuestAbstractActionViewModel(IQuestViewModelFactory questViewModelFactory, 
                IQuestValidator<ClarifiedResponse<int>> questValidator) : base(questViewModelFactory, questValidator)
            {
            }

            public override void Action()
            {
            }
        }

        [Test]
        public void CtorNullTest()
        {
            //Arrange
            IQuestViewModelFactory viewModelFactory = MockRepository.GenerateStrictMock<IQuestViewModelFactory>();
            IQuestValidator<ClarifiedResponse<int>> validator = MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();

            //Act
            ArgumentNullException viewModelFactoryEx =
                Assert.Throws<ArgumentNullException>(() => new MockQuestAbstractActionViewModel(null,validator));
            ArgumentNullException validatorEx =
                Assert.Throws<ArgumentNullException>(() => new MockQuestAbstractActionViewModel(viewModelFactory, null));

            //Assert
            Assert.IsNotNull(viewModelFactoryEx);
            Assert.AreEqual("questViewModelFactory", viewModelFactoryEx.ParamName);

            Assert.IsNotNull(validatorEx);
            Assert.AreEqual("questValidator", validatorEx.ParamName);

            viewModelFactory.VerifyAllExpectations();
            validator.VerifyAllExpectations();
        }

        [Test]
        public void InitializeTest()
        {
            //Arrange
            IQuestViewModel questViewModel = MockRepository.GenerateStrictMock<IQuestViewModel>();

            IQuestViewModelFactory vmFactory = MockRepository.GenerateStrictMock<IQuestViewModelFactory>();
            vmFactory.Expect(vmf => vmf.CreateQuestViewModel()).
                Repeat.Once().
                Return(questViewModel);

            IQuestValidator<ClarifiedResponse<int>> validator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();

            MockQuestAbstractActionViewModel viewModel = new MockQuestAbstractActionViewModel(vmFactory, validator);

            //Act
            viewModel.Initialize();

            //Assert
            Assert.AreEqual(questViewModel, viewModel.QuestViewModel);

            questViewModel.VerifyAllExpectations();
            validator.VerifyAllExpectations();
        }

        [Test]
        public void ValidateTest()
        {
            //Arrange
            Quest model = new FakeQuest();
            ClarifiedResponse<int> response = new ClarifiedResponse<int>();

            IQuestViewModel questViewModel = MockRepository.GenerateStrictMock<IQuestViewModel>();
            questViewModel.Expect(qvm => qvm.Model).
                Repeat.Once().
                Return(model);

            IQuestViewModelFactory vmFactory = MockRepository.GenerateStrictMock<IQuestViewModelFactory>();
            vmFactory.Expect(vmf => vmf.CreateQuestViewModel()).
                Repeat.Once().
                Return(questViewModel);

            IQuestValidator<ClarifiedResponse<int>> validator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();
            validator.Expect(v => v.Validate(Arg<Quest>.Is.Equal(model))).
                Repeat.Once().
                Return(response);

            MockQuestAbstractActionViewModel viewModel = new MockQuestAbstractActionViewModel(vmFactory,validator);

            //Act
            viewModel.Initialize();
            ClarifiedResponse<int>  actualResponse = viewModel.Validate();

            //Assert
            Assert.IsNotNull(actualResponse);
            Assert.AreEqual(response, actualResponse);

            questViewModel.VerifyAllExpectations();
            vmFactory.VerifyAllExpectations();
            validator.VerifyAllExpectations();
        }
    }
}
