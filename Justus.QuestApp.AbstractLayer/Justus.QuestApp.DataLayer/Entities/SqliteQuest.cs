using Justus.QuestApp.AbstractLayer.Entities.Quest;
using System;
using System.Collections.Generic;
using SQLite;
using SQLite.Net.Attributes;

namespace Justus.QuestApp.DataLayer.Entities
{
    [Table("Quests")]
    public class SqliteQuest : Quest
    {
        #region Quest overriding

        ///<inheritdoc/>
        [PrimaryKey]
        [AutoIncrement]
        public override int Id { get; set; }

        ///<inheritdoc/>
        [Ignore]
        public override Quest Parent { get; set; }

        ///<inheritdoc/>
        [Ignore]
        public sealed override List<Quest> Children { get; set; } 

        #endregion
    }
}
