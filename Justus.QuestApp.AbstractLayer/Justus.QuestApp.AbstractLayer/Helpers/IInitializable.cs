using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.AbstractLayer.Helpers
{
    /// <summary>
    /// Interface for types, that can be initialize.
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Initialize implementation.
        /// </summary>
        void Initialize();
    }
}
