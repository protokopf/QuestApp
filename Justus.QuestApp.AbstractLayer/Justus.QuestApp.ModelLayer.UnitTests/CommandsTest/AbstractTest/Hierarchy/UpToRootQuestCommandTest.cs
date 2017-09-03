using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.AbstractTest.Hierarchy
{
    [TestFixture]
    class UpToRootQuestCommandTest
    {
        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(UpToRootQuestCommand).IsSubclassOf(typeof(UpHierarchyQuestCommand)));
        }

        [Test]
        public void CtorNullTest()
        {
            //Arrange
            IQuestCommand questCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            Quest targer = QuestHelper.CreateQuest();

            //Act
            ArgumentNullException treeEx = Assert.Throws<ArgumentNullException>(() => new UpToRootQuestCommand(targer,null, questCommand));

            //Assert
            Assert.IsNotNull(treeEx);
            Assert.AreEqual("questTree", treeEx.ParamName);
        }

        [Test]
        public void ExecuteWithParentsTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent0 = new FakeQuest();
            Quest parent1 = new FakeQuest();
            Quest parent2 = new FakeQuest();
            root.Parent = null;
            parent0.Parent = root;
            parent1.Parent = parent0;
            parent2.Parent = parent1;

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.Root).Return(root).Repeat.Times(4);

            IQuestCommand innerCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            innerCommand.Expect(ic => ic.Execute(Arg<Quest>.Is.Equal(parent2))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Execute(Arg<Quest>.Is.Equal(parent1))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Execute(Arg<Quest>.Is.Equal(parent0))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Execute(Arg<Quest>.Is.Equal(root))).Repeat.Never();

            UpToRootQuestCommand command = new UpToRootQuestCommand(parent2,tree, innerCommand);


            //Act
            bool result = command.Execute();

            //Assert
            Assert.IsTrue(result);

            innerCommand.VerifyAllExpectations();
        }

        [Test]
        public void UndoWithParentsTest()
        {
            //Arrange
            Quest root = new FakeQuest();
            Quest parent0 = new FakeQuest();
            Quest parent1 = new FakeQuest();
            Quest parent2 = new FakeQuest();
            root.Parent = null;
            parent0.Parent = root;
            parent1.Parent = parent0;
            parent2.Parent = parent1;

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.Root).Return(root).Repeat.Times(4);

            IQuestCommand innerCommand = MockRepository.GenerateStrictMock<IQuestCommand>();
            innerCommand.Expect(ic => ic.Undo(Arg<Quest>.Is.Equal(parent2))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Undo(Arg<Quest>.Is.Equal(parent1))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Undo(Arg<Quest>.Is.Equal(parent0))).Repeat.Once().Return(true);
            innerCommand.Expect(ic => ic.Undo(Arg<Quest>.Is.Equal(root))).Repeat.Never();

            UpToRootQuestCommand command = new UpToRootQuestCommand(parent2, tree, innerCommand);


            //Act
            bool result = command.Undo();

            //Assert
            Assert.IsTrue(result);

            innerCommand.VerifyAllExpectations();
        }


    }
}
