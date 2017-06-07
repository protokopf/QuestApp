using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Model;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Justus.QuestApp.ModelLayer.UnitTests.ModelTest
{
    [TestFixture]
    class AllUpperQuestProgressRecounterTest
    {
        [Test]
        public void QuestNullTest()
        {
            //Arrange
            AllUpperQuestProgressRecounter recounter = new AllUpperQuestProgressRecounter();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => recounter.RecountProgress(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [TestCase(QuestState.Progress)]
        [TestCase(QuestState.Done)]
        [TestCase(QuestState.Failed)]
        [TestCase(QuestState.Idle)]
        public void QuestWithoutParentTest(QuestState state)
        {
            //Arrange
            AllUpperQuestProgressRecounter recounter = new AllUpperQuestProgressRecounter();
            Quest q = QuestHelper.CreateQuest(state);
            q.Parent = null;

            //Act
            recounter.RecountProgress(q);

            //Assert
            Assert.AreEqual(state == QuestState.Done ? 1 : 0, q.Progress);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void QuestWithOneParentWithoutSiblingsTest(bool isQuestDone)
        {
            //Arrange
            double progress = isQuestDone ? 1 : 0;

            AllUpperQuestProgressRecounter recounter = new AllUpperQuestProgressRecounter();
            Quest parent = QuestHelper.CreateQuest();
            parent.Children = new List<Quest>()
            {
                QuestHelper.CreateQuest()
            };

            Quest onlyChild = parent.Children[0];
            onlyChild.Progress = progress;

            //Act
            recounter.RecountProgress(parent);

            //Assert
            Assert.AreEqual(progress, parent.Progress);
        }

        [TestCase(new double[] { 0, 0, 0})]
        [TestCase(new double[] { 0, 0, 1 })]
        [TestCase(new double[] { 0, 1, 1 })]
        [TestCase(new double[] { 1, 1, 1 })]
        public void QuestWithParentAndSiblingsTest(double[] progresses)
        {
            //Arrange
            AllUpperQuestProgressRecounter recounter = new AllUpperQuestProgressRecounter();

            Quest parent = QuestHelper.CreateQuest();
            parent.Children = new List<Quest>();

            int count = progresses.Length;

            for (int i = 0; i < count; ++i)
            {
                parent.Children.Add(new Quest() {Progress = progresses[i]});
            }


            //Act
            recounter.RecountProgress(parent);

            //Assert
            double expectedProgress = progresses.Average(d => d);
            Assert.AreEqual(expectedProgress, parent.Progress);
        }
    }
}
