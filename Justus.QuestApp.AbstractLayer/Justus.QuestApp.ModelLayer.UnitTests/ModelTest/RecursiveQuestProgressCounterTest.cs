using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Model;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.ModelTest
{
    [TestFixture]
    class RecursiveQuestProgressCounterTest
    {
        [Test]
        public void NullQuestCountExpectedTest()
        {
            //Arrange
            IQuestProgressCounter counter = new RecursiveQuestProgressCounter();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => counter.CountProgress(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void QuestProgressWithoutChildrenTest()
        {
            //Arrange
            IQuestProgressCounter counter = new RecursiveQuestProgressCounter();
            Quest testQuest = QuestHelper.CreateQuest(QuestState.Progress);

            int expectedTotal = 1;
            int expectedCurrent = 0;

            //Act
            ProgressValue result = counter.CountProgress(testQuest);

            //Assert
            Assert.AreEqual(expectedTotal, result.Total);
            Assert.AreEqual(expectedCurrent, result.Current);
        }

        [Test]
        public void QuestDoneWithoutChildrenTest()
        {
            //Arrange
            IQuestProgressCounter counter = new RecursiveQuestProgressCounter();
            Quest testQuest = QuestHelper.CreateQuest(QuestState.Done);

            int expectedTotal = 1;
            int expectedCurrent = 1;

            //Act
            ProgressValue result = counter.CountProgress(testQuest);

            //Assert
            Assert.AreEqual(expectedTotal, result.Total);
            Assert.AreEqual(expectedCurrent, result.Current);
        }

        [Test]
        public void QuestWithOneLevelChildrenTest()
        {
            //Arrange
            IQuestProgressCounter counter = new RecursiveQuestProgressCounter();
            Quest testQuest = QuestHelper.CreateQuest(QuestState.Progress);
            testQuest.Children.Add(QuestHelper.CreateQuest(QuestState.Progress));
            testQuest.Children.Add(QuestHelper.CreateQuest(QuestState.Progress));
            testQuest.Children.Add(QuestHelper.CreateQuest(QuestState.Done));

            int expectedTotal = 3;
            int expectedCurrent = 1;

            //Act
            ProgressValue result = counter.CountProgress(testQuest);

            //Assert
            Assert.AreEqual(expectedTotal, result.Total);
            Assert.AreEqual(expectedCurrent, result.Current);
        }

        [Test]
        public void QuestWithTwoLevelChildrenTest()
        {
            //Arrange
            IQuestProgressCounter counter = new RecursiveQuestProgressCounter();
            Quest testQuest = QuestHelper.CreateQuest(QuestState.Progress);
            Quest child = QuestHelper.CreateQuest(QuestState.Progress);

            child.Children.Add(QuestHelper.CreateQuest(QuestState.Progress));
            child.Children.Add(QuestHelper.CreateQuest(QuestState.Progress));

            testQuest.Children.Add(child);
            testQuest.Children.Add(QuestHelper.CreateQuest(QuestState.Done));
            testQuest.Children.Add(QuestHelper.CreateQuest(QuestState.Done));

            int expectedTotal = 6;
            int expectedCurrent = 4;

            //Act
            ProgressValue result = counter.CountProgress(testQuest);

            //Assert
            Assert.AreEqual(expectedTotal, result.Total);
            Assert.AreEqual(expectedCurrent, result.Current);
        }
    }
}
