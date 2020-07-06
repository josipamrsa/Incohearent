using Incohearent.Data;
using Incohearent.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Incohearent.Controllers
{
    public class LobbyController
    {       
        SQLiteAsyncConnection conn;
        public LobbyController(ISQlite db)
        {
            conn = db.GetConnection();
            //conn.CreateTableAsync<Lobby>();
        }
    }
}
