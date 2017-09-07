using Justus.QuestApp.AbstractLayer.Entities.Quest;
using System;
using System.Collections.Generic;

namespace Justus.QuestApp.ViewModelLayer.UnitTests.Stubs
{
    internal class FakeQuest : Quest
    {
        public override int Id { get; set; }
        public override int? ParentId { get; set; }
        public override string Title { get; set; }
        public override string Description { get; set; }
        public override Quest Parent { get; set; }
        public override List<Quest> Children { get; set; }
    }
}
