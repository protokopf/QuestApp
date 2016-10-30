using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.AbstractLayer.Entities
{
    /// <summary>
    /// Abstract type for all identified entities.
    /// </summary>
    public abstract class IdentifiedEntity
    {
        /// <summary>
        /// Entity identifier.
        /// </summary>
        public int Id { get; set; }
    }
}
