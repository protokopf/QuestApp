using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Commands;


namespace Justus.QuestApp.ModelLayer.Commands.Repository
{
    /// <summary>
    /// Command, which deletes quest from repository.
    /// </summary>
    public class DeleteQuestCommand : Command
    {
        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
