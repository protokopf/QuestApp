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
using Justus.QuestApp.AbstractLayer.Entities.Quest;

namespace Justus.QuestApp.View.Droid.ViewHoldersFillers
{
    /// <summary>
    /// Fills view holder with information.
    /// </summary>
    public interface IViewHolderFiller<in TViewHolder>
    {
        /// <summary>
        /// Fills view holder with quest data.
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="questData"></param>
        void Fill(TViewHolder holder, Quest questData);
    }
}