using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.ModelLayer.Commands.Classic.Common.Abstracts.Hierarchy;
using Justus.QuestApp.ModelLayer.UnitTests.Stubs;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Justus.QuestApp.ModelLayer.UnitTests.CommandsTest.AbstractTest.Hierarchy
{
    [TestFixture]
    class ThisQuestCommandQuestTest
    {
        class MockThisQuestCommand : ThisQuestCommand
        {
            public MockThisQuestCommand(Quest quest) : base(quest)
            {
            }

            protected override bool InnerCommit()
            {
                throw new NotImplementedException();
            }

            protected override void ExecuteOnQuest(Quest quest)
            {
                OnExecuteOnQuest?.Invoke(quest);
            }

            protected override void UndoOnQuest(Quest quest)
            {
                OnUndoOnQuest?.Invoke(quest);
            }

            public event Action<Quest> OnExecuteOnQuest;
            public event Action<Quest> OnUndoOnQuest;
        }

        [Test]
        public void BaseClassTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(typeof(ThisQuestCommand).IsSubclassOf(typeof(AbstractQuestCommand)));
        }

        [Test]
        public void ExecuteUndoTest()
        {
            //Arrange
            Quest quest = new FakeQuest();

            MockThisQuestCommand command = new MockThisQuestCommand(quest);
            command.OnExecuteOnQuest += (q) => { Assert.AreEqual(quest, q); };
            command.OnUndoOnQuest += (q) => { Assert.AreEqual(quest, q); };

            //Act
            command.Execute();
            command.Undo();

            //Assert
        }
    }
}
