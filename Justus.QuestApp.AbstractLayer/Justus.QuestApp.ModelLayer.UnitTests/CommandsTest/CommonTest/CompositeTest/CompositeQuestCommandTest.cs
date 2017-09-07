using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Common.Composite;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.CommonTest.CompositeTest
{
    [TestFixture]
    class CompositeQuestCommandTest
    {
        [Test]
        public void CtorNullTest()
        {
            //Arrange & Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new CompositeQuestCommand(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("innerCommands", ex.ParamName);
        }

        [Test]
        public void ExecuteOrderTest()
        {
            //Arrange
            Quest quest = new FakeQuest();
            Stack<int> stack = new Stack<int>();

            IQuestCommand firstOne = MockRepository.GenerateStrictMock<IQuestCommand>();
            firstOne.Expect(c => c.Execute(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once().
                Do(new Func<Quest,bool>(q =>
                {
                    stack.Push(1);
                    return true;
                }));

            IQuestCommand secondOne = MockRepository.GenerateStrictMock<IQuestCommand>();
            secondOne.Expect(c => c.Execute(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once().
                Do(new Func<Quest, bool>(q =>
                {
                    stack.Push(2);
                    return true;
                }));

            CompositeQuestCommand composite = new CompositeQuestCommand(new[] { firstOne, secondOne });

            //Act
            composite.Execute(quest);

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
            Quest quest = new FakeQuest();

            IQuestCommand firstOne = MockRepository.GenerateStrictMock<IQuestCommand>();
            firstOne.Expect(c => c.Undo(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once().
                Do(new Func<Quest, bool>((q) =>
                {
                    stack.Push(1);
                    return true;
                }));

            IQuestCommand secondOne = MockRepository.GenerateStrictMock<IQuestCommand>();
            secondOne.Expect(c => c.Undo(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once().
                Do(new Func<Quest, bool>(q =>
                {
                    stack.Push(2);
                    return true;
                }));

            CompositeQuestCommand composite = new CompositeQuestCommand(new[] { firstOne, secondOne });

            //Act
            composite.Undo(quest);

            //Assert
            Assert.AreEqual(1, stack.Pop());
            Assert.AreEqual(2, stack.Pop());

            firstOne.VerifyAllExpectations();
            secondOne.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteWithNullTest()
        {
            //Arrange
            Quest quest = null;

            IQuestCommand firstOne = MockRepository.GenerateStrictMock<IQuestCommand>();
            firstOne.Expect(c => c.Execute(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once().
                Return(true);

            IQuestCommand secondOne = MockRepository.GenerateStrictMock<IQuestCommand>();
            secondOne.Expect(c => c.Execute(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once().
                Return(true);

            CompositeQuestCommand composite = new CompositeQuestCommand(new[] { firstOne, secondOne });

            //Act
            Assert.DoesNotThrow(() => composite.Execute(quest));

            //Assert
            firstOne.VerifyAllExpectations();
            secondOne.VerifyAllExpectations();
        }

        [Test]
        public void UndoWithNullTest()
        {
            //Arrange
            Quest quest = null;

            IQuestCommand firstOne = MockRepository.GenerateStrictMock<IQuestCommand>();
            firstOne.Expect(c => c.Undo(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once().
                Return(true);

            IQuestCommand secondOne = MockRepository.GenerateStrictMock<IQuestCommand>();
            secondOne.Expect(c => c.Undo(Arg<Quest>.Is.Equal(quest))).
                Repeat.Once().
                Return(true);

            CompositeQuestCommand composite = new CompositeQuestCommand(new[] { firstOne, secondOne });

            //Act
            Assert.DoesNotThrow(() => composite.Undo(quest));

            //Assert
            firstOne.VerifyAllExpectations();
            secondOne.VerifyAllExpectations();
        }
    }
}
