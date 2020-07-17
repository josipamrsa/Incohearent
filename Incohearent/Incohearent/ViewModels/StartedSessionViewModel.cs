using Incohearent.Data;
using Incohearent.Models;
using Microsoft.AspNetCore.SignalR.Client;
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
        public PhoneticPhrases Phrase { get; private set; }

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
        public ICommand ConnectSessionCommand { get; private set; }


        public StartedSessionViewModel(User user, ISessionStore ss, IPageService ps)
        {
            sessionStore = ss;
            pageService = ps;
            User = user;
            Phrase = new PhoneticPhrases();

            hubConn = new HubConnectionBuilder().WithUrl("https://incohearentwebserver.azurewebsites.net/gameHub").Build();

            ConnectSessionCommand = new Command(async () => await ConnectToSession(User));         
            FetchPhrasesCommand = new Command(async () => await FetchPhrases(User, Phrase));

            Session = new Session();

            hubConn.On<PhoneticPhrases>("PhrasesGenerated", (phrase) =>
            {
                MessagingCenter.Send(this, "phraseGenerated", phrase);
            });

            hubConn.On<PhoneticPhrases>("PhrasesNotGenerated", (phrase) =>
            {
                MessagingCenter.Send(this, "phraseNotGenerated", phrase);
            });
        }

        private async Task ConnectToSession(User user)
        {
            await hubConn.StartAsync();
            await hubConn.InvokeAsync("ConnectSession", user);
            MessagingCenter.Send(this, "userSession", "session");
        }

        private async Task FetchPhrases(User user, PhoneticPhrases phrase)
        {
            await hubConn.InvokeAsync("GeneratePhrases", user, phrase);            
        }
    }
}
