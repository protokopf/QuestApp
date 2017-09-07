using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Model.Order;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.ModelTest.Order
{
    [TestFixture]
    class TimeLeftOrderStrategyTest
    {
        [Test]
        public void OrderQuestsNullTest()
        {
            //Arrange
            IEnumerable<Quest> quests = null;

            TimeLeftOrderStrategy strategy = new TimeLeftOrderStrategy();

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => strategy.Order(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quests", ex.ParamName);
        }

        [Test]
        public void OrderQuestsDefaulsAscendingTest()
        {
            //Arrange
            DateTime current = DateTime.Now;

            Quest q1 = new FakeQuest() {Deadline = current + new TimeSpan(1, 0, 0)};
            Quest q2 = new FakeQuest() { Deadline = current + new TimeSpan(3, 10, 0) };

            List<Quest> quests = new List<Quest> {q2, q1};

            TimeLeftOrderStrategy strategy = new TimeLeftOrderStrategy();

            //Act
            IList<Quest> orderedList = strategy.Order(quests).ToList();

            //Assert
            Assert.IsNotNull(orderedList);
            Assert.AreEqual(2, orderedList.Count);
            Assert.AreEqual(q1, orderedList[0]);
            Assert.AreEqual(q2, orderedList[1]);
        }

        [Test]
        public void OrderQuestsDescendingTest()
        {
            //Arrange
            DateTime current = DateTime.Now;

            Quest q1 = new FakeQuest() { Deadline = current + new TimeSpan(1, 0, 0) };
            Quest q2 = new FakeQuest() { Deadline = current + new TimeSpan(3, 10, 0) };

            List<Quest> quests = new List<Quest> { q2, q1 };

            TimeLeftOrderStrategy strategy = new TimeLeftOrderStrategy()
            {
                Descending = true
            };

            //Act
            IList<Quest> orderedList = strategy.Order(quests).ToList();

            //Assert
            Assert.IsNotNull(orderedList);
            Assert.AreEqual(2, orderedList.Count);
            Assert.AreEqual(q2, orderedList[0]);
            Assert.AreEqual(q1, orderedList[1]);
        }
    }
}
