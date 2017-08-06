using Justus.QuestApp.ViewModelLayer.ViewModels.QuestDetails.Abstract;

namespace Justus.QuestApp.ViewModelLayer.Factories
{
    /// <summary>
    /// Interface for factories, that are responsible for creation IQuestViewModel implementations.
    /// </summary>
    public interface IQuestViewModelFactory
    {
        /// <summary>
        /// Returns IQuestViewModel implementation.
        /// </summary>
        /// <returns></returns>
        IQuestViewModel CreateQuestViewModel();
    }
}
