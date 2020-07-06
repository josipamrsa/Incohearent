using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Incohearent.Data
{
    public class PageService : IPageService
    {
        // Navigacija sa stranice na stranicu
        public async Task PushAsync(Page page)
        {
            await MainPage.Navigation.PushAsync(page);
        }

        public async Task<Page> PopAsync()
        {
            return await MainPage.Navigation.PopAsync();
        }
       
        // Prikazat ce sve poruke koje pristizu
        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await MainPage.DisplayAlert(title, message, ok, cancel);
        }

        public async Task DisplayAlert(string title, string message, string ok)
        {
            await MainPage.DisplayAlert(title, message, ok);
        }


        private Page MainPage { get => Application.Current.MainPage; }
    }
}
