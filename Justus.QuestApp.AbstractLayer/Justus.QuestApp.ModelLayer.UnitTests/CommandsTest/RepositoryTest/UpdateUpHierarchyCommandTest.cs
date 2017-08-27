using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.Commands.Repository;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.RepositoryTest
{
    [TestFixture]
    class UpdateUpHierarchyCommandTest
    {
        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(UpdateUpHierarchyCommand).IsSubclassOf(typeof(UpHierarchyQuestCommand)));
        }

        [Test]
        public void CtorNullTest()
        {
            //Arrange
            Quest target = new FakeQuest();

            //Act
            ArgumentNullException treeNullEx = Assert.Throws<ArgumentNullException>(() => new UpdateUpHierarchyCommand(
                target,
                null));

            //Assert
            Assert.IsNotNull(treeNullEx);

            Assert.AreEqual("questTree", treeNullEx.ParamName);
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            Quest target = new FakeQuest();

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();
            tree.Expect(tr => tr.Save()).Repeat.Once();

            UpdateUpHierarchyCommand command = new UpdateUpHierarchyCommand(target, tree);

            //Act
            bool commitResult = command.Commit();

            //Assert
            Assert.IsTrue(commitResult);

            tree.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteTest()
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
            tree.Expect(tr => tr.Root).
                Repeat.Times(4).
                Return(root);
            tree.Expect(tr => tr.Update(Arg<Quest>.Is.Equal(parent2))).
                Repeat.Once();
            tree.Expect(tr => tr.Update(Arg<Quest>.Is.Equal(parent1))).
                Repeat.Once();
            tree.Expect(tr => tr.Update(Arg<Quest>.Is.Equal(parent0))).
                Repeat.Once();

            UpdateUpHierarchyCommand command = new UpdateUpHierarchyCommand(parent2, tree);


            //Act
            bool result = command.Execute();

            //Assert
            Assert.IsTrue(result);
            tree.VerifyAllExpectations();
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
            tree.Expect(tr => tr.Root).
                Repeat.Times(4).
                Return(root);
            tree.Expect(tr => tr.RevertUpdate(Arg<Quest>.Is.Equal(parent2))).
                Repeat.Once();
            tree.Expect(tr => tr.RevertUpdate(Arg<Quest>.Is.Equal(parent1))).
                Repeat.Once();
            tree.Expect(tr => tr.RevertUpdate(Arg<Quest>.Is.Equal(parent0))).
                Repeat.Once();

            UpdateUpHierarchyCommand command = new UpdateUpHierarchyCommand(parent2, tree);


            //Act
            bool result = command.Undo();

            //Assert
            Assert.IsTrue(result);
            tree.VerifyAllExpectations();
        }
    }
}
