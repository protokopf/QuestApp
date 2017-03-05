using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model;
using Justus.QuestApp.ModelLayer.Model;
using Justus.QuestApp.ModelLayer.UnitTests.Helpers;
using NUnit.Framework;
using System;

namespace Justus.QuestApp.ModelLayer.UnitTests.ModelTest
{
    [TestFixture]
    class QuestTimeLeftCounterTest
    {
        [Test]
        public void CountNullQuestTest()
        {
            //Arrange
            IQuestTimeLeftCounter counter = new QuestTimeLeftCounter();

            //Act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => counter.CountTimeLeft(null));

            //Assert
            Assert.IsNotNull(exception);
            Assert.AreEqual("quest", exception.ParamName);
        }

        [Test]
        public void CountNotExpiredTest()
        {
            //Arrange
            IQuestTimeLeftCounter counter = new QuestTimeLeftCounter();

            Quest quest = QuestHelper.CreateQuest();
            quest.Deadline = DateTime.Now.AddHours(1);

            //Act
            TimeSpan timeLeft = counter.CountTimeLeft(quest);

            //Assert
            Assert.IsTrue(timeLeft > TimeSpan.Zero);
        }

        [Test]
        public void CountExpiredTest()
        {
            //Arrange
            IQuestTimeLeftCounter counter = new QuestTimeLeftCounter();

            Quest quest = QuestHelper.CreateQuest();
            quest.Deadline = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1);

            //Act
            TimeSpan timeLeft = counter.CountTimeLeft(quest);

            //Assert
            Assert.IsTrue(timeLeft < TimeSpan.Zero);
        }
    }
}
