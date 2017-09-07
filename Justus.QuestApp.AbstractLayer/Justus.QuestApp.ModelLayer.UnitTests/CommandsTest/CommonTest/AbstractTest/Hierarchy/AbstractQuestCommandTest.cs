using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Classic.Common.Abstracts;
using Justus.QuestApp.ModelLayer.Commands.Classic.Common.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.AbstractTest.Hierarchy
{
    [TestFixture]
    class AbstractQuestCommandTest
    {
        private class MockAbstractQuestCommand : AbstractQuestCommand
        {
            public MockAbstractQuestCommand(Quest quest) : base(quest)
            {
            }

            protected override bool InnerExecute()
            {
                throw new NotImplementedException();
            }

            protected override bool InnerUndo()
            {
                throw new NotImplementedException();
            }

            protected override bool InnerCommit()
            {
                throw new NotImplementedException();
            }

            protected override void ExecuteOnQuest(Quest quest)
            {
                throw new NotImplementedException();
            }

            protected override void UndoOnQuest(Quest quest)
            {
                throw new NotImplementedException();
            }

            public Quest GetQuestRef => QuestRef;
        }

        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(AbstractQuestCommand).IsSubclassOf(typeof(SwitchCommand)));
        }

        [Test]
        public void CtorNullTest()
        {
            //Arrange

            //Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => new MockAbstractQuestCommand(null));

            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual("quest", ex.ParamName);
        }

        [Test]
        public void CtorTest()
        {
            //Arrange
            Quest quest = new FakeQuest();
            MockAbstractQuestCommand command = null;

            //Act
            Assert.DoesNotThrow(() => command = new MockAbstractQuestCommand(quest));

            //Assert
            Assert.AreEqual(quest, command.GetQuestRef);
        }
    }
}
