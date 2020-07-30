using Incohearent.Data;
using Incohearent.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Incohearent.Controllers
{
    // Metode za upravljanje zapisima o Lobbyju
    public class LobbyController : ILobbyStore
    {       
        SQLiteAsyncConnection conn;
        
        public LobbyController(ISQlite db)
        {
            conn = db.GetConnection();
            conn.CreateTableAsync<Lobby>();
        }

        public async Task AddLobby(Lobby lobby)
        {
            await conn.InsertAsync(lobby);
        }

        public async Task DeleteLobby(Lobby lobby)
        {
            await conn.DeleteAsync(lobby);
        }

        public async Task<Lobby> GetLobby(int id)
        {
            return await conn.FindAsync<Lobby>(id);
        }

        public async Task UpdateLobby(Lobby lobby)
        {
            await conn.UpdateAsync(lobby);
        }

        public async Task<User> GetUser(int id)
        {
            return await conn.Table<User>().Where(i => i.UserId == id).FirstOrDefaultAsync(); 
        }       
    }
}
