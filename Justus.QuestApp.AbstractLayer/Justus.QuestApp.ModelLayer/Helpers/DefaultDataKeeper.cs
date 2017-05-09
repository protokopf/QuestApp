using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Helpers;

namespace Justus.QuestApp.ModelLayer.Helpers
{
    /// <summary>
    /// Default implementation IDataKeeper
    /// </summary>
    public class DefaultDataKeeper : IDataKeeper
    {
        /// <summary>
        /// Internal struct for keeping 
        /// </summary>
        private struct DataTypePair
        {
            /// <summary>
            /// Actual data.
            /// </summary>
            public Object Data;

            /// <summary>
            /// Data type.
            /// </summary>
            public Type Type;
        }

        private readonly Dictionary<string, DataTypePair> _internalStorage;

        /// <summary>
        /// Default ctor;
        /// </summary>
        public DefaultDataKeeper()
        {
            _internalStorage = new Dictionary<string, DataTypePair>();
        }


        #region IDataKeeper implementation

        ///<inheritdoc/>
        public bool Keep<TData>(string key, TData data)
        {
            ValidateKey(key);

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (!_internalStorage.ContainsKey(key))
            {
                DataTypePair pair = new DataTypePair()
                {
                    Data = data,
                    Type = typeof(TData)
                };
                _internalStorage.Add(key, pair);

                return true;
            }
            return false;
        }

        ///<inheritdoc/>
        public TData Get<TData>(string key)
        {
            ValidateKey(key);

            DataTypePair pair;
            TData data = default(TData);
            if (_internalStorage.TryGetValue(key, out pair))
            {
                if (pair.Type == typeof(TData))
                {
                    data = (TData) pair.Data;
                }
            }
            return data;
        }

        ///<inheritdoc/>
        public bool Delete(string key)
        {
            ValidateKey(key);
            return _internalStorage.Remove(key);
        }

        #endregion

        #region Private methods

        private void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Key should be valid non-empty string.");
            }
        }

        #endregion
    }
}
