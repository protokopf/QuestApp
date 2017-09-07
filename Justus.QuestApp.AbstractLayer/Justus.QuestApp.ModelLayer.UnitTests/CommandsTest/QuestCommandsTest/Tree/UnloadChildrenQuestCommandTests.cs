using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.QuestCommands.Tree;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestCommandsTest.Tree
{
    [TestFixture]
    class UnloadChildrenQuestCommandTests
    {
        [Test]
        public void CtorNullTest()
        {
            //Arrange

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new UnloadChildrenQuestCommand(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("questTree", ex.ParamName);
        }

        [Test]
        public void ExecuteTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.UnloadChildren(Arg<Quest>.Is.Equal(quest))).Repeat.Once();

            UnloadChildrenQuestCommand command = new UnloadChildrenQuestCommand(tree);

            //Act
            bool result = command.Execute(quest);

            //Assert
            Assert.IsTrue(result);

            tree.VerifyAllExpectations();
        }

        [Test]
        public void UndoTest()
        {
            //Arrange
            Quest quest = QuestHelper.CreateQuest();

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.LoadChildren(Arg<Quest>.Is.Equal(quest))).Repeat.Once();

            UnloadChildrenQuestCommand command = new UnloadChildrenQuestCommand(tree);

            //Act
            bool result = command.Undo(quest);

            //Assert
            Assert.IsTrue(result);

            tree.VerifyAllExpectations();
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();

            UnloadChildrenQuestCommand command = new UnloadChildrenQuestCommand(tree);

            //Act
            bool result = command.Commit();

            //Assert
            Assert.IsTrue(result);

            tree.VerifyAllExpectations();
        }
    }
}
