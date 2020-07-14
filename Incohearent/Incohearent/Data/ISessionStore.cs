using Incohearent.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incohearent.Data
{
    public interface ISessionStore
    {
        Task SaveSession(Session session);
        Task DeleteSession(Session session);
        Task<Session> GetSession(int id);      
    }
}
