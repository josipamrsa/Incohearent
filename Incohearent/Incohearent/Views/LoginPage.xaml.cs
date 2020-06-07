using Incohearent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Incohearent.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void SignIntoLobby(object sender, EventArgs e)
        {
            User u = new User(EnUser.Text, "127.0.0.1"); // test
            if (u.CheckInformation())  { DisplayAlert("Information", "Success", "OK"); }
            else
                DisplayAlert("Information", "Login Failed", "OK");
        }
    }
}