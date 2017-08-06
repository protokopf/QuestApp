using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Commands.Wrappers;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.WrapersTest
{
    [TestFixture]
    class RecountQuestProgressCommandWrapperTest
    {
        [Test]
        public void CtorNullTest()
        {
            //Arrange
            ICommand command = MockRepository.GenerateStrictMock<ICommand>();
            Quest target = new FakeQuest();
            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();

            //Act
            ArgumentNullException commandNullEx = Assert.Throws<ArgumentNullException>(() => new RecountQuestProgressCommandWrapper(
                null,
                target,
                recounter));
            ArgumentNullException targetNullEx = Assert.Throws<ArgumentNullException>(() => new RecountQuestProgressCommandWrapper(
                command,
                null,
                recounter));
            ArgumentNullException recounterNullEx = Assert.Throws<ArgumentNullException>(() => new RecountQuestProgressCommandWrapper(
                command,
                target,
                null));

            //Assert
            Assert.IsNotNull(commandNullEx);
            Assert.IsNotNull(targetNullEx);
            Assert.IsNotNull(targetNullEx);

            Assert.AreEqual("innerCommand", commandNullEx.ParamName);
            Assert.AreEqual("target", targetNullEx.ParamName);
            Assert.AreEqual("recountStrategy", recounterNullEx.ParamName);
        }

        [Test]
        public void ExecuteProgressWontBeRecountedIfInnerCommandReturnsFalseTest()
        {
            //Arrange
            ICommand command = MockRepository.GenerateStrictMock<ICommand>();
            command.Expect(cm => cm.Execute()).Return(false).Repeat.Once();

            Quest target = new FakeQuest();

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();
            recounter.Expect(rc => rc.RecountProgress(Arg<Quest>.Is.Equal(target))).Repeat.Never();

            RecountQuestProgressCommandWrapper wrapper = new RecountQuestProgressCommandWrapper(command, target, recounter);

            //Act
            wrapper.Execute();

            //Assert
            command.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteProgressWillBeRecountedIfInnerCommandReturnsTrueTest()
        {
            //Arrange
            ICommand command = MockRepository.GenerateStrictMock<ICommand>();
            command.Expect(cm => cm.Execute()).Return(true).Repeat.Once();

            Quest target = new FakeQuest();

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();
            recounter.Expect(rc => rc.RecountProgress(Arg<Quest>.Is.Equal(target))).Repeat.Once();

            RecountQuestProgressCommandWrapper wrapper = new RecountQuestProgressCommandWrapper(command, target, recounter);

            //Act
            wrapper.Execute();

            //Assert
            command.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }

        [Test]
        public void UndoProgressWontBeRecountedIfInnerCommandReturnsFalseTest()
        {
            //Arrange
            ICommand command = MockRepository.GenerateStrictMock<ICommand>();
            command.Expect(cm => cm.Undo()).Return(false).Repeat.Once();

            Quest target = new FakeQuest();

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();
            recounter.Expect(rc => rc.RecountProgress(Arg<Quest>.Is.Equal(target))).Repeat.Never();

            RecountQuestProgressCommandWrapper wrapper = new RecountQuestProgressCommandWrapper(command, target, recounter);

            //Act
            wrapper.Undo();

            //Assert
            command.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }

        [Test]
        public void UndoProgressWillBeRecountedIfInnerCommandReturnsTrueTest()
        {
            //Arrange
            ICommand command = MockRepository.GenerateStrictMock<ICommand>();
            command.Expect(cm => cm.Undo()).Return(true).Repeat.Once();

            Quest target = new FakeQuest();

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();
            recounter.Expect(rc => rc.RecountProgress(Arg<Quest>.Is.Equal(target))).Repeat.Once();

            RecountQuestProgressCommandWrapper wrapper = new RecountQuestProgressCommandWrapper(command, target, recounter);

            //Act
            wrapper.Undo();

            //Assert
            command.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }

        [Test]
        public void IsValidTest()
        {
            //Arrange
            ICommand command = MockRepository.GenerateStrictMock<ICommand>();
            command.Expect(cm => cm.IsValid()).
                Return(true).
                Repeat.Once();

            Quest target = new FakeQuest();

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();

            RecountQuestProgressCommandWrapper wrapper = new RecountQuestProgressCommandWrapper(command, target, recounter);

            //Act
            bool isValidResult = wrapper.IsValid();

            //Assert
            Assert.IsTrue(isValidResult);

            command.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            ICommand command = MockRepository.GenerateStrictMock<ICommand>();
            command.Expect(cm => cm.Commit()).
                Return(true).
                Repeat.Once();

            Quest target = new FakeQuest();

            IQuestProgressRecounter recounter = MockRepository.GenerateStrictMock<IQuestProgressRecounter>();

            RecountQuestProgressCommandWrapper wrapper = new RecountQuestProgressCommandWrapper(command, target, recounter);

            //Act
            bool commitResult = wrapper.Commit();

            //Assert
            Assert.IsTrue(commitResult);

            command.VerifyAllExpectations();
            recounter.VerifyAllExpectations();
        }
    }
}
