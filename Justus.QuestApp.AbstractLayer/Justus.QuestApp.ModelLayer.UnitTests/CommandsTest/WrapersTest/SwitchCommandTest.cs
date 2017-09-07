using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.WrapersTest
{
    [TestFixture]
    internal class SwitchCommandTest
    {
        [Test]
        public void ExecuteTest()
        {
            //Arrange
            ICommand innerCommand = MockRepository.GenerateStrictMock<ICommand>();
            innerCommand.Expect(ic => ic.Execute()).
                Repeat.Once().
                Return(true);

            SwitchCommand command = new SwitchCommand(innerCommand);

            //Act
            bool executeResult = command.Execute();

            //Assert
            Assert.IsTrue(executeResult);

            innerCommand.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteAfterCommitTest()
        {
            //Arrange
            ICommand innerCommand = MockRepository.GenerateStrictMock<ICommand>();
            innerCommand.Expect(ic => ic.Commit()).
                Repeat.Once().
                Return(true);
            innerCommand.Expect(ic => ic.Execute()).
                Repeat.Never();

            SwitchCommand command = new SwitchCommand(innerCommand);

            //Act
            bool commitResult = command.Commit();
            bool executeResult = command.Execute();

            //Assert
            Assert.IsTrue(commitResult);
            Assert.IsFalse(executeResult);

            innerCommand.VerifyAllExpectations();
        }

        [Test]
        public void UndoWithoutExecuteTest()
        {
            //Arrange
            ICommand innerCommand = MockRepository.GenerateStrictMock<ICommand>();
            innerCommand.Expect(ic => ic.Undo()).
                Repeat.Never();

            SwitchCommand command = new SwitchCommand(innerCommand);

            //Act
            bool undoResult = command.Undo();

            //Assert
            Assert.IsFalse(undoResult);

            innerCommand.VerifyAllExpectations();
        }

        [Test]
        public void UndoAfterExecuteTest()
        {
            //Arrange
            ICommand innerCommand = MockRepository.GenerateStrictMock<ICommand>();
            innerCommand.Expect(ic => ic.Execute()).
                Repeat.Once().
                Return(true);
            innerCommand.Expect(ic => ic.Undo()).
                Repeat.Once().
                Return(true);

            SwitchCommand command = new SwitchCommand(innerCommand);

            //Act
            bool execteResult = command.Execute();
            bool undoResult = command.Undo();

            //Assert
            Assert.IsTrue(execteResult);
            Assert.IsTrue(undoResult);

            innerCommand.VerifyAllExpectations();
        }

        [Test]
        public void UndoAfterExecuteCommitTest()
        {
            //Arrange
            ICommand innerCommand = MockRepository.GenerateStrictMock<ICommand>();
            innerCommand.Expect(ic => ic.Execute()).
                Repeat.Once().
                Return(true);
            innerCommand.Expect(ic => ic.Commit()).
                Repeat.Once().
                Return(true);
            innerCommand.Expect(ic => ic.Undo()).
                Repeat.Never();

            SwitchCommand command = new SwitchCommand(innerCommand);

            //Act
            bool execteResult = command.Execute();
            bool commitResult = command.Commit();
            bool undoResult = command.Undo();

            //Assert
            Assert.IsTrue(execteResult);
            Assert.IsTrue(commitResult);
            Assert.IsFalse(undoResult);

            innerCommand.VerifyAllExpectations();
        }

        [Test]
        public void IsValidTest()
        {
            //Arrange
            ICommand innerCommand = MockRepository.GenerateStrictMock<ICommand>();
            innerCommand.Expect(ic => ic.IsValid()).
                Repeat.Once().
                Return(true);

            SwitchCommand command = new SwitchCommand(innerCommand);

            //Act
            bool isValidResult = command.IsValid();

            //Assert
            Assert.IsTrue(isValidResult);

            innerCommand.VerifyAllExpectations();
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            ICommand innerCommand = MockRepository.GenerateStrictMock<ICommand>();
            innerCommand.Expect(ic => ic.Commit()).
                Repeat.Once().
                Return(true);

            SwitchCommand command = new SwitchCommand(innerCommand);

            //Act
            bool commitResult = command.Commit();

            //Assert
            Assert.IsTrue(commitResult);

            innerCommand.VerifyAllExpectations();
        }

        [Test]
        public void DoubleCommitTest()
        {
            //Arrange
            ICommand innerCommand = MockRepository.GenerateStrictMock<ICommand>();
            innerCommand.Expect(ic => ic.Commit()).
                Repeat.Once().
                Return(true);

            SwitchCommand command = new SwitchCommand(innerCommand);

            //Act
            bool commitResult = command.Commit();
            bool secondCommitResult = command.Commit();

            //Assert
            Assert.IsTrue(commitResult);
            Assert.IsFalse(secondCommitResult);

            innerCommand.VerifyAllExpectations();
        }
    }
}
