using Incohearent.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incohearent.ViewModels
{
    // MVVM - Praćenje promjena u vrijednosti svojstva za Session model
    public class SessionViewModel : IncohearentBaseViewModel
    {
        public int Id;

        public SessionViewModel() { }
        public SessionViewModel(Session session) { }

        private int userId;
        public int UserId
        {
            get => userId;
            set
            {
                SetValue(ref userId, value);
                OnPropertyChanged(nameof(UserId));
            }
        }

        private int roundNum;
        public int RoundNum
        {
            get => roundNum;
            set
            {
                SetValue(ref roundNum, value);
                OnPropertyChanged(nameof(RoundNum));
            }
        }

        private int playerNum;
        public int PlayerNum
        {
            get => roundNum;
            set
            {
                SetValue(ref playerNum, value);
                OnPropertyChanged(nameof(PlayerNum));
            }
        }

    }
}
