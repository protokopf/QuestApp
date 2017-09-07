using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Justus.QuestApp.AbstractLayer.Helpers.Extentions;
using Justus.QuestApp.AbstractLayer.Model;

namespace Justus.QuestApp.ModelLayer.Model.Order
{
    /// <summary>
    /// Orders quests by left time.
    /// </summary>
    public class TimeLeftOrderStrategy : IQuestOrderStrategy
    {
        private DateTime _orderTime;

        #region IQuestOrderStrategy implementation

        ///<inheritdoc cref="IQuestOrderStrategy"/>
        public IOrderedEnumerable<Quest> Order(IEnumerable<Quest> quests)
        {
            quests.ThrowIfNull(nameof(quests));
            _orderTime = DateTime.Now;
            return Descending ? quests.OrderByDescending(InnerOrder) : quests.OrderBy(InnerOrder);
        }

        ///<inheritdoc cref="IQuestOrderStrategy"/>
        public bool Descending { get; set; }

        #endregion

        private TimeSpan InnerOrder(Quest toOrder)
        {
            return toOrder.Deadline != null ? toOrder.Deadline.Value - _orderTime : TimeSpan.Zero;
        }
    }
}
