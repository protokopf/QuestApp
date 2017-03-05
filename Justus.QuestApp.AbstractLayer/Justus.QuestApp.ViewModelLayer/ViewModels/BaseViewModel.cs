using System;

namespace Justus.QuestApp.ViewModelLayer.ViewModels
{
    /// <summary>
    /// Base type for all view models.
    /// </summary>
    public class BaseViewModel
    {
        private bool _isBusy = false;

        /// <summary>
        /// Event, which notifies, whether view model busy or not.
        /// </summary>
        public event EventHandler IsBusyChanged = delegate {};

        /// <summary>
        /// Indicates, whether view model busy or not.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                IsBusyChanged.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
