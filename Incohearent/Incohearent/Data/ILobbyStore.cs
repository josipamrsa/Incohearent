using Incohearent.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incohearent.Data
{
    public interface ILobbyStore
    {
        Task<Lobby> GetLobby(int id);
        Task AddLobby(Lobby lobby);
        Task UpdateLobby(Lobby lobby);
        Task DeleteLobby(Lobby lobby);
        Task<User> GetUser(int id);
    }
}
