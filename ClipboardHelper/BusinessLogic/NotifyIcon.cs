using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace ClipboardHelperRegEx.BusinessLogic
{
    /// <summary>
    ///     Set the notify icon as a static icon or set it as dynamic periodically changing icon.
    /// </summary>
    public sealed class NotifyIcon : IDisposable
    {
        private CancellationToken _cancellationToken;
        private CancellationTokenSource _tokenSource;
        private Action<Icon> Action { get; set; }
        private Icon StaticIcon { get; set; }
        private List<Icon> DynamicIcons { get; set; }

        public void Initiate(Icon staticIcon, List<Icon> dynamicIcons, Action<Icon> action)
        {
            Action = action;
            StaticIcon = staticIcon;
            DynamicIcons = dynamicIcons;
            SetStatic();
        }

        public void SetStatic()
        {
            try
            {
                _tokenSource?.Cancel();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {

            }
            Action(StaticIcon);
        }

        public void SetDynamic()
        {
            _tokenSource = new CancellationTokenSource();
            _cancellationToken = _tokenSource.Token;
            Task.Run(() => ChangeIconPeriodically(DynamicIcons), _cancellationToken)
                .ContinueWith(t => End(), _cancellationToken, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task ChangeIconPeriodically(IReadOnlyCollection<Icon> dynamicIcons)
        {
            while (true)
                foreach (var icon in dynamicIcons)
                {
                    Action(icon);
                    await Task.Delay(500, _cancellationToken).ConfigureAwait(true);
                }

            // ReSharper disable once FunctionNeverReturns
        }

        private void End()
        {
            Dispose(true);
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        //The CA2213 warning is suppressed since Microsoft has acknowledged that this isn't an error, but a known fault with FxCop:
        //https://stackoverflow.com/questions/36229230/ca2213-warning-when-using-null-conditional-operator-to-call-dispose/36229431
        //https://github.com/dotnet/roslyn-analyzers/issues/695
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_tokenSource")]
        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing) _tokenSource?.Dispose();
            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}