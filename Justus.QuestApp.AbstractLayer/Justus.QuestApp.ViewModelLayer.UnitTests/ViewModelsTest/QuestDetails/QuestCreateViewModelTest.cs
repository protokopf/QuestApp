using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
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
    class QuestCreateViewModelTest
    {
        [Test]
        public void CtorNullTest()
        {
            //Arrange
            IQuestTree questTree = MockRepository.GenerateStrictMock<IQuestTree>();
            IQuestViewModelFactory questFactory = MockRepository.GenerateStrictMock<IQuestViewModelFactory>();
            IQuestValidator<ClarifiedResponse<int>> questValidator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();
            ITreeCommandsFactory treeCommands =
                MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            //Act
            ArgumentNullException treeEx =
                Assert.Throws<ArgumentNullException>(
                    () => new QuestCreateViewModel(questFactory, questValidator,null, treeCommands));
            ArgumentNullException treeCommandsEx =
                 Assert.Throws<ArgumentNullException>(
                    () => new QuestCreateViewModel(questFactory, questValidator, questTree, null));

            //Assert
            Assert.IsNotNull(treeEx);
            Assert.IsNotNull(treeCommandsEx);

            Assert.AreEqual("questTree", treeEx.ParamName);
            Assert.AreEqual("treeCommands", treeCommandsEx.ParamName);

            questFactory.VerifyAllExpectations();
            questTree.VerifyAllExpectations();
            questValidator.VerifyAllExpectations();
            treeCommands.VerifyAllExpectations();
        }

        [TestCase(false,false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void ActionTestWithStartTimeDeadlineTest(bool useStartTime, bool useDeadLine)
        {
            //Arrange
            Quest quest = new FakeQuest();
            Quest parent = new FakeQuest();

            IQuestViewModel questViewModel = MockRepository.GenerateStrictMock<IQuestViewModel>();
            questViewModel.Expect(qvm => qvm.Model).
                Repeat.Once().
                Return(quest);

            ICommand addCommand = MockRepository.GenerateStrictMock<ICommand>();
            addCommand.Expect(ac => ac.Execute()).
                Return(true).
                Repeat.Once();
            addCommand.Expect(ac => ac.Commit()).
                Return(true).
                Repeat.Once();

            IQuestTree questRepository = MockRepository.GenerateStrictMock<IQuestTree>();
            questRepository.Expect(qt => qt.Get(Arg<Predicate<Quest>>.Is.NotNull)).
                Repeat.Once().
                Return(parent);

            ITreeCommandsFactory factory = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();
            factory.Expect(f => f.AddQuest(Arg<Quest>.Is.Equal(parent),Arg<Quest>.Is.Equal(quest)))
                .Return(addCommand)
                .Repeat.Once();

            IQuestValidator<ClarifiedResponse<int>> questValidator = 
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();

            IQuestViewModelFactory questViewModelFactory = MockRepository.GenerateStrictMock<IQuestViewModelFactory>();
            questViewModelFactory.Expect(qc => qc.CreateQuestViewModel()).
                Repeat.Once().
                Return(questViewModel);

            QuestCreateViewModel viewModel = new QuestCreateViewModel(
                questViewModelFactory, 
                questValidator,
                questRepository,
                factory);

            //Act   
            viewModel.Initialize();
            viewModel.Action();

            //Assert
            questViewModel.VerifyAllExpectations();
            addCommand.VerifyAllExpectations();
            questRepository.VerifyAllExpectations();
            factory.VerifyAllExpectations();
            questValidator.VerifyAllExpectations();
            questViewModelFactory.VerifyAllExpectations();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ActionCommitsCommandDependesOnExecuteResultTest(bool isExecuted)
        {
            //Arrange
            Quest quest = new FakeQuest();
            Quest parent = new FakeQuest();

            IQuestViewModel questViewModel = MockRepository.GenerateStrictMock<IQuestViewModel>();
            questViewModel.Expect(qvm => qvm.Model).
                Repeat.Once().
                Return(quest);

            ICommand addCommand = MockRepository.GenerateStrictMock<ICommand>();
            addCommand.Expect(ac => ac.Execute()).
                Return(isExecuted).
                Repeat.Once();
            if (isExecuted)
            {
                addCommand.Expect(ac => ac.Commit()).
                    Return(true).
                    Repeat.Once();
            }


            IQuestTree questRepository = MockRepository.GenerateStrictMock<IQuestTree>();
            questRepository.Expect(qt => qt.Get(Arg<Predicate<Quest>>.Is.NotNull)).
                Repeat.Once().
                Return(parent);

            ITreeCommandsFactory factory = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();
            factory.Expect(f => f.AddQuest(Arg<Quest>.Is.Equal(parent), Arg<Quest>.Is.Equal(quest)))
                .Return(addCommand)
                .Repeat.Once();

            IQuestValidator<ClarifiedResponse<int>> questValidator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();

            IQuestViewModelFactory questViewModelFactory = MockRepository.GenerateStrictMock<IQuestViewModelFactory>();
            questViewModelFactory.Expect(qc => qc.CreateQuestViewModel()).
                Repeat.Once().
                Return(questViewModel);

            QuestCreateViewModel viewModel = new QuestCreateViewModel(
                questViewModelFactory,
                questValidator,
                questRepository,
                factory);

            //Act   
            viewModel.Initialize();
            viewModel.Action();

            //Assert
            questViewModel.VerifyAllExpectations();
            addCommand.VerifyAllExpectations();
            questRepository.VerifyAllExpectations();
            factory.VerifyAllExpectations();
            questValidator.VerifyAllExpectations();
            questViewModelFactory.VerifyAllExpectations();
        }

        [Test]
        public void ActionWithModelNullTest()
        {
            //Arrange
            IQuestViewModel questViewModel = MockRepository.GenerateStrictMock<IQuestViewModel>();
            questViewModel.Expect(qvm => qvm.Model).
                Repeat.Once().
                Return(null);
            questViewModel.Expect(qvm => qvm.UseStartTime).
                Repeat.Never();
            questViewModel.Expect(qvm => qvm.UseDeadline).
                Repeat.Never();


            IQuestTree questRepository = MockRepository.GenerateStrictMock<IQuestTree>();
            questRepository.Expect(qt => qt.Get(Arg<Predicate<Quest>>.Is.NotNull)).
                Repeat.Never();

            ITreeCommandsFactory factory = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();
            factory.Expect(f => f.AddQuest(Arg<Quest>.Is.Anything, Arg<Quest>.Is.Anything))
                .Repeat.Never();

            IQuestValidator<ClarifiedResponse<int>> questValidator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();

            IQuestViewModelFactory questViewModelFactory = MockRepository.GenerateStrictMock<IQuestViewModelFactory>();
            questViewModelFactory.Expect(qc => qc.CreateQuestViewModel()).
                Repeat.Once().
                Return(questViewModel);

            QuestCreateViewModel viewModel = new QuestCreateViewModel(
                questViewModelFactory,
                questValidator,
                questRepository,
                factory);

            //Act   
            viewModel.Initialize();
            viewModel.Action();

            //Assert
            questViewModel.VerifyAllExpectations();
            questRepository.VerifyAllExpectations();
            factory.VerifyAllExpectations();
            questValidator.VerifyAllExpectations();
            questViewModelFactory.VerifyAllExpectations();
        }

        [Test]
        public void ParentIdTest()
        {
            //Arrange
            int parentId = 42;

            IQuestViewModel questViewModel = MockRepository.GenerateStrictMock<IQuestViewModel>();

            IQuestTree questRepository = MockRepository.GenerateStrictMock<IQuestTree>();

            ITreeCommandsFactory factory = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            IQuestValidator<ClarifiedResponse<int>> questValidator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();

            IQuestViewModelFactory questViewModelFactory = MockRepository.GenerateStrictMock<IQuestViewModelFactory>();

            QuestCreateViewModel viewModel = new QuestCreateViewModel(
                questViewModelFactory,
                questValidator,
                questRepository,
                factory);

            //Act
            viewModel.ParentId = parentId;
            int actualParentId = viewModel.ParentId;

            //Assert
            Assert.AreEqual(parentId, actualParentId);

            questViewModel.VerifyAllExpectations();
            questRepository.VerifyAllExpectations();
            factory.VerifyAllExpectations();
            questValidator.VerifyAllExpectations();
            questViewModelFactory.VerifyAllExpectations();
        }
    }
}
