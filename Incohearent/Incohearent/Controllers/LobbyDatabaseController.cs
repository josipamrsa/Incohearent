using Incohearent.Data;
using Incohearent.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Incohearent.Controllers
{
    public class LobbyDatabaseController
    {
        static object locker = new object();
        SQLiteConnection db;
        public LobbyDatabaseController()
        {
            db = DependencyService.Get<ISQlite>().GetConnection();
            db.CreateTable<Lobby>();
        }

        public Lobby CreateNewLobby() { return new Lobby(); }

    }
}
