using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Commands.Factories;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ViewModelLayer.UnitTests.Stubs;
using Justus.QuestApp.ViewModelLayer.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.ViewModelsTest
{
    [TestFixture]
    class ResultQuestListVIewModelTest
    {
        [Test]
        public void CtorNotThrowExceptionsTest()
        {
            //Arrange
            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            //Act && Assert
            Assert.DoesNotThrow(() => new ResultsQuestListViewModel(model, stateCommands, repoCommands));

            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }

        [Test]
        public void RestartQuestInvalidPositionTest()
        {
            //Arrange
            int position = 1;

            List<Quest> leaves = new List<Quest>();

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.Leaves).
                Repeat.Once().
                Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            stateCommands.Expect(sc => sc.CancelQuest(Arg<Quest>.Is.Anything)).
                Repeat.Never();
            stateCommands.Expect(sc => sc.StartQuest(Arg<Quest>.Is.Anything)).
                Repeat.Never();


            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            ResultsQuestListViewModel viewModel = new ResultsQuestListViewModel(model, stateCommands, repoCommands);

            //Act
            Task deleteTask = viewModel.RestartQuest(position);

            //Assert
            Assert.IsNull(deleteTask);

            model.VerifyAllExpectations();
            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
        }

        [Test]
        public void RestartQuestValidPositionTest()
        {
            //Arrange
            int position = 0;
            Quest toRestart = new FakeQuest();

            List<Quest> leaves = new List<Quest> { toRestart };

            ICommand startCommand = MockRepository.GenerateStrictMock<ICommand>();
            startCommand.Expect(dc => dc.Execute()).
                Repeat.Once().
                Return(true);
            startCommand.Expect(dc => dc.Commit()).
                Repeat.Once().
                Return(true);

            ICommand cancelCommand = MockRepository.GenerateStrictMock<ICommand>();
            cancelCommand.Expect(dc => dc.Execute()).
                Repeat.Once().
                Return(true);
            cancelCommand.Expect(dc => dc.Commit()).
                Repeat.Once().
                Return(true);

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(rep => rep.Leaves).
                Repeat.Once().
                Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            stateCommands.Expect(sc => sc.CancelQuest(Arg<Quest>.Is.Equal(toRestart))).
                Repeat.Once().
                Return(cancelCommand);
            stateCommands.Expect(sc => sc.StartQuest(Arg<Quest>.Is.Equal(toRestart))).
                Repeat.Once().
                Return(startCommand);

            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            ResultsQuestListViewModel viewModel = new ResultsQuestListViewModel(model, stateCommands, repoCommands);

            //Act
            Task startTask = viewModel.RestartQuest(position);

            //Assert
            Assert.IsNotNull(startTask);
            startTask.Wait();

            cancelCommand.VerifyAllExpectations();
            startCommand.VerifyAllExpectations();
            model.VerifyAllExpectations();
            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
        }
    }
}
