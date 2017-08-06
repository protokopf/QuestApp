using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Entities.Responses;
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
    class QuestEditViewModelTest
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
                    () => new QuestEditViewModel(questFactory, questValidator, null, treeCommands));
            ArgumentNullException treeCommandsEx =
                 Assert.Throws<ArgumentNullException>(
                    () => new QuestEditViewModel(questFactory, questValidator, questTree, null));

            //Assert
            Assert.IsNotNull(treeEx);
            Assert.IsNotNull(treeCommandsEx);

            Assert.AreEqual("questTree", treeEx.ParamName);
            Assert.AreEqual("treeCommands", treeCommandsEx.ParamName);

            questTree.VerifyAllExpectations();
            questFactory.VerifyAllExpectations();
            questValidator.VerifyAllExpectations();
            treeCommands.VerifyAllExpectations();
        }

        [Test]
        public void QuestIdTest()
        {
            //Arrange
            int questId = 42;

            IQuestTree questTree = MockRepository.GenerateStrictMock<IQuestTree>();
            IQuestViewModelFactory questFactory = MockRepository.GenerateStrictMock<IQuestViewModelFactory>();
            IQuestValidator<ClarifiedResponse<int>> questValidator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();
            ITreeCommandsFactory treeCommands =
                MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            QuestEditViewModel vm = new QuestEditViewModel(questFactory, questValidator, questTree, treeCommands)
            {
                QuestId = questId
            };

            //Act
            int expectedQuestId = vm.QuestId;

            //Assert
            Assert.AreEqual(questId, expectedQuestId);

            questTree.VerifyAllExpectations();
            questFactory.VerifyAllExpectations();
            questValidator.VerifyAllExpectations();
            treeCommands.VerifyAllExpectations();
        }

        [Test]
        public void InitializeRetrieveQuestWithoutStartDeadlineFromTreeByIdTest()
        {
            int questId = 42;

            Quest model = new FakeQuest()
            {
                StartTime = null,
                Deadline = null
            };

            IQuestViewModel questViewModel = MockRepository.GenerateStrictMock<IQuestViewModel>();
            questViewModel.Expect(qvm => qvm.Model).
                SetPropertyWithArgument(model).
                Repeat.Once();
            questViewModel.Expect(qvm => qvm.UseStartTime).
                SetPropertyWithArgument(false).
                Repeat.Once();
            questViewModel.Expect(qvm => qvm.UseDeadline).
                SetPropertyWithArgument(false).
                Repeat.Once();

            IQuestTree questTree = MockRepository.GenerateStrictMock<IQuestTree>();
            questTree.Expect(qt => qt.Get(Arg<Predicate<Quest>>.Is.NotNull)).
                Repeat.Once().
                Return(model);

            IQuestViewModelFactory questFactory = MockRepository.GenerateStrictMock<IQuestViewModelFactory>();
            questFactory.Expect(qf => qf.CreateQuestViewModel()).
                Repeat.Once().
                Return(questViewModel);

            IQuestValidator<ClarifiedResponse<int>> questValidator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();
            ITreeCommandsFactory treeCommands =
                MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            QuestEditViewModel vm = new QuestEditViewModel(questFactory, questValidator, questTree, treeCommands)
            {
                QuestId = questId
            };

            //Act
            vm.Initialize();

            //Assert
            questViewModel.VerifyAllExpectations();
            questTree.VerifyAllExpectations();
            questFactory.VerifyAllExpectations();
            questValidator.VerifyAllExpectations();
            treeCommands.VerifyAllExpectations();
        }

        [Test]
        public void InitializeRetrieveQuestWithStartDeadlineFromTreeByIdTest()
        {
            int questId = 42;

            Quest model = new FakeQuest()
            {
                StartTime = DateTime.Now,
                Deadline = DateTime.Now
            };

            IQuestViewModel questViewModel = MockRepository.GenerateStrictMock<IQuestViewModel>();
            questViewModel.Expect(qvm => qvm.Model).
                SetPropertyWithArgument(model).
                Repeat.Once();
            questViewModel.Expect(qvm => qvm.UseStartTime).
                SetPropertyWithArgument(true).
                Repeat.Once();
            questViewModel.Expect(qvm => qvm.UseDeadline).
                SetPropertyWithArgument(true).
                Repeat.Once();

            IQuestTree questTree = MockRepository.GenerateStrictMock<IQuestTree>();
            questTree.Expect(qt => qt.Get(Arg<Predicate<Quest>>.Is.NotNull)).
                Repeat.Once().
                Return(model);

            IQuestViewModelFactory questFactory = MockRepository.GenerateStrictMock<IQuestViewModelFactory>();
            questFactory.Expect(qf => qf.CreateQuestViewModel()).
                Repeat.Once().
                Return(questViewModel);

            IQuestValidator<ClarifiedResponse<int>> questValidator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();
            ITreeCommandsFactory treeCommands =
                MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            QuestEditViewModel vm = new QuestEditViewModel(questFactory, questValidator, questTree, treeCommands)
            {
                QuestId = questId
            };

            //Act
            vm.Initialize();

            //Assert
            questViewModel.VerifyAllExpectations();
            questTree.VerifyAllExpectations();
            questFactory.VerifyAllExpectations();
            questValidator.VerifyAllExpectations();
            treeCommands.VerifyAllExpectations();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ActionTest(bool isCommandExecuted)
        {
            int questId = 42;

            Quest model = new FakeQuest()
            {
                StartTime = DateTime.Now,
                Deadline = DateTime.Now
            };

            IQuestViewModel questViewModel = MockRepository.GenerateStrictMock<IQuestViewModel>();
            questViewModel.Expect(qvm => qvm.Model).
                SetPropertyWithArgument(model).
                Repeat.Once();
            questViewModel.Expect(qvm => qvm.Model).
                Repeat.Once().
                Return(model);
            questViewModel.Expect(qvm => qvm.UseStartTime).
                SetPropertyWithArgument(true).
                Repeat.Once();
            questViewModel.Expect(qvm => qvm.UseDeadline).
                SetPropertyWithArgument(true).
                Repeat.Once();

            IQuestTree questTree = MockRepository.GenerateStrictMock<IQuestTree>();
            questTree.Expect(qt => qt.Get(Arg<Predicate<Quest>>.Is.NotNull)).
                Repeat.Once().
                Return(model);

            IQuestViewModelFactory questFactory = MockRepository.GenerateStrictMock<IQuestViewModelFactory>();
            questFactory.Expect(qf => qf.CreateQuestViewModel()).
                Repeat.Once().
                Return(questViewModel);

            IQuestValidator<ClarifiedResponse<int>> questValidator =
                MockRepository.GenerateStrictMock<IQuestValidator<ClarifiedResponse<int>>>();

            ICommand updateCommand = MockRepository.GenerateStrictMock<ICommand>();
            updateCommand.Expect(uc => uc.Execute()).
                Repeat.Once().
                Return(isCommandExecuted);
            if (isCommandExecuted)
            {
                updateCommand.Expect(uc => uc.Commit()).
                    Repeat.Once().
                    Return(true);
            }

            ITreeCommandsFactory treeCommands =
                MockRepository.GenerateStrictMock<ITreeCommandsFactory>();
            treeCommands.Expect(tc => tc.UpdateQuest(Arg<Quest>.Is.Equal(model))).
                Repeat.Once().
                Return(updateCommand);

            QuestEditViewModel vm = new QuestEditViewModel(questFactory, questValidator, questTree, treeCommands)
            {
                QuestId = questId
            };

            //Act
            vm.Initialize();
            vm.Action();

            //Assert
            questViewModel.VerifyAllExpectations();
            questTree.VerifyAllExpectations();
            questFactory.VerifyAllExpectations();
            questValidator.VerifyAllExpectations();
            updateCommand.VerifyAllExpectations();
            treeCommands.VerifyAllExpectations();
        }
    }
}
