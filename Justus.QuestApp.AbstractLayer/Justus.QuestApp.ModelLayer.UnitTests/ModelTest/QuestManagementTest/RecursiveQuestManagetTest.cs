using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Model.QuestManagement;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.ModelTest.QuestManagementTest
{
    [TestFixture]
    class RecursiveQuestManagetTest
    {
        [Test]
        public void StartWithNullQuestTest()
        {
            //Arrange
            IQuestActionManager actionManager = new RecursiveQuestActionManager();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => actionManager.Start(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void StartSuccessfulWithAllParentsTest()
        {
            //Arrange
            IQuestActionManager manager = new RecursiveQuestActionManager();
            Quest quest = QuestHelper.CreateQuest(QuestState.Idle);
            quest.Parent = QuestHelper.CreateQuest(QuestState.Idle);
            quest.Parent.Parent = QuestHelper.CreateQuest(QuestState.Idle);

            //Act
            manager.Start(quest);

            //Assert
            Assert.AreEqual(QuestState.Progress, quest.CurrentState);
            Quest parent = quest.Parent;
            while (parent != null)
            {
                Assert.AreEqual(QuestState.Progress, parent.CurrentState);
                parent = parent.Parent;
            }
        }

        [Test]
        public void IdleWithNullQuestTest()
        {
            //Arrange
            IQuestActionManager actionManager = new RecursiveQuestActionManager();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => actionManager.Idle(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void IdleSuccessfulWithAllChildrenTest()
        {
            //Arrange
            IQuestActionManager manager = new RecursiveQuestActionManager();
            Quest quest = QuestHelper.CreateCompositeQuest(2, 2, QuestState.Progress);

            //Act
            manager.Idle(quest);

            //Assert
            Assert.AreEqual(QuestState.Idle, quest.CurrentState);
            Assert.IsTrue(QuestHelper.CheckThatAllQuestsHierarchyMatchPredicate(quest.Children, q => q.CurrentState == QuestState.Idle));
        }

        [Test]
        public void FailWithNullQuestTest()
        {
            //Arrange
            IQuestActionManager actionManager = new RecursiveQuestActionManager();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => actionManager.Fail(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void FailSuccessfulTest()
        {
            //Arrange
            IQuestActionManager manager = new RecursiveQuestActionManager();
            Quest quest = QuestHelper.CreateQuest(QuestState.Progress);

            //Act
            manager.Fail(quest);

            //Assert
            Assert.AreEqual(QuestState.Failed, quest.CurrentState);
        }

        [Test]
        public void FinishWithNullQuestTest()
        {
            //Arrange
            IQuestActionManager actionManager = new RecursiveQuestActionManager();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => actionManager.Finish(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void FinishSuccessfulTest()
        {
            //Arrange
            IQuestActionManager manager = new RecursiveQuestActionManager();
            Quest quest = QuestHelper.CreateQuest(QuestState.Progress);

            //Act
            manager.Finish(quest);

            //Assert
            Assert.AreEqual(QuestState.Done, quest.CurrentState);
        }

    }
}
