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
using Justus.QuestApp.AbstractLayer.Commands;

namespace Justus.QuestApp.View.Droid.StubServices
{
    public class StubCommandManager : ICommandManager
    {
        public void Add(Command command)
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public void Do()
        {
            throw new NotImplementedException();
        }
    }
}