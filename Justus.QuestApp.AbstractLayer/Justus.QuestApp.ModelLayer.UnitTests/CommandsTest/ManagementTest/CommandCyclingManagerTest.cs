using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.ModelLayer.Commands.Management;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.ManagementTest
{
    [TestFixture]
    class CommandCyclingManagerTest
    {
        [TestCase(-1)]
        [TestCase(0)]
        public void InitializeFailsTest(int size)
        {
            //Arrange && Act
            IndexOutOfRangeException ex = Assert.Throws<IndexOutOfRangeException>(() => new CommandCyclingManager(size));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("ERR_COMMAND_MGR_INDX", ex.Message);
        }

        [Test]
        public void AddNullCommandTest()
        {
            //Arrange
            ICommandManager manager = new CommandCyclingManager(2);

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => manager.Add(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("commandToAdd", ex.ParamName);
        }

        [Test]
        public void AddDoUndoCommandTest()
        {
            //Arrange
            Command first = MockRepository.GenerateStrictMock<Command>();
            first.Expect(c => c.Execute()).Repeat.Once();
            first.Expect(c => c.Undo()).Repeat.Once();
            first.Expect(c => c.IsValid()).Return(true).Repeat.Once();

            Command second = MockRepository.GenerateStrictMock<Command>();
            Command third = MockRepository.GenerateStrictMock<Command>();

            ICommandManager manager = new CommandCyclingManager(3);

            //Act
            manager.Add(first);
            manager.Add(second);
            manager.Add(third);

            manager.Do();
            manager.Undo();

            //Assert
            first.VerifyAllExpectations();
        }

        [Test]
        public void AddNotExecuteCommandsTest()
        {
            //Arrange
            Command first = MockRepository.GenerateStrictMock<Command>();
            first.Expect(c => c.Execute()).Repeat.Never();
            first.Expect(c => c.IsValid()).Return(true).Repeat.Never();

            Command second = MockRepository.GenerateStrictMock<Command>();
            second.Expect(c => c.Execute()).Repeat.Never();
            second.Expect(c => c.IsValid()).Return(true).Repeat.Never();

            ICommandManager manager = new CommandCyclingManager(2);

            //Act
            manager.Add(first);
            manager.Add(second);

            //Assert
            first.VerifyAllExpectations();
            second.VerifyAllExpectations();
        }

        [Test]
        public void ThirdCommandOverlapsFirstOneTest()
        {
            //Arrange
            Command first = MockRepository.GenerateStrictMock<Command>();
            first.Expect(c => c.Execute()).Repeat.Never();
            first.Expect(c => c.IsValid()).Return(true).Repeat.Never();

            Command second = MockRepository.GenerateStrictMock<Command>();
            second.Expect(c => c.Execute()).Repeat.Once();
            second.Expect(c => c.IsValid()).Return(true).Repeat.Once();

            Command third = MockRepository.GenerateStrictMock<Command>();
            third.Expect(c => c.Execute()).Repeat.Once();
            third.Expect(c => c.IsValid()).Return(true).Repeat.Once();

            ICommandManager manager = new CommandCyclingManager(2);

            //Act
            manager.Add(first);
            manager.Add(second);
            manager.Add(third);

            manager.Do();
            manager.Do();
            manager.Do();

            //Assert
            first.VerifyAllExpectations();
            second.VerifyAllExpectations();
            third.VerifyAllExpectations();
        }

        [Test]
        public void DoMoreThanCommandsCountTest()
        {
            //Arrange
            Command first = MockRepository.GenerateStrictMock<Command>();
            first.Expect(c => c.Execute()).Repeat.Once();
            first.Expect(c => c.IsValid()).Return(true).Repeat.Once();

            Command second = MockRepository.GenerateStrictMock<Command>();
            second.Expect(c => c.Execute()).Repeat.Once();
            second.Expect(c => c.IsValid()).Return(true).Repeat.Once();

            Command third = MockRepository.GenerateStrictMock<Command>();
            third.Expect(c => c.Execute()).Repeat.Once();
            third.Expect(c => c.IsValid()).Return(true).Repeat.Once();

            ICommandManager manager = new CommandCyclingManager(3);

            //Act
            manager.Add(first);
            manager.Add(second);
            manager.Add(third);

            manager.Do();
            manager.Do();
            manager.Do();
            manager.Do();

            //Assert
            first.VerifyAllExpectations();
            second.VerifyAllExpectations();
            third.VerifyAllExpectations();
        }

        [Test]
        public void UndoMoreThenDoTest()
        {
            //Arrange
            Command first = MockRepository.GenerateStrictMock<Command>();
            first.Expect(c => c.Execute()).Repeat.Once();
            first.Expect(c => c.Undo()).Repeat.Once();
            first.Expect(c => c.IsValid()).Return(true).Repeat.Once();

            Command second = MockRepository.GenerateStrictMock<Command>();
            second.Expect(c => c.Execute()).Repeat.Once();
            second.Expect(c => c.Undo()).Repeat.Once();
            second.Expect(c => c.IsValid()).Return(true).Repeat.Once();

            Command third = MockRepository.GenerateStrictMock<Command>();
            third.Expect(c => c.Execute()).Repeat.Never();
            third.Expect(c => c.Undo()).Repeat.Never();
            third.Expect(c => c.IsValid()).Return(true).Repeat.Never();

            ICommandManager manager = new CommandCyclingManager(3);

            //Act
            manager.Add(first);
            manager.Add(second);
            manager.Add(third);

            manager.Do();
            manager.Do();

            manager.Undo();
            manager.Undo();
            manager.Undo();

            //Assert
            first.VerifyAllExpectations();
        }

        [Test]
        public void DoMoreThanUndo()
        {
            //Arrange
            Command first = MockRepository.GenerateStrictMock<Command>();
            first.Expect(c => c.Execute()).Repeat.Once();
            first.Expect(c => c.IsValid()).Return(true).Repeat.Once();

            Command second = MockRepository.GenerateStrictMock<Command>();
            second.Expect(c => c.Execute()).Repeat.Once();
            second.Expect(c => c.IsValid()).Return(true).Repeat.Once();


            ICommandManager manager = new CommandCyclingManager(2);

            //Act
            manager.Add(first);
            manager.Add(second);

            manager.Do();
            manager.Do();
            manager.Do();

            //Assert
            first.VerifyAllExpectations();
            second.VerifyAllExpectations();
        }
    }
}
