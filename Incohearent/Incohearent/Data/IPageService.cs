﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Incohearent.Data
{
    // Interface za metode za prikaz obavijesti (neiskorišteno zasad)
    public interface IPageService
    {
        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);
        Task DisplayAlert(string title, string message, string ok);
    }
}
