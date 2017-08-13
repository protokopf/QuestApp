using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.ModelLayer.Commands.State;
using Justus.QuestApp.ModelLayer.Commands.State.Common;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.QuestStateTest
{
    [TestFixture]
    class StartQuestCommandTest
    {
        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(StartQuestCommand).IsSubclassOf(typeof(ChangeStateUpHierarchy)));
        }
    }
}
