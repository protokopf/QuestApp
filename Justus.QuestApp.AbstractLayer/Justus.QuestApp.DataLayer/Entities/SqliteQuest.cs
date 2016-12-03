using Justus.QuestApp.AbstractLayer.Entities.Quest;
using System;
using System.Collections.Generic;
using SQLite;

namespace Justus.QuestApp.DataLayer.Entities
{
    [Table("Quests")]
    public class SqliteQuest : Quest
    {
        #region Quest overriding

        ///<inheritdoc/>
        [PrimaryKey]
        public override int Id { get; set; }

        ///<inheritdoc/>
        public override int ParentId { get; set; }

        ///<inheritdoc/>
        public override string Title { get; set; }

        ///<inheritdoc/>
        public override string Description { get; set; }

        ///<inheritdoc/>
        public override DateTime Deadline { get; set; }

        ///<inheritdoc/>
        [Ignore]
        public override Quest Parent { get; set; }

        ///<inheritdoc/>
        [Ignore]
        public sealed override List<Quest> Children { get; set; } 

        #endregion
    }
}
