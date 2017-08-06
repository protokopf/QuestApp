using System;
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
    class ActiveQuestListViewModelTest
    {
        [Test]
        public void CountProgressWrongPosition()
        {
            //Arrange
            int expected = -1;
            int position = 1;
            List<Quest> leaves = new List<Quest>();

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(m => m.Leaves).
                Repeat.Once().
                Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(model, stateCommands, repoCommands);


            //Act
            int progressInt = viewModel.CountProgress(position);

            //Assert
            Assert.AreEqual(expected, progressInt);

            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }

        [TestCase(0)]
        [TestCase(0.33333)]
        [TestCase(0.5)]
        [TestCase(0.75)]
        [TestCase(1)]
        public void CountProgressTest(double progress)
        {
            //Arrange
            int expected = (int)Math.Floor(progress * 100);
            int position = 0;

            Quest quest = new Quest()
            {
                Progress = progress
            };

            List<Quest> leaves = new List<Quest>()
            {
                quest
            };

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(m => m.Leaves).
                Repeat.Once().
                Return(leaves);


            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(model, stateCommands, repoCommands);


            //Act
            int progressInt = viewModel.CountProgress(position);

            //Assert
            Assert.AreEqual(expected, progressInt);

            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }

        [TestCase(1.1)]
        [TestCase(2.1)]
        public void CountProgressMoreThanOne(double progress)
        {
            //Arrange
            int expected = 100;
            int position = 0;

            Quest quest = new Quest
            {
                Progress = progress
            };

            List<Quest> leaves = new List<Quest>
            {
                quest
            };

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(m => m.Leaves).
                Repeat.Once().
                Return(leaves);


            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(model, stateCommands, repoCommands);


            //Act
            int progressInt = viewModel.CountProgress(position);

            //Assert
            Assert.AreEqual(expected, progressInt);

            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }

        [Test]
        public void IsRootDoneInTopRootTest()
        {
            //Arrange
            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(mo => mo.IsInTheRoot).Repeat.Once().Return(true);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(model, stateCommands, repoCommands);

            //Act
            bool isRootDone = viewModel.IsRootDone();

            //Assert
            Assert.IsFalse(isRootDone);

            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }

        [Test]
        public void IsRootDoneTest()
        {
            //Arrange
            List<Quest> list = new List<Quest>()
            {
                new FakeQuest()
                {
                    State = State.Progress
                }
            };

            Quest root = list[0];

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(mo => mo.IsInTheRoot).Repeat.Twice().Return(false);
            model.Expect(mo => mo.Parent).Repeat.Twice().Return(root);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();

            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(model, stateCommands, repoCommands);

            //Act
            bool isRootDoneBefore = viewModel.IsRootDone();
            root.State = State.Done;
            bool isRootDoneAfter = viewModel.IsRootDone();

            //Assert
            Assert.IsFalse(isRootDoneBefore);
            Assert.IsTrue(isRootDoneAfter);

            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }

        [Test]
        public void FailQuestWrongPositionTest()
        {
            //Arrange
            int position = 1;

            List<Quest> leaves = new List<Quest>();

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(m => m.Leaves).
                Repeat.Once().
                Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(model, stateCommands, repoCommands);


            //Act
            Task failTask = viewModel.FailQuest(position);

            //Assert
            Assert.IsNull(failTask);

            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }

        [Test]
        public void FailQuestTest()
        {
            //Arrange
            int position = 0;

            Quest toFail = new FakeQuest();

            List<Quest> leaves = new List<Quest> {toFail};

            ICommand returnedCommand = MockRepository.GenerateStrictMock<ICommand>();
            returnedCommand.Expect(rc => rc.Execute()).
                Repeat.Once().
                Return(true);
            returnedCommand.Expect(rc => rc.Commit()).
                Repeat.Once().
                Return(true);

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(m => m.Leaves).
                Repeat.Once().
                Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            stateCommands.Expect(sc => sc.FailQuest(Arg<Quest>.Is.Equal(toFail))).
                Repeat.Once().
                Return(returnedCommand);

            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(model, stateCommands, repoCommands);


            //Act
            Task failTask = viewModel.FailQuest(position);

            //Assert
            Assert.IsNotNull(failTask);
            failTask.Wait();

            returnedCommand.VerifyAllExpectations();
            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }

        [Test]
        public void DoneQuestWrongPositionTest()
        {
            //Arrange
            int position = 1;

            List<Quest> leaves = new List<Quest>();

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(m => m.Leaves).
                Repeat.Once().
                Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(model, stateCommands, repoCommands);


            //Act
            Task doneTask = viewModel.DoneQuest(position);

            //Assert
            Assert.IsNull(doneTask);

            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }

        [Test]
        public void DoneQuestTest()
        {
            //Arrange
            int position = 0;

            Quest toFail = new FakeQuest();

            List<Quest> leaves = new List<Quest> { toFail };

            ICommand returnedCommand = MockRepository.GenerateStrictMock<ICommand>();
            returnedCommand.Expect(rc => rc.Execute()).
                Repeat.Once().
                Return(true);
            returnedCommand.Expect(rc => rc.Commit()).
                Repeat.Once().
                Return(true);

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(m => m.Leaves).
                Repeat.Once().
                Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            stateCommands.Expect(sc => sc.DoneQuest(Arg<Quest>.Is.Equal(toFail))).
                Repeat.Once().
                Return(returnedCommand);

            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(model, stateCommands, repoCommands);


            //Act
            Task doneTask = viewModel.DoneQuest(position);

            //Assert
            Assert.IsNotNull(doneTask);
            doneTask.Wait();

            returnedCommand.VerifyAllExpectations();
            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }

        [Test]
        public void CancelQuestWrongPositionTest()
        {
            //Arrange
            int position = 1;

            List<Quest> leaves = new List<Quest>();

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(m => m.Leaves).
                Repeat.Once().
                Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(model, stateCommands, repoCommands);


            //Act
            Task cancelTask = viewModel.CancelQuest(position);

            //Assert
            Assert.IsNull(cancelTask);

            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }

        [Test]
        public void CancelQuestTest()
        {
            //Arrange
            int position = 0;

            Quest toFail = new FakeQuest();

            List<Quest> leaves = new List<Quest> { toFail };

            ICommand returnedCommand = MockRepository.GenerateStrictMock<ICommand>();
            returnedCommand.Expect(rc => rc.Execute()).
                Repeat.Once().
                Return(true);
            returnedCommand.Expect(rc => rc.Commit()).
                Repeat.Once().
                Return(true);

            IQuestListModel model = MockRepository.GenerateStrictMock<IQuestListModel>();
            model.Expect(m => m.Leaves).
                Repeat.Once().
                Return(leaves);

            IStateCommandsFactory stateCommands = MockRepository.GenerateStrictMock<IStateCommandsFactory>();
            stateCommands.Expect(sc => sc.CancelQuest(Arg<Quest>.Is.Equal(toFail))).
                Repeat.Once().
                Return(returnedCommand);

            ITreeCommandsFactory repoCommands = MockRepository.GenerateStrictMock<ITreeCommandsFactory>();


            ActiveQuestListViewModel viewModel = new ActiveQuestListViewModel(model, stateCommands, repoCommands);


            //Act
            Task cancelTask = viewModel.CancelQuest(position);

            //Assert
            Assert.IsNotNull(cancelTask);
            cancelTask.Wait();

            returnedCommand.VerifyAllExpectations();
            stateCommands.VerifyAllExpectations();
            repoCommands.VerifyAllExpectations();
            model.VerifyAllExpectations();
        }
    }
}
