using Android.Support.Design.Widget;

namespace Justus.QuestApp.View.Droid.Abstract.Fragments
{
    /// <summary>
    /// Represents interface for types, which manages FAB.
    /// </summary>
    public interface IFabDecorator
    {
        /// <summary>
        /// Manages given FAB.
        /// </summary>
        /// <param name="fab"></param>
        void Decorate(FloatingActionButton fab);
    }

}