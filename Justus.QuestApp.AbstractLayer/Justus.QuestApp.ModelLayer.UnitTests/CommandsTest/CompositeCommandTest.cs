using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.ModelLayer.Commands;
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
            firstOne.Expect(c => c.Execute()).Repeat.Once().Do(new Action((() =>
            {
                stack.Push(1);
            })));

            Command secondOne = MockRepository.GenerateStrictMock<Command>();
            secondOne.Expect(c => c.Execute()).Repeat.Once().Do(new Action((() =>
            {
                stack.Push(2);
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
            firstOne.Expect(c => c.Undo()).Repeat.Once().Do(new Action((() =>
            {
                stack.Push(1);
            })));

            Command secondOne = MockRepository.GenerateStrictMock<Command>();
            secondOne.Expect(c => c.Undo()).Repeat.Once().Do(new Action((() =>
            {
                stack.Push(2);
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
    }
}
