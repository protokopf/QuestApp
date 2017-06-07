using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.ModelLayer.Commands;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest
{
    [TestFixture]
    class CompositeCommandTest
    {
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

            Command firstOne = MockRepository.GenerateStrictMock<Command>();
            firstOne.Expect(c => c.Execute()).Repeat.Once().Do(new Func<bool>((() =>
            {
                stack.Push(1);
                return true;
            })));

            Command secondOne = MockRepository.GenerateStrictMock<Command>();
            secondOne.Expect(c => c.Execute()).Repeat.Once().Do(new Func<bool>((() =>
            {
                stack.Push(2);
                return true;
            })));

            Command composite = new CompositeCommand(new []{firstOne, secondOne});

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

            Command firstOne = MockRepository.GenerateStrictMock<Command>();
            firstOne.Expect(c => c.Undo()).Repeat.Once().Return(true).Do(new Func<bool>((() =>
            {
                stack.Push(1);
                return true;
            })));

            Command secondOne = MockRepository.GenerateStrictMock<Command>();
            secondOne.Expect(c => c.Undo()).Repeat.Once().Return(true).Do(new Func<bool>((() =>
            {
                stack.Push(2);
                return true;
            })));

            Command composite = new CompositeCommand(new[] { firstOne, secondOne });

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

            Command firstOne = MockRepository.GenerateStrictMock<Command>();
            firstOne.Expect(c => c.IsValid()).Repeat.Once().Return(true);

            Command secondOne = MockRepository.GenerateStrictMock<Command>();
            secondOne.Expect(c => c.IsValid()).Repeat.Once().Return(true);

            Command composite = new CompositeCommand(new[] { firstOne, secondOne });

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
            Command firstOne = MockRepository.GenerateStrictMock<Command>();
            firstOne.Expect(c => c.IsValid()).Repeat.Once().Return(true);

            Command secondOne = MockRepository.GenerateStrictMock<Command>();
            secondOne.Expect(c => c.IsValid()).Repeat.Once().Return(false);

            Command composite = new CompositeCommand(new[] { firstOne, secondOne });

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
            Command[] commands = new Command[count];
            for (int i = 0; i < count; ++i)
            {
                commands[i] = MockRepository.GenerateStrictMock<Command>();
                commands[i].Expect(cm => cm.Execute()).Return(executeResults[i]).Repeat.Once();
            }

            Command composite = new CompositeCommand(commands);

            //Act
            bool result = composite.Execute();

            //Assert
            Assert.IsFalse(result);

            foreach (Command command in commands)
            {
                command.VerifyAllExpectations();
            }
        }
    }
}
