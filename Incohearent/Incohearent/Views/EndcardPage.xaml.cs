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
        // Da se vrijednosti automatski dohvaćaju / mijenjaju te da se mogu koristiti metode iz VM-a
        public EndcardDisplayViewModel ViewModel
        {
            get { return BindingContext as EndcardDisplayViewModel; }
            set { BindingContext = value; }
        }
        public static Points Points { get; private set; }   // Broj bodova igrača
        public static User User { get; private set; }       // Korisnik
        public EndcardPage(Points points, User user)
        {
            InitializeComponent();
            Points = points;
            User = user;
            ViewModel = new EndcardDisplayViewModel(Points);          // Inicijaliziraj ViewModel
        }

        protected override void OnAppearing()
        {
            // Ako je igrač bio sudac onda se samo prikazuje njegovo ime i oznaka 
            if (Points.IsGameMaster) {
                LBLPlayerName.Text = Constants.PlayerName + Points.Username;
                LBLPlayerPoints.Text = Constants.GameMasterInfo;
            }

            // U protivnom se izlistava ime igrača i broj bodova
            else
            {                
                LBLPlayerName.Text = Constants.PlayerName + Points.Username;
                LBLPlayerPoints.Text = Constants.PlayerPoints + Points.PointsWon;
            }
            
            base.OnAppearing();           
        }
    }
}