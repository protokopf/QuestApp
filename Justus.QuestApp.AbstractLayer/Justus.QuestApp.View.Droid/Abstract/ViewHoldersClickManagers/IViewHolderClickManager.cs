using Justus.QuestApp.View.Droid.Abstract.ViewHolders;

namespace Justus.QuestApp.View.Droid.Abstract.ViewHoldersClickManagers
{
    public interface IViewHolderClickManager<in TViewHolder> where TViewHolder : PositionedViewHolder
    {
        void BindClickListeners(TViewHolder holder);

        void UnbindClickListeners(TViewHolder holder);
    }
}