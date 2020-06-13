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
    public partial class SessionPage : ContentPage
    {
        public SessionPage()
        {
            InitializeComponent();
        }
        
        private void PhraseAssignment()
        {
            /*
            
            Nasumično bira fraze i dodjeljuje ih idućem igraču (nakon isteka vremena ili događaja gdje je igrač odgovorio točno
            prije isteka vremena).

            */
        }

        private void KeepScore()
        {
            /*
            
            Pratit će statistiku za svakog igrača.
            
            */
        }

        private void EndGame()
        {
            /*
            
            Triggerat će se po završetku igre i slati statistiku na Endcard ekran.
             
            */
        }
    }
}