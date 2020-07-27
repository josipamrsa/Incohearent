using Incohearent.Models;
using Incohearent.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Incohearent.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EndcardPage : ContentPage
    {
        public EndcardDisplayViewModel ViewModel
        {
            get { return BindingContext as EndcardDisplayViewModel; }
            set { BindingContext = value; }
        }
        public static Points Points { get; private set; }
        public static User User { get; private set; }
        public EndcardPage(Points points, User user)
        {
            InitializeComponent();
            Points = points;
            User = user;
            ViewModel = new EndcardDisplayViewModel(Points);           
        }

        protected override void OnAppearing()
        {
            if (Points.IsGameMaster) {
                LBLPlayerName.Text = Constants.PlayerName + Points.Username;
                LBLPlayerPoints.Text = Constants.GameMasterInfo;
            }

            else
            {                
                LBLPlayerName.Text = Constants.PlayerName + Points.Username;
                LBLPlayerPoints.Text = Constants.PlayerPoints + Points.PointsWon;
            }
            
            base.OnAppearing();           
        }
    }
}