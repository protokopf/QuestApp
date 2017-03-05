using System.Threading;
using System.Threading.Tasks;

namespace Justus.QuestApp.AbstractLayer.Services
{
    /// <summary>
    /// Represents service, which work in interval way.
    /// </summary>
    public abstract class IntervalAbstractService : IService
    {
        /// <summary>
        /// Cancellation token for stop service.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Points, whether service has been started or not.
        /// </summary>
        private bool _isStarted;

        /// <summary>
        /// Interval in milliseconds.
        /// </summary>
        protected int IntervalMilliseconds;

        /// <summary>
        /// Receives interval milliseconds.
        /// </summary>
        /// <param name="intervalMilliseconds"></param>
        protected IntervalAbstractService(int intervalMilliseconds)
        {
            IntervalMilliseconds = intervalMilliseconds;
            _isStarted = false;
        }

        #region IService implementation

        ///<inheritdoc/>
        public void Start()
        {
            if (!_isStarted)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                HandleDelay(_cancellationTokenSource.Token);
            }                       
        }

        ///<inheritdoc/>
        public void Stop()
        {
            if (_isStarted)
            {
                _cancellationTokenSource.Cancel();
                _isStarted = false;
            }
        }
         
        #endregion

        /// <summary>
        /// Operation, which represents task, that will be executed each inteval.
        /// </summary>
        /// <returns></returns>
        protected abstract Task IntervalTaskOperation();

        private async void HandleDelay(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await IntervalTaskOperation();
                await Task.Delay(IntervalMilliseconds);
            }
        }
    }
}
