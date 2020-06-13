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
    public partial class LobbyPage : ContentPage
    {
        public LobbyPage()
        {
            InitializeComponent();
        }

        private void ConnectIntoSession()
        {
            /*
            
            Pritiskom dugmeta će spojiti igrače u sesiju.
            
            */

            SetSessionDetails();
            GeneratePhrases();
            StartSession();
            
        }

        private void SetSessionDetails()
        {
            /*

           Podiže zahtjev za korisnika kojim korisnik određuje trajanje rundi, broj rundi, ime...

           */
        }

        private void GeneratePhrases()
        {
            /*
             
            Šalje zahtjev aplikaciji koja generira skup frazi koji će se koristiti za igru.
            
            */
        }

        private void StartSession()
        {
            /*

           Nakon generiranja frazi, šalje zahtjev aplikaciji dijeljenja frazi s detaljima, te započinje sesiju

           */
        }


    }
}