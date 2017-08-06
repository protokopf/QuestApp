using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.DataLayer.Entities;
using Justus.QuestApp.ModelLayer.Factories;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.FactoriesTest
{
    [TestFixture]
    class SqliteQuestCreatorTest
    {
        [Test]
        public void CreateTest()
        {
            //Arrange
            IQuestFactory factory = new SqliteQuestFactory();

            //Act
            Quest item = factory.CreateQuest();

            //Assert
            Assert.IsInstanceOf<SqliteQuest>(item);
            Assert.IsNotNull(item);
            Assert.IsNull(item.Children);
            Assert.AreEqual(State.Idle, item.State);
            Assert.AreEqual(String.Empty, item.Title);
            Assert.AreEqual(String.Empty, item.Description);
            Assert.AreEqual(null, item.Deadline);
            Assert.AreEqual(null, item.Deadline);
            Assert.IsFalse(item.IsImportant);
            Assert.IsNull(item.ParentId);
            Assert.IsNull(item.Parent);
        }
    }
}
