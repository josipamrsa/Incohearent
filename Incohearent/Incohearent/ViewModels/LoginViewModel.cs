using Incohearent.Data;
using Incohearent.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Incohearent.ViewModels
{
    // Upravljanje Loginom
    public class LoginViewModel : IncohearentBaseViewModel
    {
        private UserViewModel userVm;           // ViewModel svojstvo za poziv iz Viewa
        private IUserStore userStore;           // Metode za rad s tablicom User modela u bazi podataka 
        private IPageService pageService;       // Metode za rad sa obavijestima (neiskorišteno)

        public User User { get; private set; }  // Korisnik
       
        public UserViewModel UserVm
        {
            get => userVm;
            set
            {
                SetValue(ref userVm, value);
            }
        }

        public ICommand SignInUserCommand { get; private set; } // Naredba za login u aplikaciju

        public LoginViewModel(IUserStore us, IPageService ps)
        { 
            userStore = us;
            pageService = ps;            
            SignInUserCommand = new Command(async () => await SignInUser());
            User = new User();
        }

        private async Task SignInUser()
        {
            // Dohvati metode za rad s mrežom
            var networkConnection = DependencyService.Get<INetworkConnection>(); 

            // Provjera je li polje korisničkog imena prazno
            if (String.IsNullOrWhiteSpace(User.Username))
            {
                MessagingCenter.Send(this, "loginFail", "OK");
                return;
            }

            // Provjera je li korisnik povezan u mrežu
            if (!networkConnection.IsConnected)
            {
                MessagingCenter.Send(this, "networkFailure", "OK");
                return;
            }

            // Javna adresa korisnika
            User.PublicAddress = App.RestApi.GetPublicIpAddress();

            // Pokušaj dohvatiti preko WiFi-ja, a ako nije na WiFi-ju (mobilna mreža) onda dohvati preko DNS-a
            if (!string.IsNullOrEmpty(networkConnection.GetIpAddressDevice()))
                User.PrivateAddress = networkConnection.GetIpAddressDevice();
            else if (!string.IsNullOrEmpty(networkConnection.GetIPAddressCellularNetwork()))
                User.PrivateAddress = networkConnection.GetIPAddressCellularNetwork();
            else
            {
                MessagingCenter.Send(this, "ipNotFound", "OK");
                return;
            }

            // Spremi zapis u bazu podataka
            User.LoggedIn = true;
            if (User.UserId == 0)
                await userStore.AddUser(User);

            // Obavijesti korisnika za spajanje na WiFi
            if (!networkConnection.UserIsOnWifi()) MessagingCenter.Send(this, "notOnWifi", "OK");
            MessagingCenter.Send(this, "loggedIn", User);
        }
       
    }
}
