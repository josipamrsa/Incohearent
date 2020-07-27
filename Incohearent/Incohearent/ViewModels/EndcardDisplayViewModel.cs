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
        public Points Points { get; private set; }
        public ICommand ExitGameCommand { get; private set; }
        

        public EndcardDisplayViewModel(Points p)
        {
            Points = p;
            ExitGameCommand = new Command(async () => await Task.Run(() => Environment.Exit(0)));
        }       
    }
}
