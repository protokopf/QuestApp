using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Command command = MockRepository.GenerateStrictMock<Command>();
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
            Command command = MockRepository.GenerateStrictMock<Command>();
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
            Command command = MockRepository.GenerateStrictMock<Command>();
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
            Command command = MockRepository.GenerateStrictMock<Command>();
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
            Command command = MockRepository.GenerateStrictMock<Command>();
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
    }
}
