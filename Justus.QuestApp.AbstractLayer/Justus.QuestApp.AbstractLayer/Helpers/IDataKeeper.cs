using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justus.QuestApp.AbstractLayer.Helpers
{
    /// <summary>
    /// Interface for types, which can keep data.
    /// </summary>
    public interface IDataKeeper
    {
        /// <summary>
        /// Keeps data for specified key.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns>True, if data was keeped (key was not used before), otherwise false.</returns>
        bool Keep<TData>(string key, TData data);

        /// <summary>
        /// Returns keeped data by key.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="key"></param>
        /// <returns>Actual data, if it was keeped by key, otherwise default value fordata type.</returns>
        TData Get<TData>(string key);

        /// <summary>
        /// Deletes data by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True, if data was deleted, otherwise - false.</returns>
        bool Delete(string key);
    }
}
