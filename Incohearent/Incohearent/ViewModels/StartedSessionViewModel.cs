using Incohearent.Data;
using Incohearent.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Incohearent.ViewModels
{
    public class StartedSessionViewModel : IncohearentBaseViewModel
    {
        private SessionViewModel sessionVm;
        private ISessionStore sessionStore;
        private IPageService pageService;

        public User User { get; private set; }
        public Session Session { get; private set; }
        public string UserConnectionId { get; private set; }

        public SessionViewModel SessionVm
        {
            get => sessionVm;
            set
            {
                SetValue(ref sessionVm, value);
            }
        }

        private HubConnection hubConn;
        public ICommand FetchPhrasesCommand { get; private set; }


        public StartedSessionViewModel(string id, ISessionStore ss, IPageService ps)
        {
            UserConnectionId = id;
            sessionStore = ss;
            pageService = ps;

            FetchPhrasesCommand = new Command(async () => await FetchPhrases());
            Session = new Session();

        }

        private async Task FetchPhrases()
        {
            
        }
    }
}
