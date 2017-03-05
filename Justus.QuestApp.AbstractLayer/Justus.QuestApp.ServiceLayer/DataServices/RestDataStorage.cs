using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Justus.QuestApp.AbstractLayer.Data;
using Justus.QuestApp.AbstractLayer.Entities;
using Justus.QuestApp.AbstractLayer.Entities.Quest;
using Newtonsoft.Json;

namespace Justus.QuestApp.ServiceLayer.DataServices
{
    /// <summary>
    /// Rest consumer to service api.
    /// </summary>
    public class RestDataStorage : IDataAccessInterface<Quest>
    {
        private string _apiUrlString;
        private bool _isClosed = true;

        #region IDataAccessInterface implementation

        ///<inheritdoc/>
        public void Dispose()
        {
            Close();
        }

        ///<inheritdoc/>
        public void Open(string pathToStorage)
        {
            if (_isClosed)
            {
                if (string.IsNullOrWhiteSpace(pathToStorage))
                {
                    throw new ArgumentException(nameof(pathToStorage));
                }
                _apiUrlString = pathToStorage;
                _isClosed = false;
            }
            else
            {
                throw new InvalidOperationException("You should close already opened connection first!");
            }
        }

        ///<inheritdoc/>
        public void Close()
        {
            _isClosed = true;
        }

        ///<inheritdoc/>
        public bool IsClosed()
        {
            return _isClosed;
        }

        ///<inheritdoc/>
        public void Insert(Quest entity)
        {
            
        }

        ///<inheritdoc/>
        public void InsertAll(List<Quest> entities)
        {
            
        }

        ///<inheritdoc/>
        public void Update(Quest entity)
        {
           
        }

        ///<inheritdoc/>
        public void UpdateAll(List<Quest> entities)
        {
            
        }

        ///<inheritdoc/>
        public Quest Get(int id)
        {
            return default(Quest);
        }

        ///<inheritdoc/>
        public List<Quest> GetAll()
        {
            HttpWebRequest request = WebRequest.CreateHttp(_apiUrlString);
            request.Accept = "application/json";
            request.Method = "GET";

            using (WebResponse response = request.GetResponseAsync().Result)
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    JsonSerializer serializer = new JsonSerializer();
                    using (StreamReader streamReader = new StreamReader(responseStream))
                    {
                        using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                        {
                            List<Quest> quests = serializer.Deserialize<List<Quest>>(jsonReader);
                            return quests;
                        }
                    }

                }
            }
        }

        ///<inheritdoc/>
        public void Delete(int id)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"{_apiUrlString}/{id}");
            request.Method = "DELETE";
            request.Accept = "application/json";

            using (WebResponse response = request.GetResponseAsync().Result)
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string responseString = reader.ReadToEnd();
                    }
                }
            }
        }

        ///<inheritdoc/>
        public void DeleteAll()
        {
            
        } 

        #endregion
    }
}
