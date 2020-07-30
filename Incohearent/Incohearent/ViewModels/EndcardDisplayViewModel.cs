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
    public class EndcardDisplayViewModel : IncohearentBaseViewModel
    {
        public Points Points { get; private set; }              // Bodovi korisnika
        public ICommand ExitGameCommand { get; private set; }   // Izlaz iz aplikacije
        

        public EndcardDisplayViewModel(Points p) // ViewModel konstruktor za poziv iz Viewa
        {
            Points = p;
            ExitGameCommand = new Command(async () => await Task.Run(() => Environment.Exit(0)));
        }       
    }
}
