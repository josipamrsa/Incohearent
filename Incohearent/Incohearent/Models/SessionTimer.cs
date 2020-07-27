using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Incohearent.Models
{
    public class SessionTimer
    {
        private readonly TimeSpan timespan;
        private readonly Action callback;
        private CancellationTokenSource cancel;

        public SessionTimer(TimeSpan ts, Action cb)
        {
            this.timespan = ts;
            this.callback = cb;
            this.cancel = new CancellationTokenSource();
        }

        public void Start()
        {
            CancellationTokenSource cts = this.cancel;
            Device.StartTimer(this.timespan, () =>
            {
                if (cts.IsCancellationRequested) return false;
                this.callback.Invoke();
                return false;
            });
        }

        public void Stop()
        {
            Interlocked.Exchange(ref this.cancel, new CancellationTokenSource()).Cancel();
        }
    }


}
