using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.WrapersTest
{
    [TestFixture]
    class CompositeCommandTest
    {
        private ICommand[] CreateCommands(int count, Action<ICommand> actionWithCommand)
        {
            ICommand[] commands = new ICommand[count];
            for (int i = 0; i < count; ++i)
            {
                commands[i] = MockRepository.GenerateStrictMock<ICommand>();
                actionWithCommand(commands[i]);
            }
            return commands;
        }

        [Test]
        public void CtorNullTest()
        {
            //Arrange & Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new CompositeCommand(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("commands", ex.ParamName);
        }

        [Test]
        public void ExecuteOrderTest()
        {
            //Arrange
            Stack<int> stack = new Stack<int>();

            ICommand firstOne = MockRepository.GenerateStrictMock<ICommand>();
            firstOne.Expect(c => c.Execute()).Repeat.Once().Do(new Func<bool>((() =>
            {
                stack.Push(1);
                return true;
            })));

            ICommand secondOne = MockRepository.GenerateStrictMock<ICommand>();
            secondOne.Expect(c => c.Execute()).Repeat.Once().Do(new Func<bool>((() =>
            {
                stack.Push(2);
                return true;
            })));

            ICommand composite = new CompositeCommand(new []{firstOne, secondOne});

            //Act
            composite.Execute();

            //Assert
            Assert.AreEqual(2, stack.Pop());
            Assert.AreEqual(1, stack.Pop());

            firstOne.VerifyAllExpectations();
            secondOne.VerifyAllExpectations();
        }

        [Test]
        public void UndoOrderTest()
        {
            //Arrange
            Stack<int> stack = new Stack<int>();

            ICommand firstOne = MockRepository.GenerateStrictMock<ICommand>();
            firstOne.Expect(c => c.Undo()).Repeat.Once().Return(true).Do(new Func<bool>((() =>
            {
                stack.Push(1);
                return true;
            })));

            ICommand secondOne = MockRepository.GenerateStrictMock<ICommand>();
            secondOne.Expect(c => c.Undo()).Repeat.Once().Return(true).Do(new Func<bool>((() =>
            {
                stack.Push(2);
                return true;
            })));

            ICommand composite = new CompositeCommand(new[] { firstOne, secondOne });

            //Act
            composite.Undo();

            //Assert
            Assert.AreEqual(1, stack.Pop());
            Assert.AreEqual(2, stack.Pop());

            firstOne.VerifyAllExpectations();
            secondOne.VerifyAllExpectations();
        }

        [Test]
        public void ValidTrueTest()
        {
            //Arrange

            ICommand firstOne = MockRepository.GenerateStrictMock<ICommand>();
            firstOne.Expect(c => c.IsValid()).Repeat.Once().Return(true);

            ICommand secondOne = MockRepository.GenerateStrictMock<ICommand>();
            secondOne.Expect(c => c.IsValid()).Repeat.Once().Return(true);

            ICommand composite = new CompositeCommand(new[] { firstOne, secondOne });

            //Act
            bool isValid = composite.IsValid();

            //Assert
            Assert.IsTrue(isValid);

            firstOne.VerifyAllExpectations();
            secondOne.VerifyAllExpectations();
        }

        [Test]
        public void ValidFalseTest()
        {
            //Arrange
            ICommand firstOne = MockRepository.GenerateStrictMock<ICommand>();
            firstOne.Expect(c => c.IsValid()).Repeat.Once().Return(true);

            ICommand secondOne = MockRepository.GenerateStrictMock<ICommand>();
            secondOne.Expect(c => c.IsValid()).Repeat.Once().Return(false);

            ICommand composite = new CompositeCommand(new[] { firstOne, secondOne });

            //Act
            bool isValid = composite.IsValid();

            //Assert
            Assert.IsFalse(isValid);

            firstOne.VerifyAllExpectations();
            secondOne.VerifyAllExpectations();
        }

        [TestCase(new[] { true, true, true, false})]
        [TestCase(new[] { true, true, false, false})]
        [TestCase(new[] { true, false, false, false})]
        [TestCase(new[] { false, false, false, false})]
        public void IfOneOrMoreExecutesWithFalseResultFalseTest(bool[] executeResults)
        {
            //Arrange
            int count = executeResults.Length;
            ICommand[] commands = new ICommand[count];
            for (int i = 0; i < count; ++i)
            {
                commands[i] = MockRepository.GenerateStrictMock<ICommand>();
                commands[i].Expect(cm => cm.Execute()).Return(executeResults[i]).Repeat.Once();
            }

            ICommand composite = new CompositeCommand(commands);

            //Act
            bool result = composite.Execute();

            //Assert
            Assert.IsFalse(result);

            foreach (ICommand command in commands)
            {
                command.VerifyAllExpectations();
            }
        }

        [TestCase(new[] { true, true, true, false })]
        [TestCase(new[] { true, true, false, false })]
        [TestCase(new[] { true, false, false, false })]
        [TestCase(new[] { false, false, false, false })]
        public void IfOneOrMoreUndoWithFalseResultFalseTest(bool[] undoResults)
        {
            //Arrange
            int count = undoResults.Length;
            ICommand[] commands = new ICommand[count];
            for (int i = 0; i < count; ++i)
            {
                commands[i] = MockRepository.GenerateStrictMock<ICommand>();
                commands[i].Expect(cm => cm.Undo()).Return(undoResults[i]).Repeat.Once();
            }

            ICommand composite = new CompositeCommand(commands);

            //Act
            bool result = composite.Undo();

            //Assert
            Assert.IsFalse(result);

            foreach (ICommand command in commands)
            {
                command.VerifyAllExpectations();
            }
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            ICommand[] commands = CreateCommands(5, (cm) =>
            {
                cm.Expect(c => c.Commit()).Repeat.Once().Return(true);
            });

            CompositeCommand compositeCommand = new CompositeCommand(commands);

            //Act
            bool commitResult = compositeCommand.Commit();

            //Assert
            Assert.IsTrue(commitResult);

            foreach (ICommand command in commands)
            {
                command.VerifyAllExpectations();
            }
        }

        [TestCase(new[] { true, true, true, false })]
        [TestCase(new[] { true, true, false, false })]
        [TestCase(new[] { true, false, false, false })]
        [TestCase(new[] { false, false, false, false })]
        public void IfOneOrMoreCommitWithFalseResultFalseTest(bool[] commitResults)
        {
            //Arrange
            int count = commitResults.Length;
            ICommand[] commands = new ICommand[count];
            for (int i = 0; i < count; ++i)
            {
                commands[i] = MockRepository.GenerateStrictMock<ICommand>();
                commands[i].Expect(cm => cm.Commit()).
                    Return(commitResults[i]).
                    Repeat.Once();
                if (!commitResults[i])
                {
                    break;
                }
                
            }

            ICommand composite = new CompositeCommand(commands);

            //Act
            bool result = composite.Commit();

            //Assert
            Assert.IsFalse(result);

            foreach (ICommand command in commands)
            {
                command?.VerifyAllExpectations();
            }
        }
    }
}
