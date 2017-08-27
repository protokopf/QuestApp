using System;
using System.Collections.Generic;
using System.Linq;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.QuestTree;
using Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.Commands.Other;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.OtherTest
{
    [TestFixture]
    class RecountQuestProgressCommandTest
    {  
        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(RecountQuestProgressCommand).IsSubclassOf(typeof(UpHierarchyQuestCommand)));
        }

        [Test]
        public void CtorNullTest()
        {
            //Arrange
            Quest target = new FakeQuest();

            //Act
            ArgumentNullException treeNullEx = Assert.Throws<ArgumentNullException>(() => new RecountQuestProgressCommand(
                target,
                null));

            //Assert
            Assert.IsNotNull(treeNullEx);

            Assert.AreEqual("questTree", treeNullEx.ParamName);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ExecuteQuestWithOneParentWithoutSiblingsTest(bool isQuestDone)
        {
            //Arrange
            double progress = isQuestDone ? 1 : 0;

            Quest root = QuestHelper.CreateQuest();

            Quest parent = QuestHelper.CreateQuest();
            parent.Parent = root;
            parent.Children = new List<Quest>
            {
                QuestHelper.CreateQuest()
            };

            Quest onlyChild = parent.Children[0];
            onlyChild.State = isQuestDone ? State.Done : State.Idle;
            onlyChild.Parent = parent;

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(rp => rp.Root).
                Return(root).
                Repeat.Times(3);

            RecountQuestProgressCommand recounter = new RecountQuestProgressCommand(onlyChild, repository);

            //Act
            bool result = recounter.Execute();

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(progress, parent.Progress);

            repository.VerifyAllExpectations();
        }

        [TestCase(new double[] { 0, 0, 0 })]
        [TestCase(new double[] { 0, 0, 1 })]
        [TestCase(new double[] { 0, 1, 1 })]
        [TestCase(new double[] { 1, 1, 1 })]
        public void ExecuteQuestWithParentAndSiblingsTest(double[] progresses)
        {
            //Arrange
            int count = progresses.Length;
            Quest root = QuestHelper.CreateQuest();

            Quest parent = QuestHelper.CreateQuest();
            parent.Parent = root;
            parent.Children = new List<Quest>();
            for (int i = 0; i < count; ++i)
            {
                Quest q = new Quest { Progress = progresses[i] };
                parent.Children.Add(q);
            }

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(rp => rp.Root).
                Return(root).
                Repeat.Times(2);

            RecountQuestProgressCommand recounter = new RecountQuestProgressCommand(parent,repository);



            //Act
            recounter.Execute();

            //Assert
            double expectedProgress = progresses.Average(d => d);
            Assert.AreEqual(expectedProgress, parent.Progress);

            repository.VerifyAllExpectations();
        }

        [TestCase(new double[] { 0, 0, 0 })]
        [TestCase(new double[] { 0, 0, 1 })]
        [TestCase(new double[] { 0, 1, 1 })]
        [TestCase(new double[] { 1, 1, 1 })]
        public void UndoTest(double[] progresses)
        {
            //Arrange
            double parentProgress = 0.33;

            int count = progresses.Length;
            Quest root = QuestHelper.CreateQuest();

            Quest parent = QuestHelper.CreateQuest();
            parent.Progress = parentProgress;
            parent.Parent = root;
            parent.Children = new List<Quest>();

            IQuestTree repository = MockRepository.GenerateStrictMock<IQuestTree>();
            repository.Expect(rp => rp.Root).
                Return(root).
                Repeat.Times(4);

            RecountQuestProgressCommand recounter = new RecountQuestProgressCommand(parent, repository);

            for (int i = 0; i < count; ++i)
            {
                Quest q = new Quest { Progress = progresses[i] };
                parent.Children.Add(q);
            }

            //Act
            recounter.Execute();
            recounter.Undo();

            //Assert
            Assert.AreEqual(parentProgress, parent.Progress);
            for (int i = 0; i < count; ++i)
            {
                Assert.AreEqual(progresses[i], parent.Children[i].Progress);
            }

            repository.VerifyAllExpectations();
        }

        [Test]
        public void CommitTest()
        {
            //Arrange
            Quest target = new FakeQuest();

            IQuestTree tree = MockRepository.GenerateStrictMock<IQuestTree>();

            RecountQuestProgressCommand wrapper = new RecountQuestProgressCommand(target, tree);

            //Act
            bool commitResult = wrapper.Commit();

            //Assert
            Assert.IsTrue(commitResult);

            tree.VerifyAllExpectations();
        }
    }
}
