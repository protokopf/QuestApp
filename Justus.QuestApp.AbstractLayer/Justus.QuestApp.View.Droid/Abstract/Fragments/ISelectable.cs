namespace Justus.QuestApp.View.Droid.Abstract.Fragments
{
    /// <summary>
    /// Interface for fragments, which repronses on fact, that their were selected.
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Fires, when implementation is selected.
        /// </summary>
        void OnSelect();

        /// <summary>
        /// Fires, when implementation is unselected.
        /// </summary>
        void OnUnselect();
    }
}