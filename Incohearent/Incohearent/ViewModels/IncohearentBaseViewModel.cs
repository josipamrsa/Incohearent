using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Incohearent.ViewModels
{
    // Bazni ViewModel na kojem ce se temeljiti ostali
    public class IncohearentBaseViewModel : INotifyPropertyChanged // Metode koje obavještavaju kontrolu da se svojstvo promijenilo
    {
        public event PropertyChangedEventHandler PropertyChanged; // Handler za mijenjanje svojstva
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Kad se svojstvo promijeni, poziva se handler i šalje se argument s novom vrijednošću
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Postavljanje vrijednosti svojstva sa stare na novu
        protected void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return;
            backingField = value;
            OnPropertyChanged(propertyName);
        }
    }
}
