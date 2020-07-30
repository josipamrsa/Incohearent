using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Incohearent.Models
{
    // Timer za sesiju
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

        // Pokreni timer
        public void Start()
        {
            CancellationTokenSource cts = this.cancel;
            Device.StartTimer(this.timespan, () =>
            {
                // Dok nije zatražen CancellationToken timer otkucava
                // Nakon toga metoda poziva callback metodu koja će se izvršiti nakon završetka timera
                if (cts.IsCancellationRequested) return false; 
                this.callback.Invoke();
                return false;
            });
        }

        // Zaustavi timer
        public void Stop()
        {
            // Kada dvije niti pokušaju ažurirati iste varijable ili kad se izvode istovremeno
            // može doći do raznih grešaka i neispravnih ažuriranja podataka. Stoga se Interlocked
            // koristi za manipulaciju varijablama koje su dostupne više niti bez da dođe do
            // Exceptiona - https://www.c-sharpcorner.com/UploadFile/1d42da/interlocked-class-in-C-Sharp-threading/

            // Atomično mijenja vrijednosti određenih varijabli
            Interlocked.Exchange(ref this.cancel, new CancellationTokenSource()).Cancel();
        }
    }


}
