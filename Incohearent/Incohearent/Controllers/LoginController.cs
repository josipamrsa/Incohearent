using Incohearent.Data;
using Incohearent.Models;
using Microsoft.Data.Sqlite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Incohearent.Controllers
{
    // Metode za upravljanje tablicom korisnika
    public class LoginController : IUserStore
    {
        private SQLiteAsyncConnection conn;

        public LoginController(ISQlite db) {
            conn = db.GetConnection();
            conn.CreateTableAsync<User>();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await conn.Table<User>().ToListAsync(); // dohvati sve korisnike u listu async
        }

        public async Task DeleteUser(User user)
        {
            await conn.DeleteAsync(user); // izbrisi korisnika async
        }

        public async Task AddUser(User user)
        {
            await conn.InsertAsync(user); // unesi korisnika async
        }

        public async Task UpdateUser(User user)
        {
            await conn.UpdateAsync(user); // azuriraj podatke korisnika async
        }

        public async Task<User> GetUser(int id)
        {
            return await conn.FindAsync<User>(id); // pronadji korisnika po id-u async
        }
    }
}
