using System;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.AbstractTest.Hierarchy
{
    [TestFixture]
    class CurrentQuestCommandTest
    {
        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(CurrentQuestCommand).IsSubclassOf(typeof(object)));
        }

        [Test]
        public void CtorNullTest()
        {
            //Arrange
            IQuestCommand questCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            Quest targer = QuestHelper.CreateQuest();

            //Act
            ArgumentNullException targetEx = Assert.Throws<ArgumentNullException>(() => new CurrentQuestCommand(null, questCommand));
            ArgumentNullException commandEx = Assert.Throws<ArgumentNullException>(() => new CurrentQuestCommand(targer, null));

            //Assert
            Assert.IsNotNull(targetEx);
            Assert.AreEqual("quest", targetEx.ParamName);

            Assert.IsNotNull(commandEx);
            Assert.AreEqual("questCommand", commandEx.ParamName);
        }

        [Test]
        public void ExecuteTest()
        {
            //Arrange
            Quest target = QuestHelper.CreateQuest();

            IQuestCommand questCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            questCommand.Expect(qc => qc.Execute(Arg<Quest>.Is.Equal(target))).
                Repeat.Once().
                Return(true);

            CurrentQuestCommand command = new CurrentQuestCommand(target, questCommand);

            //Act
            bool result = command.Execute();


            //Assert
            Assert.IsTrue(result);

            questCommand.VerifyAllExpectations();
        }

        [Test]
        public void UndoTest()
        {
            //Arrange
            Quest target = QuestHelper.CreateQuest();

            IQuestCommand questCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            questCommand.Expect(qc => qc.Undo(Arg<Quest>.Is.Equal(target))).
                Repeat.Once().
                Return(true);

            CurrentQuestCommand command = new CurrentQuestCommand(target, questCommand);

            //Act
            bool result = command.Undo();


            //Assert
            Assert.IsTrue(result);

            questCommand.VerifyAllExpectations();
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            Quest target = QuestHelper.CreateQuest();

            IQuestCommand questCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            questCommand.Expect(qc => qc.Commit()).
                Repeat.Once().
                Return(true);

            CurrentQuestCommand command = new CurrentQuestCommand(target, questCommand);

            //Act
            bool result = command.Commit();


            //Assert
            Assert.IsTrue(result);

            questCommand.VerifyAllExpectations();
        }

        [Test]
        public void IsValidTest()
        {
            //Arrange
            Quest target = QuestHelper.CreateQuest();

            IQuestCommand questCommand = MockRepository.GenerateStrictMock<IQuestCommand>();

            CurrentQuestCommand command = new CurrentQuestCommand(target, questCommand);

            //Act
            bool result = command.IsValid();


            //Assert
            Assert.IsTrue(result);

            questCommand.VerifyAllExpectations();
        }
    }
}
