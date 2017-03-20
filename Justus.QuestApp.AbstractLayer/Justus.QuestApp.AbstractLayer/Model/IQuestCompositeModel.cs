using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Model.Composite;

namespace Justus.QuestApp.AbstractLayer.Model
{
    /// <summary>
    /// Quest composite model.
    /// </summary>
    public interface IQuestCompositeModel : ICompositeModel<Quest>
    {
    }
}
