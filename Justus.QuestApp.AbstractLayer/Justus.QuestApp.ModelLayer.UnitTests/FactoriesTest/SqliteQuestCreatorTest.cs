using System;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Factories;
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
            IQuestCreator creator = new SqliteQuestCreator();

            //Act
            Quest item = creator.Create();

            //Assert
            Assert.IsNotNull(item);
            Assert.IsNotNull(item.Children);
            Assert.IsEmpty(item.Children);
            Assert.AreEqual(QuestState.Idle, item.CurrentState);
            Assert.AreEqual(String.Empty, item.Title);
            Assert.AreEqual(String.Empty, item.Description);
            Assert.AreEqual(DateTime.MinValue, item.Deadline);
            Assert.AreEqual(DateTime.MinValue, item.Deadline);
            Assert.IsFalse(item.IsImportant);
        }
    }
}
