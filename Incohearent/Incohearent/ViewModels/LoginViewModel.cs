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
    public class LoginViewModel : IncohearentBaseViewModel
    {
        private UserViewModel userVm;
        private IUserStore userStore;
        private IPageService pageService;
        
        public User User { get; private set; }
       
        public UserViewModel UserVm
        {
            get => userVm;
            set
            {
                SetValue(ref userVm, value);
            }
        }

        public ICommand SignInUserCommand { get; private set; }

        public LoginViewModel(IUserStore us, IPageService ps)
        { 
            userStore = us;
            pageService = ps;            
            SignInUserCommand = new Command(async () => await SignInUser());
            User = new User();
        }

        private async Task SignInUser()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();

            if (String.IsNullOrWhiteSpace(User.Username))
            {
                MessagingCenter.Send(this, "loginFail", "OK");
                return;
            }

            User.PublicAddress = App.RestApi.GetPublicIpAddress();
            User.PrivateAddress = networkConnection.GetIpAddressDevice();
            User.LoggedIn = true;

            if (User.UserId == 0)
                await userStore.AddUser(User);

            //else            
            //    await userStore.UpdateUser(User);            

            if (!networkConnection.UserIsOnWifi()) MessagingCenter.Send(this, "notOnWifi", "OK");
            MessagingCenter.Send(this, "loggedIn", User);
        }

        private async Task SignOutUser() { }
    }
}
