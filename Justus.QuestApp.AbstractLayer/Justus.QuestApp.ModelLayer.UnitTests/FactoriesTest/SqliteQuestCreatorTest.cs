﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Factories;
using Justus.QuestApp.AbstractLayer.Model;
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
            Assert.AreEqual(QuestState.Ready, item.CurrentState);
        }
    }
}