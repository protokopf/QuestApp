using System;
using System.Collections.Generic;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.AbstractTest.Hierarchy
{
    [TestFixture]
    class DownHierarchyQuestCommandTest
    {
        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(DownHierarchyQuestCommand).IsSubclassOf(typeof(object)));
            Assert.IsTrue(typeof(ICommand).IsAssignableFrom(typeof(DownHierarchyQuestCommand)));
        }

        [Test]
        public void CtorNullTest()
        {
            //Arrange
            IQuestCommand questCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            Quest targer = QuestHelper.CreateQuest();

            //Act
            ArgumentNullException targetEx = Assert.Throws<ArgumentNullException>(() => new DownHierarchyQuestCommand(null, questCommand, questCommand));
            ArgumentNullException beforeQuestCommand = Assert.Throws<ArgumentNullException>(() => new DownHierarchyQuestCommand(targer, null, questCommand));
            ArgumentNullException afterQuestCommand = Assert.Throws<ArgumentNullException>(() => new DownHierarchyQuestCommand(targer, questCommand, null));

            //Assert
            Assert.IsNotNull(targetEx);
            Assert.AreEqual("quest", targetEx.ParamName);

            Assert.IsNotNull(beforeQuestCommand);
            Assert.AreEqual("beforeTraverseCommand", beforeQuestCommand.ParamName);

            Assert.IsNotNull(afterQuestCommand);
            Assert.AreEqual("afterTraverseCommand", afterQuestCommand.ParamName);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ExecuteOnQuestWithoutChildrenTest(bool beforeCommandResult)
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();
            IQuestCommand beforeQuestCommanduestCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            beforeQuestCommanduestCommand.Expect(bqc => bqc.Execute(Arg<Quest>.Is.Equal(quest)))
                .Repeat.Once()
                .Return(beforeCommandResult);

            IQuestCommand afterQuestCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            afterQuestCommand.Expect(aqc => aqc.Execute(Arg<Quest>.Is.Equal(quest)))
                .Repeat.Never();

            DownHierarchyQuestCommand command = new DownHierarchyQuestCommand(quest, beforeQuestCommanduestCommand,
                afterQuestCommand);

            //Act
            bool result = command.Execute();

            //Assert
            Assert.IsTrue(result);

            beforeQuestCommanduestCommand.VerifyAllExpectations();
            afterQuestCommand.VerifyAllExpectations();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ExecuteOnQuestWithChildrenTest(bool beforeCommandResult)
        {
            //Arrange
            Quest child = QuestHelper.CreateQuest();
            Quest quest = QuestHelper.CreateQuest();
            quest.Children = new List<Quest> {child};

            IQuestCommand beforeQuestCommanduestCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            beforeQuestCommanduestCommand.Expect(bqc => bqc.Execute(Arg<Quest>.Is.Equal(quest)))
                .Repeat.Once()
                .Return(beforeCommandResult);
            if (beforeCommandResult)
            {
                beforeQuestCommanduestCommand.Expect(bqc => bqc.Execute(Arg<Quest>.Is.Equal(child)))
                    .Repeat.Once()
                    .Return(true);
            }

            IQuestCommand afterQuestCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            if(beforeCommandResult)
            {
                afterQuestCommand.Expect(aqc => aqc.Execute(Arg<Quest>.Is.Equal(quest)))
                    .Repeat.Once()
                    .Return(true);
            }

            DownHierarchyQuestCommand command = new DownHierarchyQuestCommand(quest, beforeQuestCommanduestCommand,
                afterQuestCommand);

            //Act
            bool result = command.Execute();

            //Assert
            Assert.IsTrue(result);

            beforeQuestCommanduestCommand.VerifyAllExpectations();
            afterQuestCommand.VerifyAllExpectations();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void UndoOnQuestWithoutChildrenTest(bool beforeCommandResult)
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();
            IQuestCommand beforeQuestCommanduestCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            beforeQuestCommanduestCommand.Expect(bqc => bqc.Undo(Arg<Quest>.Is.Equal(quest)))
                .Repeat.Once()
                .Return(beforeCommandResult);

            IQuestCommand afterQuestCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            afterQuestCommand.Expect(aqc => aqc.Undo(Arg<Quest>.Is.Equal(quest)))
                .Repeat.Never();

            DownHierarchyQuestCommand command = new DownHierarchyQuestCommand(quest, beforeQuestCommanduestCommand,
                afterQuestCommand);

            //Act
            bool result = command.Undo();

            //Assert
            Assert.IsTrue(result);

            beforeQuestCommanduestCommand.VerifyAllExpectations();
            afterQuestCommand.VerifyAllExpectations();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void UndoOnQuestWithChildrenTest(bool beforeCommandResult)
        {
            //Arrange
            Quest child = QuestHelper.CreateQuest();
            Quest quest = QuestHelper.CreateQuest();
            quest.Children = new List<Quest> { child };

            IQuestCommand beforeQuestCommanduestCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            beforeQuestCommanduestCommand.Expect(bqc => bqc.Undo(Arg<Quest>.Is.Equal(quest)))
                .Repeat.Once()
                .Return(beforeCommandResult);
            if (beforeCommandResult)
            {
                beforeQuestCommanduestCommand.Expect(bqc => bqc.Undo(Arg<Quest>.Is.Equal(child)))
                    .Repeat.Once()
                    .Return(true);
            }

            IQuestCommand afterQuestCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            if (beforeCommandResult)
            {
                afterQuestCommand.Expect(aqc => aqc.Undo(Arg<Quest>.Is.Equal(quest)))
                    .Repeat.Once()
                    .Return(true);
            }

            DownHierarchyQuestCommand command = new DownHierarchyQuestCommand(quest, beforeQuestCommanduestCommand,
                afterQuestCommand);

            //Act
            bool result = command.Undo();

            //Assert
            Assert.IsTrue(result);

            beforeQuestCommanduestCommand.VerifyAllExpectations();
            afterQuestCommand.VerifyAllExpectations();
        }

        [Test]
        public void IsValidTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();

            IQuestCommand beforeQuestCommanduestCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            IQuestCommand afterQuestCommand = MockRepository.GenerateStrictMock<IQuestCommand>();

            DownHierarchyQuestCommand command = new DownHierarchyQuestCommand(quest, beforeQuestCommanduestCommand,
                afterQuestCommand);

            //Act
            bool result = command.IsValid();

            //Assert
            Assert.IsTrue(result);

            beforeQuestCommanduestCommand.VerifyAllExpectations();
            afterQuestCommand.VerifyAllExpectations();
        }

        [TestCase(true, true)]
        [TestCase(true,false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void CommitTestTest(bool beforeCommitResult, bool afterCommitResult)
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();

            IQuestCommand beforeQuestCommanduestCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            beforeQuestCommanduestCommand.Expect(bqc => bqc.Commit()).
                Repeat.Once().
                Return(beforeCommitResult);

            IQuestCommand afterQuestCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            afterQuestCommand.Expect(aqc => aqc.Commit()).
                Repeat.Once().
                Return(afterCommitResult);

            DownHierarchyQuestCommand command = new DownHierarchyQuestCommand(quest, beforeQuestCommanduestCommand,
                afterQuestCommand);

            //Act
            bool result = command.Commit();

            //Assert
            Assert.AreEqual(beforeCommitResult && afterCommitResult, result);

            beforeQuestCommanduestCommand.VerifyAllExpectations();
            afterQuestCommand.VerifyAllExpectations();
        }
    }
}
