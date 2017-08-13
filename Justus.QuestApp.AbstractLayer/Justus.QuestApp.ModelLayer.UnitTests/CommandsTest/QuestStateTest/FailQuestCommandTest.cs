using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.ModelLayer.Commands.State;
using Justus.QuestApp.ModelLayer.Commands.State.Common;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestStateTest
{
    [TestFixture]
    class FailQuestCommandTest
    {
        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(FailQuestCommand).IsSubclassOf(typeof(ChangeStateUpHierarchyIfChildrenHaveTheSameState)));
        }
    }
}
