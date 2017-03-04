using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Justus.QuestApp.AbstractLayer.Data;

namespace Justus.QuestApp.View.Droid.StubServices
{
    class StubWebStringProvider : IConnectionStringProvider
    {
        public string GetConnectionString()
        {
            return "http://localhost:37993/api/Quests";
        }
    }
}