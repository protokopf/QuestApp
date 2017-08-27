using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;

namespace Justus.QuestApp.ModelLayer.Commands.Abstracts.Hierarchy
{
    /// <summary>
    /// Operates only on current quest.
    /// </summary>
    public abstract class ThisQuestCommand : AbstractQuestCommand
    {
        /// <summary>
        /// Receives quest to operate on
        /// </summary>
        /// <param name="quest"></param>
        protected ThisQuestCommand(Quest quest) : base(quest)
        {
        }

        #region AbstractQuestCommand overriding

        ///<inehritdoc cref="AbstractQuestCommand"/>
        public override bool Execute()
        {
            ExecuteOnQuest(QuestRef);
            return true;
        }

        ///<inehritdoc cref="AbstractQuestCommand"/>
        public override bool Undo()
        {
            UndoOnQuest(QuestRef);
            return true;
        }

        #endregion
    }
}
