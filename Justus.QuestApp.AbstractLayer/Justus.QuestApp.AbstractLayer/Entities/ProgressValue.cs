using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.AbstractLayer.Entities
{
    /// <summary>
    /// Struct, which represents progress result.
    /// </summary>
    public struct ProgressValue
    {
        /// <summary>
        /// Expected value,
        /// </summary>
        public int Total;

        /// <summary>
        /// Actual value.
        /// </summary>
        public int Current;

        /// <summary>
        /// Initialize total and current values.
        /// </summary>
        /// <param name="total"></param>
        /// <param name="current"></param>
        public ProgressValue(int total, int current)
        {
            Total = total;
            Current = current;
        }
    }
}
