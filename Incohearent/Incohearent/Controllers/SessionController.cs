using Incohearent.Data;
using Incohearent.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incohearent.Controllers
{
    // Metode za upravljanje zapisima o Sessionu (neiskorišteno zasad)
    public class SessionController : ISessionStore
    {
        SQLiteAsyncConnection conn;

        public SessionController(ISQlite db)
        {
            conn = db.GetConnection();
            conn.CreateTableAsync<Session>();
        }

        public async Task SaveSession(Session session)
        {
            await conn.InsertAsync(session);
        }

        public async Task DeleteSession(Session session)
        {
            await conn.DeleteAsync(session);
        }

        public async Task<Session> GetSession(int id)
        {
            return await conn.FindAsync<Session>(id);
        }
    }
}
